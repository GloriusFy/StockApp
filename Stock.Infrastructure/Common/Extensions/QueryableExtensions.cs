using System.Linq.Expressions;
using StringToExpression.LanguageDefinitions;

namespace Stock.Infrastructure.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, string oDataFilterString)
    {
        try
        {
            return ApplyFilterInternal(query, oDataFilterString);
        }
        catch(Exception e)
        {
            throw new FormatException($"The specified filter string '{oDataFilterString}' is invalid.", e);
        }
    }
    
    public static IQueryable<T> ApplyOrder<T>(this IQueryable<T> query, string oDataOrderByString, int maximumNumberOfOrdering = 5)
    {
        try
        {
            return ApplyOrderInternal(query, oDataOrderByString, maximumNumberOfOrdering);
        }
        catch(Exception e)
        {
            throw new FormatException($"The specified orderBy string '{oDataOrderByString}' is invalid.", e);
        }
    }
    
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int pageSize, int pageIndex)
            => query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

        private static IQueryable<T> ApplyFilterInternal<T>(IQueryable<T> query, string oDataFilterString)
        {
            if (string.IsNullOrWhiteSpace(oDataFilterString))
            {
                return query;
            }

            var filterExpression = new ODataFilterLanguage().Parse<T>(oDataFilterString);
            return query.Where(filterExpression);
        }

        private static IQueryable<T> ApplyOrderInternal<T>(IQueryable<T> query, string oDataOrderByString, int maximumNumberOfOrdering)
        {
            if (string.IsNullOrWhiteSpace(oDataOrderByString))
            {
                return query;
            }

            var firstOrdering = true;
            foreach (var (propertyName, order) in GetOrderEntries(oDataOrderByString, maximumNumberOfOrdering))
            {
                query = ApplyOrdering(query, propertyName, order, firstOrdering);
                firstOrdering = false;
            }

            return query;

            static IEnumerable<(string propertyPath, SortOrder order)> GetOrderEntries(string orderByString, int maxOrders)
            {
                return orderByString
                    .Split(',', count: maxOrders, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(orderStr =>
                    {
                        var divider = orderStr.IndexOf(' ');
                        if (divider < 0) return (propertyPath: orderStr, order: SortOrder.Asc);
                        else return (
                            propertyPath: orderStr[0..divider],
                            order: Enum.Parse<SortOrder>(orderStr[divider..].Trim(), ignoreCase: true)
                        );
                    });
            }

            static IQueryable<T> ApplyOrdering(IQueryable<T> query, string propertyPath, SortOrder order, bool firstOrdering)
            {
                var param = Expression.Parameter(typeof(T), "p");
                var member = (MemberExpression)propertyPath.Split('/').Aggregate((Expression)param, Expression.Property); //Expression.Property(param, propertyPath);
                var exp = Expression.Lambda(member, param);
                string methodName = order switch
                {
                    SortOrder.Asc => firstOrdering ? "OrderBy" : "ThenBy",
                    SortOrder.Desc => firstOrdering ? "OrderByDescending" : "ThenByDescending"
                };
                Type[] types = new Type[] { query.ElementType, exp.Body.Type };
                var orderByExpression = Expression.Call(typeof(Queryable), methodName, types, query.Expression, exp);
                return query.Provider.CreateQuery<T>(orderByExpression);
            }
        }

        private enum SortOrder
        {
            Asc,
            Desc
        }
}