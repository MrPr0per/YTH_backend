using System.Linq.Expressions;
using YTH_backend.Enums;
using YTH_backend.Infrastructure.Exceptions;

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
    public static IQueryable<T> ApplyOrderSettings<T>(this IQueryable<T> query, OrderType orderType, string fieldName)
    {
        if (string.IsNullOrEmpty(fieldName))
            throw new ArgumentNullException(nameof(fieldName), "fieldName must not be null or empty");
        
        var property = typeof(T).GetProperty(fieldName);
        
        if (property is null)
            throw new ArgumentException($"Property {fieldName} not found on type {typeof(T).FullName}");
        
        var parameter = Expression.Parameter(typeof(T), "t");
        var propertyAccess = Expression.Property(parameter, property);
        var selector = Expression.Lambda(propertyAccess, parameter);

        return orderType == OrderType.Asc
            ? Queryable.OrderBy(query, (dynamic)selector)
            : Queryable.OrderByDescending(query, (dynamic)selector);
    }

    /// <summary>
    /// Применяет настройку курсора для IQueryable<T>
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cursorType">Тип курсора</param>
    /// <param name="take">Количество элементов, которые необходимо взять</param>
    /// <param name="cursorId">Id объекта с которого начинается отбор</param>
    /// <param name="idSelector">Функция для получения Id модели</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IQueryable<T> ApplyCursorSettings<T>(this IQueryable<T> query, CursorType cursorType, int take, Guid? cursorId, Func<T, Guid> idSelector)
    {
        if (cursorType == CursorType.Default || !cursorId.HasValue)
            return query.Take(take);
        
        var ids = query.AsEnumerable().Select(idSelector).ToList();
        var index = ids.IndexOf(cursorId.Value);
        
        if (index == -1)
            throw new EntityNotFoundException("Cursor ID not found in the query");
        
        switch (cursorType)
        {
            case CursorType.After:
                return query
                    .Skip(index + 1)
                    .Take(take);

            case CursorType.Before:
                var skip = index - take >= 0 ? index - take : 0;
                var takeCount = Math.Min(take, index);
                return query
                    .Skip(skip)
                    .Take(takeCount);
            default:
                throw new ArgumentOutOfRangeException(nameof(cursorType), cursorType, null);
        }
        
        return query;
    }
}