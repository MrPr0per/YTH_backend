using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
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
        
        var property = typeof(T).GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        
        if (property is null)
            throw new ArgumentException($"Property {fieldName} not found on type {typeof(T).FullName}");
        
        var idProperty = typeof(T).GetProperty("Id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        
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
    public static IQueryable<T> ApplyCursorSettings<T>(this SortedQueryable<T> sortedQuery, CursorType cursorType,
        int take, Guid? cursorId)
    {
        if (cursorType == CursorType.Default || !cursorId.HasValue)
            return sortedQuery.Query.Take(take);

        var idProperty =
            typeof(T).GetProperty("Id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

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

        var tProp = typeof(T).GetProperty(sortedQuery.OrderByFieldName,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (tProp == null)
            throw new InvalidOperationException(
                $"Property {sortedQuery.OrderByFieldName} not found on type {typeof(T).FullName}");

        var cursorValue = tProp.GetValue(entity);
        var propAccess = Expression.Property(param, tProp);
        var constant = Expression.Constant(cursorValue);

        var comparison = null as Expression;
        var idConstForCompare = Expression.Constant(cursorId.Value, idProperty.PropertyType);
        var equalOnProp = Expression.Equal(propAccess, constant);

        switch (cursorType)
        {
            case CursorType.After:
                // x => (x.SortField > cursorValue) || (x.SortField == cursorValue && x.Id > cursorId)
                if (sortedQuery.OrderType == OrderType.Asc)
                {
                    comparison = Expression.OrElse(
                        BuildComparison(propAccess, constant, wantGreater: true),
                        Expression.AndAlso(
                            equalOnProp,
                            BuildComparison(idAccess, idConstForCompare, wantGreater: true)
                        )
                    );
                }
                // x => (x.SortField < cursorValue) || (x.SortField == cursorValue && x.Id < cursorId)
                else
                {
                    comparison = Expression.OrElse(
                        BuildComparison(propAccess, constant, wantGreater: false),
                        Expression.AndAlso(
                            equalOnProp,
                            BuildComparison(idAccess, idConstForCompare, wantGreater: false)
                        )
                    );
                }

                var lambdaAfter = Expression.Lambda<Func<T, bool>>(comparison, param);
                return sortedQuery.Query.Where(lambdaAfter).Take(take);

            case CursorType.Before:
                // x => (x.SortField < cursorValue) || (x.SortField == cursorValue && x.Id < cursorId)
                if (sortedQuery.OrderType == OrderType.Asc)
                {
                    comparison = Expression.OrElse(
                        BuildComparison(propAccess, constant, wantGreater: false),
                        Expression.AndAlso(
                            equalOnProp,
                            BuildComparison(idAccess, idConstForCompare, wantGreater: false)
                        )
                    );
                }
                // x => (x.SortField > cursorValue) || (x.SortField == cursorValue && x.Id > cursorId)
                else
                {
                    comparison = Expression.OrElse(
                        BuildComparison(propAccess, constant, wantGreater: true),
                        Expression.AndAlso(
                            equalOnProp,
                            BuildComparison(idAccess, idConstForCompare, wantGreater: true)
                        )
                    );
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

    private static Expression BuildComparison(Expression left, Expression right, bool wantGreater)
    {
        var propType = left.Type;

        // if both are value types but boxed constant might be object, ensure same type
        if (left.Type != right.Type)
            right = Expression.Convert(right, left.Type);

        // Numbers, DateTime, Guid etc. that support direct comparison operators
        if (IsSimpleComparable(propType))
        {
            return wantGreater ? Expression.GreaterThan(left, right) : Expression.LessThan(left, right);
        }

        // строки -> используем PgDbFunctionsExtensions
        if (propType == typeof(string))
        {
            // Генерируем: left.CompareTo(right) > 0 или left.CompareTo(right) < 0
            var compareToMethod = typeof(string).GetMethod("CompareTo", new[] { typeof(string) });
            
            var compareToCallStr = Expression.Call(left, compareToMethod!, right);

            return wantGreater
                ? Expression.GreaterThan(compareToCallStr, Expression.Constant(0))
                : Expression.LessThan(compareToCallStr, Expression.Constant(0));
        }

        // fallback to IComparable: ((IComparable)left).CompareTo((object)right) > 0 / < 0
        var iComparableType = typeof(IComparable);
        var leftAsIComparable = Expression.Convert(left, iComparableType);
        var rightAsObject = Expression.Convert(right, typeof(object));
        var compareTo = iComparableType.GetMethod("CompareTo", new[] { typeof(object) })!;
        var compareToCall = Expression.Call(leftAsIComparable, compareTo, rightAsObject);
        return wantGreater
            ? Expression.GreaterThan(compareToCall, Expression.Constant(0))
            : Expression.LessThan(compareToCall, Expression.Constant(0));
    }
    
    private static bool IsSimpleComparable(Type t)
    {
        // types that have builtin <, > operators that Expression supports:
        var underlying = Nullable.GetUnderlyingType(t) ?? t;
        return underlying.IsPrimitive
               || underlying == typeof(decimal)
               || underlying == typeof(DateTime)
               || underlying == typeof(DateTimeOffset)
               || underlying == typeof(Guid)
               || underlying.IsEnum;
    }
}