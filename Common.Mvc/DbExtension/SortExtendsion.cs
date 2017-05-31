using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mvc
{
    public static class SortExtendsion
    {
        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<TEntity>(source, "OrderBy", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        public static IOrderedQueryable<TEntity> OrderByDescending<TEntity>(this IQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<TEntity>(source, "OrderByDescending", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }
        public static IOrderedQueryable<TEntity> ThenBy<TEntity>(this IOrderedQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<TEntity>(source, "ThenBy", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }
        public static IOrderedQueryable<TEntity> ThenByDescending<TEntity>(this IOrderedQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<TEntity>(source, "ThenByDescending", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="fieldNames">排序字段列表</param>
        /// <param name="sortTypes">排序方式列表</param>
        /// <returns></returns>
        public static IOrderedQueryable<TEntity> OrderUsingSortExpression<TEntity>(this IQueryable<TEntity> source, string fieldNames, string sortTypes) where TEntity : class
        {
            Type entityType = typeof(TEntity);
            Dictionary<string, string> propertyNames = entityType.GetProperties().ToDictionary(Key => Key.Name.ToLower(), Value => Value.Name);
            string[] orderFields = fieldNames.Split(',');
            string[] orderSorts = sortTypes.Split(',');
            IOrderedQueryable<TEntity> result = null;
            for (int i = 0; i < orderFields.Length; i++)
            {
                string sortField = propertyNames[orderFields[i].ToLower()];
                bool sortDescending = (orderSorts[i].Equals("DESC", StringComparison.OrdinalIgnoreCase));
                if (sortDescending)
                {
                    result = i == 0 ? source.OrderByDescending(sortField) : result.ThenByDescending(sortField);
                }
                else
                {
                    result = i == 0 ? source.OrderBy(sortField) : result.ThenBy(sortField);
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortExpression">排序表达式，例(id desc,username asc)</param>
        /// <returns></returns>
        public static IOrderedQueryable<TEntity> OrderUsingSortExpression<TEntity>(this IQueryable<TEntity> source, string sortExpression) where TEntity : class
        {
            Type type = typeof(TEntity);
            string[] properties = type.GetProperties().Select(c => c.Name).ToArray();
            string[] orderFields = sortExpression.Split(',');
            IOrderedQueryable<TEntity> result = null;
            for (int i = 0; i < orderFields.Length; i++)
            {
                string[] expressionPart = orderFields[i].Trim().Split(' ');
                string sortField = properties.FirstOrDefault(c=>c.Equals(expressionPart[0],StringComparison.OrdinalIgnoreCase));
                bool sortDescending = (expressionPart.Length == 2) && (expressionPart[1].Equals("DESC", StringComparison.OrdinalIgnoreCase));
                if (sortDescending)
                {
                    result = i == 0 ? source.OrderByDescending(sortField) : result.ThenByDescending(sortField);
                }
                else
                {
                    result = i == 0 ? source.OrderBy(sortField) : result.ThenBy(sortField);
                }
            }
            return result;
        }
        private static MethodCallExpression GenerateMethodCall<TEntity>(IQueryable<TEntity> source, string methodName, String fieldName) where TEntity : class
        {
            Type entityType = typeof(TEntity);
            Type propertyType;
            LambdaExpression selector = GenerateSelector<TEntity>(fieldName, out propertyType);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName,
            new Type[] { entityType, propertyType },
            source.Expression, Expression.Quote(selector));
            return resultExp;
        }
        private static LambdaExpression GenerateSelector<TEntity>(String propertyName, out Type propertyType) where TEntity : class
        {
            var parameter = Expression.Parameter(typeof(TEntity), "c");
            PropertyInfo property;
            Expression propertyAccess;
            if (propertyName.Contains('.'))
            {
                String[] childProperties = propertyName.Split('.');
                property = typeof(TEntity).GetProperty(childProperties[0]);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
                for (int i = 1; i < childProperties.Length; i++)
                {
                    property = property.PropertyType.GetProperty(childProperties[i]);
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = typeof(TEntity).GetProperty(propertyName);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }
            propertyType = property.PropertyType;
            return Expression.Lambda(propertyAccess, parameter);
        }
    }
}
