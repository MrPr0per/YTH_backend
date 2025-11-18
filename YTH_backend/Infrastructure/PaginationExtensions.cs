using System.Linq.Expressions;
using System.Reflection;
using YTH_backend.Enums;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Infrastructure;

public static class PaginationExtensions
{
    /// <summary>
    /// Сортирует IQueryable<T> по выбранному параметру
    /// </summary>
    /// <param name="query"></param>
    /// <param name="orderType">Тип сортировки</param>
    /// <param name="fieldName">Имя поля, по которому будет происходить сортировка</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static SortedQueryable<T> ApplyOrderSettings<T>(this IQueryable<T> query, OrderType orderType, string fieldName)
    {
        if (string.IsNullOrEmpty(fieldName))
            throw new ArgumentNullException(nameof(fieldName), "fieldName must not be null or empty");
        
        var property = typeof(T).GetProperty(fieldName, BindingFlags.IgnoreCase);
        
        if (property is null)
            throw new ArgumentException($"Property {fieldName} not found on type {typeof(T).FullName}");
        
        var idProperty = typeof(T).GetProperty("Id", BindingFlags.IgnoreCase);
        
        if (idProperty is null)
            throw new ArgumentException($"Property Id not found on type {typeof(T).FullName}");
        
        //тут нельзя использовать обычный Func так как EF Core не читает рефлексию и будет аллокация в память
        var parameter = Expression.Parameter(typeof(T), "t");
        var propertyAccess = Expression.Property(parameter, property);
        var selector = Expression.Lambda(propertyAccess, parameter);
        
        var idAccess = Expression.Property(parameter, idProperty);
        var idSelector = Expression.Lambda(idAccess, parameter);

        var orderedQuery = orderType == OrderType.Asc
            ? Queryable.ThenBy(Queryable.OrderBy(query, (dynamic)selector), (dynamic)idSelector)
            : Queryable.ThenByDescending(Queryable.OrderByDescending(query, (dynamic)selector), (dynamic)idSelector);
        
        return new SortedQueryable<T>(orderedQuery, fieldName, orderType);
    }

    /// <summary>
    /// Применяет настройку курсора для IQueryable<T>
    /// </summary>
    /// <param name="sortedQuery">Уже отсортированная табличка</param>
    /// <param name="cursorType">Тип курсора</param>
    /// <param name="take">Количество элементов, которые необходимо взять</param>
    /// <param name="cursorId">Id объекта с которого начинается отбор</param>
    /// <param name="idSelector">Функция для получения Id модели</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IQueryable<T> ApplyCursorSettings<T>(this SortedQueryable<T> sortedQuery, CursorType cursorType, int take, Guid? cursorId)
    {
        if (cursorType == CursorType.Default || !cursorId.HasValue)
            return sortedQuery.Query.Take(take);

        var idProperty = typeof(T).GetProperty("Id", BindingFlags.IgnoreCase);
        
        if (idProperty is null)
            throw new ArgumentException($"Property {typeof(T).FullName} not found on type {typeof(T).FullName}");
        
        //тут нельзя использовать обычный Func так как EF Core не читает рефлексию и будет аллокация в память
        var param = Expression.Parameter(typeof(T), "x");
        var idAccess = Expression.Property(param, idProperty);
        var idConst = Expression.Constant(cursorId.Value);
        var lambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(idAccess, idConst), param);
        
        var entity = sortedQuery.Query.FirstOrDefault(lambda);
        
        if (entity == null)
            throw new EntityNotFoundException("Cursor ID not found in the query");
        
        var tProp = typeof(T).GetProperty(sortedQuery.OrderByFieldName, BindingFlags.IgnoreCase);
        
        if (tProp == null)
            throw new InvalidOperationException($"Property {sortedQuery.OrderByFieldName} not found on type {typeof(T).FullName}");
        
        var cursorValue = tProp.GetValue(entity);
        var propAccess = Expression.Property(param, tProp);
        var constant = Expression.Constant(cursorValue);
        
        var comparison = null as Expression;
        
        switch (cursorType)
        {
            case CursorType.After:
                // x => (x.SortField > cursorValue) || (x.SortField == cursorValue && x.Id > cursorId)
                if (sortedQuery.OrderType == OrderType.Asc)
                {
                    comparison = Expression.OrElse(Expression.GreaterThan(propAccess, constant),
                        Expression.AndAlso(
                            Expression.Equal(propAccess, constant),
                            Expression.GreaterThan(idAccess, Expression.Constant(cursorId))));
                }
                // x => (x.SortField < cursorValue) || (x.SortField == cursorValue && x.Id < cursorId)
                else
                {
                    comparison = Expression.OrElse(Expression.LessThan(propAccess, constant),
                        Expression.AndAlso(
                            Expression.Equal(propAccess, constant),
                            Expression.LessThan(idAccess, Expression.Constant(cursorId))));
                }
                
                var lambdaAfter = Expression.Lambda<Func<T, bool>>(comparison, param);
                return sortedQuery.Query.Where(lambdaAfter).Take(take);

            case CursorType.Before:
                // x => (x.SortField < cursorValue) || (x.SortField == cursorValue && x.Id < cursorId)
                if (sortedQuery.OrderType == OrderType.Asc)
                {
                    comparison = Expression.OrElse(Expression.LessThan(propAccess, constant),
                        Expression.AndAlso(
                            Expression.Equal(propAccess, constant),
                            Expression.LessThan(idAccess, Expression.Constant(cursorId))));
                }
                // x => (x.SortField > cursorValue) || (x.SortField == cursorValue && x.Id > cursorId)
                else
                {
                    comparison = Expression.OrElse(Expression.GreaterThan(propAccess, constant),
                        Expression.AndAlso(
                            Expression.Equal(propAccess, constant),
                            Expression.GreaterThan(idAccess, Expression.Constant(cursorId))));
                }
                
                var lambdaBefore = Expression.Lambda<Func<T, bool>>(comparison, param);
                var selectedQueryBefore = sortedQuery.Query.Where(lambdaBefore);
                
                var countBefore = selectedQueryBefore.Count();
                var skip = Math.Max(0, countBefore - take);

                return selectedQueryBefore.Skip(skip).Take(take);
            
            default:
                throw new ArgumentOutOfRangeException(nameof(cursorType), cursorType, null);
        }
    }
}