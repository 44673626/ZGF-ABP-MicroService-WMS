using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace WMS.BaseService.Bases
{
    /// <summary>
    /// 基础查询接口扩展
    /// </summary>
    public static class QueryableExtension
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName)
        {
            return _OrderBy<T>(query, propertyName, false);
        }
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName)
        {
            return _OrderBy<T>(query, propertyName, true);
        }

        private static IOrderedQueryable<T> _OrderBy<T>(IQueryable<T> query, string propertyName, bool isDesc)
        {
            string methodName = (isDesc) ? "OrderByDescendingInternal" : "OrderByInternal";

            BindingFlags flag = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance;
            var memberProp = typeof(T).GetProperty(propertyName, flag);
            if (memberProp == null)
            {
                throw new UserFriendlyException($"{propertyName} 不是有效名称, 无法排序.");
            }

            var method = typeof(QueryableExtension).GetMethod(methodName)?.MakeGenericMethod(typeof(T), memberProp?.PropertyType);
            var result = method?.Invoke(null, new object[] { query, memberProp });
            return (IOrderedQueryable<T>)result;
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, Dictionary<string, bool> orderExpressions)
        {
            object tempQuery = query;
            var n = 0;
            foreach (var orderExpr in orderExpressions)
            {
                var propertyName = orderExpr.Key;

                var memberProp = typeof(T).GetProperty(propertyName);

                string methodName;
                if (n == 0)
                    methodName = orderExpr.Value ? "OrderByInternal" : "OrderByDescendingInternal";
                else
                    methodName = orderExpr.Value ? "ThenByInternal" : "ThenByDescendingInternal";
                var method = typeof(QueryableExtension).GetMethod(methodName)?.MakeGenericMethod(typeof(T), memberProp?.PropertyType);
                tempQuery = method?.Invoke(null, new object[] { tempQuery, memberProp });
                n++;
            }



            return (IOrderedQueryable<T>)tempQuery;
        }



        public static IOrderedQueryable<T> OrderByInternal<T, TProp>(IQueryable<T> query, PropertyInfo memberProperty)
        {//public
            return query.OrderBy(_GetLambda<T, TProp>(memberProperty));
        }
        public static IOrderedQueryable<T> OrderByDescendingInternal<T, TProp>(IQueryable<T> query, PropertyInfo memberProperty)
        {//public
            return query.OrderByDescending(_GetLambda<T, TProp>(memberProperty));
        }

        public static IOrderedQueryable<T> ThenByInternal<T, TProp>(IOrderedQueryable<T> query, PropertyInfo memberProperty)
        {//public
            return query.ThenBy(_GetLambda<T, TProp>(memberProperty));
        }
        public static IOrderedQueryable<T> ThenByDescendingInternal<T, TProp>(IOrderedQueryable<T> query, PropertyInfo memberProperty)
        {//public
            return query.ThenByDescending(_GetLambda<T, TProp>(memberProperty));
        }

        private static Expression<Func<T, TProp>> _GetLambda<T, TProp>(PropertyInfo memberProperty)
        {
            if (memberProperty.PropertyType != typeof(TProp)) throw new Exception();

            var thisArg = Expression.Parameter(typeof(T));
            var lamba = Expression.Lambda<Func<T, TProp>>(Expression.Property(thisArg, memberProperty), thisArg);

            return lamba;
        }


    }
}
