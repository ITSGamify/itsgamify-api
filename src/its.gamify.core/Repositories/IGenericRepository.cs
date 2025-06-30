using its.gamify.core.Models;
using its.gamify.domains.Entities;
using its.gamify.domains.Models;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace its.gamify.core.Repositories;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    #region Query Methods

    Task<List<TEntity>> GetAllAsync(
        bool withDeleted = false,
        List<(Expression<Func<TEntity, object>> OrderBy, bool IsDescending)>? orderByList = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity?> GetByIdAsync(
        Guid id,
        bool withDeleted = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes);



    Task<List<TEntity>> WhereAsync(
        Expression<Func<TEntity, bool>> filter,
        bool withDeleted = false,
        List<(Expression<Func<TEntity, object>> OrderBy, bool IsDescending)>? orderByList = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> expression,
        bool withDeleted = false,
        CancellationToken cancellationToken = default,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeFunc = null);
    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> expression,
        bool withDeleted = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes);

    #endregion

    #region Command Methods

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);


    Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default);


    void Update(TEntity entity);


    void UpdateRange(List<TEntity> entities);


    void SoftRemove(TEntity entity);


    void SoftRemoveRange(List<TEntity> entities);

    #endregion

    #region Pagination Methods
    public Task<(Pagination Pagination, List<TEntity> Entities)> ToDynamicPagination(
        int pageIndex = 0,
        int pageSize = 10,
        bool withDeleted = false,
        string? searchTerm = null,
        List<string>? searchFields = null,
        Dictionary<string, bool>? sortOrders = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes);
    //public Task<(Pagination Pagination, List<TEntity> Entities)> ToDynamicPagination(
    //     FilterQuery? query,
    //     bool withDeleted = false,
    //     CancellationToken cancellationToken = default,
    //     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeFunc = null);
    public Task<(Pagination Pagination, List<TEntity> Entities)> ToDynamicPagination(
         int pageIndex = 0,
         int pageSize = 10,
         bool withDeleted = false,
         string? searchTerm = null,
         List<string>? searchFields = null,
         Dictionary<string, bool>? sortOrders = null,
         CancellationToken cancellationToken = default,
         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeFunc = null);
    Task<(Pagination Pagination, List<TEntity> Entities)> ToPagination(
        int pageIndex = 0,
        int pageSize = 10,
        bool withDeleted = false,
        Expression<Func<TEntity, bool>>? filter = null,
        List<(Expression<Func<TEntity, object>> OrderBy, bool IsDescending)>? orderByList = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes);

    Task<(CursorPagination, List<TEntity>)> ToPaginationWithCursor<TCursor>(
        Expression<Func<TEntity, TCursor>> cursorProperty,
        string? cursor = null,
        int pageSize = 10,
        int pageIndex = 0,
        bool forward = true,
        bool withDeleted = false,
        Expression<Func<TEntity, bool>>? filter = null,
        List<(Expression<Func<TEntity, object>> OrderBy, bool IsDescending)>? orderByList = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes);


    Task<(Pagination Pagination, List<TEntity> Entities)> ToPaginationV2(
        int pageIndex = 0,
        int pageSize = 10,
        bool withDeleted = false,
        Expression<Func<TEntity, bool>>? filter = null,
         List<OrderByItem>? orderBy = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes);

    #endregion

    #region Generic Function
    Task<TEntity> EnsureExistsIfIdNotEmpty(Guid id);
    #endregion
}
