﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace DL.Core.Linq
{
    /// <summary>Extension methods for LINQ.</summary>
    public static class LinqExtensions
    {
        public static IQueryable<TSource> SortBy<TSource, TKey>(this IQueryable<TSource> source, Func<TSource, TKey> keySelector, bool ascending = true)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (keySelector == null)
            {
                return source;
            }

            return @ascending 
                ? source.OrderBy(keySelector).AsQueryable() 
                : source.OrderByDescending(keySelector).AsQueryable();
        }

        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string propertyName, bool ascending = true)
        {
            if (source == null) 
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrEmpty(propertyName)) 
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, String.Empty);
            MemberExpression property = Expression.Property(parameter, propertyName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = ascending ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression 
                = Expression.Call(typeof(Queryable), 
                    methodName, 
                    new Type[] { source.ElementType, property.Type },
                    source.Expression, 
                    Expression.Quote(lambda));
            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return condition ? source.Where(predicate) : source;
        }
    }
}
