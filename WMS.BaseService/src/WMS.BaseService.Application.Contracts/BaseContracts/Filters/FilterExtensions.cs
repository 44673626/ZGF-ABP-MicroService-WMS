using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WMS.BaseService.BaseContracts.Filters
{
    /// <summary>
    /// 动态过滤扩展
    /// </summary>
    public static class FilterExtensions
    {
        public static Expression<Func<T, bool>> ToLambda<T>(this string jsonFilter)
        {
            if (string.IsNullOrWhiteSpace(jsonFilter))
            {
                return p => true;
            }

            var filterConditions = JsonSerializer.Deserialize<List<FilterCondition>>(jsonFilter);
            return filterConditions.ToExpression<T>();
        }

        public static Expression<Func<T, bool>> ToLambda<T>(this FilterCondition filterCondition)
        {
            var filterConditions = new List<FilterCondition> { filterCondition };
            return filterConditions.ToExpression<T>();
        }

        public static Expression<Func<T, bool>> ToExpression<T>(this List<FilterCondition> filterConditionList)
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
                            FilterLogic.And => condition.And(tempCondition),
                            FilterLogic.Or => condition.Or(tempCondition),
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

        private static Expression<Func<T, bool>> CreateLambda<T>(FilterCondition filterCondition)
        {
            Expression<Func<T, bool>> expression = p => false;
            try
            {
                var parameter = Expression.Parameter(typeof(T), "p"); //创建参数p
                var member = Expression.PropertyOrField(parameter, filterCondition.Column); //创建表达式中的属性或字段
                // var propertyType = member.Type; //取属性类型,常量constant按此类型进行转换
                //var constant = Expression.Constant(filterCondition.Value);//创建常数

                ConstantExpression constant = null;
                if (filterCondition.Action != FilterAction.In
                    && filterCondition.Action != FilterAction.NotIn)
                {
                    constant = CreateConstantExpression(member.Type, filterCondition.Value!);
                }

                switch (filterCondition.Action)
                {
                    case FilterAction.Equal:
                        expression = Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameter);
                        break;
                    case FilterAction.NotEqual:
                        expression = Expression.Lambda<Func<T, bool>>(Expression.NotEqual(member, constant), parameter);
                        break;
                    case FilterAction.BiggerThan:
                        expression = Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(member, constant), parameter);
                        break;
                    case FilterAction.SmallThan:
                        if (member.Type == typeof(DateTime?) && constant.Type == typeof(DateTime))
                        {
                            constant = Expression.Constant(((DateTime?)constant.Value), typeof(DateTime?));
                        }
                        expression = Expression.Lambda<Func<T, bool>>(
                            Expression.LessThan(member, constant),
                            parameter);
                        break;
                    case FilterAction.BiggerThanOrEqual:
                        // 确保比较的双方类型匹配
                        if (member.Type == typeof(DateTime?) && constant.Type == typeof(DateTime))
                        {
                            // 将非空 DateTime 转换为可空 DateTime?
                            constant = Expression.Constant(((DateTime?)constant.Value), typeof(DateTime?));
                        }
                        expression = Expression.Lambda<Func<T, bool>>(
                            Expression.GreaterThanOrEqual(member, constant),
                            parameter);
                        break;
                    case FilterAction.SmallThanOrEqual:
                        expression = Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(member, constant), parameter);
                        break;
                    case FilterAction.Like:
                        expression = GetExpressionLikeMethod<T>("Contains", filterCondition);
                        break;
                    case FilterAction.NotLike:
                        expression = GetExpressionNotLikeMethod<T>("Contains", filterCondition);
                        break;
                    case FilterAction.In:
                        expression = GetExpressionInMethod<T>("Contains", member.Type, filterCondition);
                        break;
                    case FilterAction.NotIn:
                        expression = GetExpressionNotInMethod<T>("Contains", member.Type, filterCondition);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    if (propertyType.GetGenericArguments()[0].Name == "Guid")
                    {
                        constant = Expression.Constant(Guid.TryParse(value, out Guid guid) ? guid : null, typeof(Guid?));
                    }
                    else
                        constant = Expression.Constant(Convert.ChangeType(value, propertyType.GetGenericArguments()[0]));
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
                        _ => Expression.Constant(Convert.ChangeType(value, propertyType))
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"获取ConstantExpression异常:{ex.Message}");
            }

            return constant;
        }

        private static Expression<Func<T, bool>> GetExpressionLikeMethod<T>(string methodName,
            FilterCondition filterCondition)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "p");
            //  MethodCallExpression methodExpression = GetMethodExpression(methodName, filterCondition.Column, filterCondition.Value, parameterExpression);
            var methodExpression = GetMethodExpression(methodName, filterCondition.Column, filterCondition.Value,
                parameterExpression);
            return Expression.Lambda<Func<T, bool>>(methodExpression, parameterExpression);
        }

        private static Expression<Func<T, bool>> GetExpressionNotLikeMethod<T>(string methodName,
            FilterCondition filterCondition)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "p");
            var methodExpression = GetMethodExpression(methodName, filterCondition.Column, filterCondition.Value,
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
        /// <param name="filterCondition">PropertyName/PropertyValue</param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> GetExpressionInMethod<T>(string methodName, Type propertyType, FilterCondition filterCondition)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "p");
            Type lstType = typeof(List<>).MakeGenericType(propertyType);
            object propertyValue = JsonSerializer.Deserialize(filterCondition.Value, lstType);
            if (propertyValue != null)
            {
                var methodExpression = GetListMethodExpression(methodName, propertyType, filterCondition.Column, propertyValue, parameterExpression);
                var expression = Expression.Lambda<Func<T, bool>>(methodExpression, parameterExpression);
                return expression;
            }
            else
            {
                return p => false;
            }
        }

        private static Expression<Func<T, bool>> GetExpressionNotInMethod<T>(string methodName, Type propertyType, FilterCondition filterCondition)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "p");
            Type lstType = typeof(List<>).MakeGenericType(propertyType);
            object propertyValue = JsonSerializer.Deserialize(filterCondition.Value!, lstType)!;
            if (propertyValue != null)
            {
                var methodExpression = GetListMethodExpression(methodName, propertyType, filterCondition.Column, propertyValue, parameterExpression);
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
        private static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> exp,
            Expression<Func<T, bool>> condition)
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
        private static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exp,
            Expression<Func<T, bool>> condition)
        {
            var inv = Expression.Invoke(condition, exp.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.And(exp.Body, inv), exp.Parameters);
        }

    }
}
