using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WMS.BaseService.CommonManagement.Filters
{
    public static class FilterExtensions
    {
        public static Expression<Func<T, bool>> ToLambda<T>(this string jsonFilter)
        {
            if (string.IsNullOrWhiteSpace(jsonFilter))
            {
                return p => true;
            }

            var filterConditions = JsonSerializer.Deserialize<List<Filter>>(jsonFilter);
            return filterConditions.ToLambda<T>();
        }

        public static Expression<Func<T, bool>> ToLambda<T>(this Filter filter)
        {
            var filterConditions = new List<Filter> { filter };
            return filterConditions.ToLambda<T>();
        }

        public static Expression<Func<T, bool>> ToLambda<T>(this ICollection<Filter> filterConditionList)
        {
            Expression<Func<T, bool>> condition = null;
            try
            {
                if (!filterConditionList.Any())
                {
                    //创建默认表达式
                    return p => true;
                }

                foreach (var filterCondition in filterConditionList)
                {
                    var tempCondition = CreateLambda<T>(filterCondition);
                    if (condition == null)
                    {
                        condition = tempCondition;
                    }
                    else
                    {
                        condition = filterCondition.Logic switch
                        {
                            "And" => condition.And(tempCondition),
                            "Or" => condition.Or(tempCondition),
                            _ => condition
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"获取筛选条件异常:{ex.Message}");
            }

            return condition;
        }

        private static Expression<Func<T, bool>> CreateLambda<T>(Filter filter)
        {
            Expression<Func<T, bool>> expression = p => false;
            try
            {
                var parameter = Expression.Parameter(typeof(T), "p"); //创建参数p
                var member = Expression.PropertyOrField(parameter, filter.Column); //创建表达式中的属性或字段
                // var propertyType = member.Type; //取属性类型,常量constant按此类型进行转换
                //var constant = Expression.Constant(filterCondition.Value);//创建常数

                ConstantExpression constant = null;
                if (filter.Action != "In" && filter.Action != "NotIn")
                {
                    constant = CreateConstantExpression(member.Type, filter.Value);
                }

                switch (filter.Action.ToLower())
                {
                    case "==":
                        expression = Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameter);
                        break;
                    case "!=":
                        expression = Expression.Lambda<Func<T, bool>>(Expression.NotEqual(member, constant), parameter);
                        break;
                    case ">":
                        expression = Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(member, constant), parameter);
                        break;
                    case "<":
                        expression = Expression.Lambda<Func<T, bool>>(Expression.LessThan(member, constant), parameter);
                        break;
                    case ">=":
                        expression = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(member, constant), parameter);
                        break;
                    case "<=":
                        expression = Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(member, constant), parameter);
                        break;
                    case "like":
                        expression = GetExpressionLikeMethod<T>("Contains", filter);
                        break;
                    case "notlike":
                        expression = GetExpressionNotLikeMethod<T>("Contains", filter);
                        break;
                    case "in":
                        expression = GetExpressionInMethod<T>("Contains", member.Type, filter);
                        break;
                    case "notin":
                        expression = GetExpressionNotInMethod<T>("Contains", member.Type, filter);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return expression;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static ConstantExpression CreateConstantExpression(Type propertyType, string value)
        {
            ConstantExpression constant = null;
            try
            {
                if (propertyType.IsGenericType &&
                    propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var objValue = Convert.ChangeType(value, propertyType.GetGenericArguments()[0], CultureInfo.InvariantCulture);
                    constant = Expression.Constant(objValue);
                }
                else if (propertyType.IsEnum)
                {
                    var enumValue = (Enum)Enum.Parse(propertyType, value, true);
                    constant = Expression.Constant(enumValue);
                }

                else
                {
                    constant = propertyType.Name switch
                    {
                        "Guid" => Expression.Constant(Guid.Parse(value)),
                        _ => Expression.Constant(Convert.ChangeType(value, propertyType, CultureInfo.InvariantCulture))
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"获取ConstantExpression异常:{ex.Message}");
            }

            return constant;
        }

        private static Expression<Func<T, bool>> GetExpressionLikeMethod<T>(string methodName, Filter filter)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "p");
            //  MethodCallExpression methodExpression = GetMethodExpression(methodName, filterCondition.Column, filterCondition.Value, parameterExpression);
            var methodExpression = GetMethodExpression(methodName, filter.Column, filter.Value,
                parameterExpression);
            return Expression.Lambda<Func<T, bool>>(methodExpression, parameterExpression);
        }

        private static Expression<Func<T, bool>> GetExpressionNotLikeMethod<T>(string methodName, Filter filter)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "p");
            var methodExpression = GetMethodExpression(methodName, filter.Column, filter.Value,
                parameterExpression);
            var notMethodExpression = Expression.Not(methodExpression);
            return Expression.Lambda<Func<T, bool>>(notMethodExpression, parameterExpression);
        }


        private static object GetPropertyValue(Type propertyType, string value)
        {
            Type lstType = typeof(List<>).MakeGenericType(propertyType);
            return JsonSerializer.Deserialize(value, lstType);
        }

        /// <summary>
        /// 生成guidList.Contains(p=>p.GUId);
        /// 除String类型,其他类型涉及到类型转换.如GUID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodName">Contains</param>
        /// <param name="propertyType">PropertyType/typeof(GUId)</param>
        /// <param name="filter">PropertyName/PropertyValue</param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> GetExpressionInMethod<T>(string methodName, Type propertyType, Filter filter)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "p");
            Type lstType = typeof(List<>).MakeGenericType(propertyType);
            object propertyValue = JsonSerializer.Deserialize(filter.Value, lstType);
            if (propertyValue != null)
            {
                var methodExpression = GetListMethodExpression(methodName, propertyType, filter.Column, propertyValue, parameterExpression);
                var expression = Expression.Lambda<Func<T, bool>>(methodExpression, parameterExpression);
                return expression;
            }
            else
            {
                return p => false;
            }
        }

        private static Expression<Func<T, bool>> GetExpressionNotInMethod<T>(string methodName, Type propertyType, Filter filter)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "p");
            Type lstType = typeof(List<>).MakeGenericType(propertyType);
            object propertyValue = JsonSerializer.Deserialize(filter.Value, lstType);
            if (propertyValue != null)
            {
                var methodExpression = GetListMethodExpression(methodName, propertyType, filter.Column, propertyValue, parameterExpression);
                var notMethodExpression = Expression.Not(methodExpression);
                return Expression.Lambda<Func<T, bool>>(notMethodExpression, parameterExpression);
            }
            else
            {
                return p => false;
            }
        }


        private static MethodCallExpression GetListMethodExpression(string methodName, Type propertyType, string propertyName, object propertyValue, ParameterExpression parameterExpression)
        {
            var propertyExpression = Expression.Property(parameterExpression, propertyName); //p.GUID
            Type type = typeof(List<>).MakeGenericType(propertyType);
            var method = type.GetMethod(methodName);//获取 List.Contains()
            var someValue = Expression.Constant(propertyValue);//Value
            return Expression.Call(someValue, method, propertyExpression);
        }


        /// <summary>
        /// 生成类似于p=>p.Code.Contains("xxx");的lambda表达式
        /// parameterExpression标识p，propertyName表示values，propertyValue表示"Code",methodName表示Contains
        /// 仅处理p的属性类型为string这种情况
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="parameterExpression"></param>
        /// <returns></returns>
        private static MethodCallExpression GetMethodExpression(string methodName, string propertyName,
            string propertyValue, ParameterExpression parameterExpression)
        {
            var propertyExpression = Expression.Property(parameterExpression, propertyName);
            var method = typeof(string).GetMethod(methodName, new[] { typeof(string) });
            var someValue = Expression.Constant(propertyValue, typeof(string));
            return Expression.Call(propertyExpression, method, someValue);
        }

        /// <summary>
        /// 默认True条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return f => true;
        }

        /// <summary>
        /// 默认False条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return f => false;
        }

        /// <summary>
        /// 拼接 OR 条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> exp, Expression<Func<T, bool>> condition)
        {
            var inv = Expression.Invoke(condition, exp.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.Or(exp.Body, inv), exp.Parameters);
        }

        /// <summary>
        /// 拼接And条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exp, Expression<Func<T, bool>> condition)
        {
            var inv = Expression.Invoke(condition, exp.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.And(exp.Body, inv), exp.Parameters);
        }

    }
}
