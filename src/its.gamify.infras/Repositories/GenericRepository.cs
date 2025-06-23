using its.gamify.core.Repositories;
using its.gamify.core.Services.Interfaces;
using its.gamify.domains.Entities;
using its.gamify.domains.Models;
using its.gamify.infras.Datas;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.Json;

namespace its.gamify.infras.Repositories;

public class GenericRepository<TEntity>(
    AppDbContext context,
    ICurrentTime currentTime,
    IClaimsService claimsService
) : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    private readonly ICurrentTime _timeService = currentTime;
    private readonly IClaimsService _claimsService = claimsService;

    #region Query Helpers


    private IQueryable<TEntity> ApplyBaseFilters(
        IQueryable<TEntity> query,
        bool withDeleted = false,
        Expression<Func<TEntity, bool>>? filter = null)
    {
        if (!withDeleted)
        {
            query = query.Where(x => !x.IsDeleted);
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query;
    }

    private IQueryable<TEntity> ApplyIncludes(
        IQueryable<TEntity> query,
        params Expression<Func<TEntity, object>>[] includes)
    {
        if (includes == null || includes.Length == 0)
        {
            return query;
        }

        return includes.Aggregate(
            query,
            (current, include) => current.Include(include));
    }

    private IQueryable<TEntity> ApplyOrdering(
        IQueryable<TEntity> query,
        List<(Expression<Func<TEntity, object>> OrderBy, bool IsDescending)>? orderByList = null)
    {
        if (orderByList == null || orderByList.Count == 0)
        {
            return query.OrderByDescending(x => x.CreatedDate);
        }

        var isFirstOrder = true;
        IOrderedQueryable<TEntity>? orderedQuery = null;

        foreach (var (orderBy, isDescending) in orderByList)
        {
            if (isFirstOrder)
            {
                orderedQuery = isDescending
                    ? query.OrderByDescending(orderBy)
                    : query.OrderBy(orderBy);
                isFirstOrder = false;
            }
            else
            {
                orderedQuery = isDescending
                    ? orderedQuery!.ThenByDescending(orderBy)
                    : orderedQuery!.ThenBy(orderBy);
            }
        }

        return orderedQuery!;
    }

    private IQueryable<TEntity> PrepareQuery(
        bool withDeleted = false,
        Expression<Func<TEntity, bool>>? filter = null,
        List<(Expression<Func<TEntity, object>> OrderBy, bool IsDescending)>? orderByList = null,
        bool asNoTracking = true,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _dbSet.AsQueryable();

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        query = ApplyIncludes(query, includes);
        query = ApplyBaseFilters(query, withDeleted, filter);
        query = ApplyOrdering(query, orderByList);

        return query;
    }

    #endregion

    #region Cursor Pagination Helpers

    private class CursorValue
    {
        public required object Value { get; set; }
        public required Type PropertyType { get; set; }
    }

    private string EncodeCursor(object value, Type propertyType)
    {
        var cursorValue = new CursorValue
        {
            Value = value,
            PropertyType = propertyType
        };

        var json = JsonSerializer.Serialize(cursorValue);
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));
    }

    private CursorValue DecodeCursor(string cursor)
    {
        var bytes = Convert.FromBase64String(cursor);
        var json = System.Text.Encoding.UTF8.GetString(bytes);
        var result = JsonSerializer.Deserialize<CursorValue>(json);

        if (result == null)
        {
            throw new InvalidOperationException("Invalid cursor format");
        }

        return result;
    }
    private string GetPropertyName<T>(Expression<Func<TEntity, T>> expression)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        throw new ArgumentException("Expression must be a member expression", nameof(expression));
    }
    private object GetPropertyValue<T>(TEntity entity, Expression<Func<TEntity, T>> expression)
    {
        var propertyName = GetPropertyName(expression);
        var property = typeof(TEntity).GetProperty(propertyName);
        return property?.GetValue(entity) ??
            throw new InvalidOperationException($"Property {propertyName} not found");
    }


    private Expression<Func<TEntity, bool>> CreateCursorExpression<T>(
        Expression<Func<TEntity, T>> cursorProperty,
        object cursorValue,
        bool forward)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var property = Expression.Property(parameter, GetPropertyName(cursorProperty));
        var constant = Expression.Constant(cursorValue, typeof(T));

        var comparison = forward
            ? Expression.GreaterThan(property, constant)
            : Expression.LessThan(property, constant);

        return Expression.Lambda<Func<TEntity, bool>>(comparison, parameter);
    }

    #endregion

    #region Query Methods

    public async Task<List<TEntity>> GetAllAsync(
        bool withDeleted = false,
        List<(Expression<Func<TEntity, object>> OrderBy, bool IsDescending)>? orderByList = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = PrepareQuery(withDeleted, null, orderByList, true, includes);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(
        Guid id,
        bool withDeleted = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = ApplyIncludes(_dbSet.AsQueryable(), includes).AsNoTracking();
        return await query.FirstOrDefaultAsync(
            x => x.Id.Equals(id) && (withDeleted || !x.IsDeleted),
            cancellationToken);
    }

    public async Task<List<TEntity>> WhereAsync(
        Expression<Func<TEntity, bool>> filter,
        bool withDeleted = false,
        List<(Expression<Func<TEntity, object>> OrderBy, bool IsDescending)>? orderByList = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = PrepareQuery(withDeleted, filter, orderByList, true, includes);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> expression,
        bool withDeleted = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = ApplyIncludes(_dbSet.AsQueryable(), includes).AsNoTracking();
        query = ApplyBaseFilters(query, withDeleted, expression);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    #endregion

    #region Command Methods

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedDate = _timeService.GetCurrentTime();
        entity.CreatedBy = _claimsService.CurrentUser;
        var result = await _dbSet.AddAsync(entity, cancellationToken);
        return result.Entity;
    }

    public async Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var currentTime = _timeService.GetCurrentTime();
        var currentUser = _claimsService.CurrentUser;

        foreach (var entity in entities)
        {
            entity.CreatedDate = currentTime;
            entity.CreatedBy = currentUser;
        }

        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        entity.UpdatedDate = _timeService.GetCurrentTime();
        entity.UpdatedBy = _claimsService.CurrentUser;
        _dbSet.Update(entity);
    }

    public void UpdateRange(List<TEntity> entities)
    {
        var currentTime = _timeService.GetCurrentTime();
        var currentUser = _claimsService.CurrentUser;

        foreach (var entity in entities)
        {
            entity.UpdatedDate = currentTime;
            entity.UpdatedBy = currentUser;
        }

        _dbSet.UpdateRange(entities);
    }

    public void SoftRemove(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.UpdatedDate = _timeService.GetCurrentTime();
        entity.UpdatedBy = _claimsService.CurrentUser;
        _dbSet.Update(entity);
    }

    public void SoftRemoveRange(List<TEntity> entities)
    {
        var currentTime = _timeService.GetCurrentTime();
        var currentUser = _claimsService.CurrentUser;

        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedDate = currentTime;
            entity.UpdatedBy = currentUser;
        }

        _dbSet.UpdateRange(entities);
    }

    #endregion

    #region Pagination Methods

    public async Task<(Pagination Pagination, List<TEntity> Entities)> ToPagination(
        int pageIndex = 0,
        int pageSize = 10,
        bool withDeleted = false,
        Expression<Func<TEntity, bool>>? filter = null,
        List<(Expression<Func<TEntity, object>> OrderBy, bool IsDescending)>? orderByList = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        pageIndex = Math.Max(0, pageIndex);
        pageSize = Math.Max(1, pageSize);

        var countQuery = ApplyBaseFilters(_dbSet.AsQueryable(), withDeleted, filter);

        var dataQuery = PrepareQuery(withDeleted, filter, orderByList, true, includes);

        var countTask = await countQuery.CountAsync(cancellationToken);
        var itemsTask = await dataQuery
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        //await Task.WhenAll(countTask, itemsTask);
        var totalCount = countTask;
        var items = itemsTask;

        var pagination = new Pagination
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalItemsCount = totalCount,
        };

        return (pagination, items);
    }


    public async Task<(CursorPagination, List<TEntity>)> ToPaginationWithCursor<TCursor>(
        Expression<Func<TEntity, TCursor>> cursorProperty,
        string? cursor = null,
        int pageSize = 10,
        int pageIndex = 0,
        bool forward = true,
        bool withDeleted = false,
        Expression<Func<TEntity, bool>>? filter = null,
        List<(Expression<Func<TEntity, object>> OrderBy, bool IsDescending)>? orderByList = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        pageSize = Math.Max(1, pageSize);

        var query = ApplyIncludes(_dbSet.AsQueryable(), includes).AsNoTracking();
        query = ApplyBaseFilters(query, withDeleted, filter);

        var countQuery = ApplyBaseFilters(_dbSet.AsQueryable(), withDeleted, filter);
        var totalItemsCount = await countQuery.CountAsync(cancellationToken);

        bool isDescending = !forward;
        IQueryable<TEntity> orderedQuery;

        if (orderByList != null && orderByList.Count > 0)
        {
            var isFirstOrder = true;
            IOrderedQueryable<TEntity>? tempOrderedQuery = null;

            foreach (var (orderBy, orderIsDescending) in orderByList)
            {
                if (isFirstOrder)
                {
                    tempOrderedQuery = orderIsDescending
                        ? query.OrderByDescending(orderBy)
                        : query.OrderBy(orderBy);
                    isFirstOrder = false;
                }
                else
                {
                    tempOrderedQuery = orderIsDescending
                        ? tempOrderedQuery!.ThenByDescending(orderBy)
                        : tempOrderedQuery!.ThenBy(orderBy);
                }
            }

            orderedQuery = isDescending
                ? tempOrderedQuery!.ThenByDescending(cursorProperty)
                : tempOrderedQuery!.ThenBy(cursorProperty);
        }
        else
        {
            orderedQuery = isDescending
                ? query.OrderByDescending(cursorProperty)
                : query.OrderBy(cursorProperty);
        }

        if (!string.IsNullOrEmpty(cursor))
        {
            try
            {
                var cursorData = DecodeCursor(cursor);
                var cursorValue = Convert.ChangeType(cursorData.Value, cursorData.PropertyType);

                var cursorFilter = CreateCursorExpression(cursorProperty, cursorValue, forward);
                orderedQuery = orderedQuery.Where(cursorFilter);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Invalid cursor format: {ex.Message}", nameof(cursor));
            }
        }

        var items = await orderedQuery
            .Take(pageSize + 1)
            .ToListAsync(cancellationToken);

        bool hasMore = items.Count > pageSize;
        if (hasMore)
        {
            items.RemoveAt(pageSize);
        }

        if (!forward)
        {
            items.Reverse();
        }

        string? nextCursor = null;
        string? previousCursor = null;

        if (hasMore && items.Count != 0)
        {
            var lastItem = items.Last();
            var lastValue = GetPropertyValue(lastItem, cursorProperty);
            var propertyType = typeof(TCursor);
            nextCursor = EncodeCursor(lastValue, propertyType);
        }

        if (items.Any())
        {
            var firstItem = items.First();
            var firstValue = GetPropertyValue(firstItem, cursorProperty);
            var propertyType = typeof(TCursor);
            previousCursor = EncodeCursor(firstValue, propertyType);
        }

        var cursorPagination = new CursorPagination
        {
            PageSize = pageSize,
            PageIndex = pageIndex,
            TotalItemsCount = totalItemsCount,
            NextCursor = hasMore ? nextCursor : null,
            PreviousCursor = !string.IsNullOrEmpty(cursor) ? previousCursor : null
        };

        return (cursorPagination, items);
    }

    #endregion
}
