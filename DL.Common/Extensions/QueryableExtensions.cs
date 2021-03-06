﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace DL.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TSource> SortBy<TSource, TKey>(this IQueryable<TSource> source, Func<TSource, TKey> keySelector, bool isAscending = true)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                return source;
            }

            return isAscending
                ? source.OrderBy(keySelector).AsQueryable()
                : source.OrderByDescending(keySelector).AsQueryable();
        }

        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string propertyName, bool ascending = true)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, string.Empty);
            MemberExpression property = Expression.Property(parameter, propertyName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            var methodName = ascending ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression
                = Expression.Call(typeof(Queryable),
                    methodName,
                    new[] { source.ElementType, property.Type },
                    source.Expression,
                    Expression.Quote(lambda));
            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return condition ? source.Where(predicate) : source;
        }
    }
}
