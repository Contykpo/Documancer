﻿using System.Linq.Expressions;

namespace Documancer.Server.Extensions
{
    public static class DataGridExtensions
    {
        public static IQueryable<T> EfOrderBySortDefinitions<T, T1>(this IQueryable<T> source, GridState<T1> state)
        {
            return source.EfOrderBySortDefinitions(state.SortDefinitions);
        }

        public static IQueryable<T> EfOrderBySortDefinitions<T, T1>(this IQueryable<T> source,
            ICollection<SortDefinition<T1>> sortDefinitions)
        {
            // Avoid multiple enumeration.
            var sourceQuery = source;

            if (sortDefinitions.Count == 0)
                return sourceQuery;

            IOrderedQueryable<T>? orderedQuery = null;

            foreach (var sortDefinition in sortDefinitions)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var orderByProperty = Expression.Property(parameter, sortDefinition.SortBy);
                var sortLambda = Expression.Lambda(orderByProperty, parameter);
                
                if (orderedQuery is null)
                {
                    var sortMethod = typeof(Queryable)
                        .GetMethods()
                        // Ensure selecting the right overload.
                        .Where(m => m.Name == (sortDefinition.Descending ? "OrderByDescending" : "OrderBy") && m.IsGenericMethodDefinition)
                        .Single(m => m.GetParameters().ToList().Count == 2);
                    
                    var genericMethod = sortMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
                    
                    orderedQuery = (IOrderedQueryable<T>?)genericMethod.Invoke(genericMethod, new object[] { source, sortLambda });
                }
                else
                {
                    var sortMethod = typeof(Queryable)
                        .GetMethods()
                        // Ensure selecting the right overload.
                        .Where(m => m.Name == (sortDefinition.Descending ? "ThenByDescending" : "ThenBy") && m.IsGenericMethodDefinition)
                        .Single(m => m.GetParameters().ToList().Count == 2);
                    
                    var genericMethod = sortMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
                    
                    orderedQuery = (IOrderedQueryable<T>?)genericMethod.Invoke(genericMethod, new object[] { source, sortLambda });
                }
            }

            return orderedQuery ?? sourceQuery;
        }
    }
}