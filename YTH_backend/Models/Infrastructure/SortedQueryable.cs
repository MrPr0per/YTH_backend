using System.Security.Cryptography.X509Certificates;
using YTH_backend.Enums;

namespace YTH_backend.Models.Infrastructure;

public record SortedQueryable<T>(IQueryable<T> Query, string OrderByFieldName, OrderType OrderType);