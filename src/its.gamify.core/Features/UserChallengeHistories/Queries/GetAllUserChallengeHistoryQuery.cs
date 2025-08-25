using its.gamify.core.Models.ShareModels;
using its.gamify.core.Utilities;
using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace its.gamify.core.Features.UserChallengeHistories.Queries
{
    public class GetAllUserChallengeHistoryQuery : IRequest<BasePagingResponseModel<UserChallengeHistory>>
    {
        public required Guid UserId { get; set; }
        public FilterQueryExtend? Filter { get; set; }
        class QueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllUserChallengeHistoryQuery, BasePagingResponseModel<UserChallengeHistory>>
        {

            public async Task<BasePagingResponseModel<UserChallengeHistory>> Handle(GetAllUserChallengeHistoryQuery request, CancellationToken cancellationToken)
            {
                Expression<Func<UserChallengeHistory, bool>> filter = x => x.UserId == request.UserId;

                // Thêm điều kiện tìm kiếm nếu có Q
                if (request.Filter != null && !string.IsNullOrEmpty(request.Filter.Q))
                {
                    Expression<Func<UserChallengeHistory, bool>> searchFilter = x =>
                        x.Status.Contains(request.Filter.Q) ||
                        x.Challenge.Title.Contains(request.Filter.Q);

                    filter = FilterCustom.CombineFilters(filter, searchFilter);
                }

                Expression<Func<UserChallengeHistory, bool>> filterStatus = null!;

                if (request.Filter!.FilterString == UserChallengeHistoryEnum.WIN)
                {
                    filterStatus = x => x.Status == UserChallengeHistoryEnum.WIN;
                    filter = filter != null ? FilterCustom.CombineFilters(filter, filterStatus) : filterStatus;
                }
                else if (request.Filter.FilterString == UserChallengeHistoryEnum.LOSE)
                {
                    filterStatus = x => x.Status == UserChallengeHistoryEnum.LOSE;
                    filter = filter != null ? FilterCustom.CombineFilters(filter, filterStatus) : filterStatus;
                }

                var (Pagination, Entities) = await unitOfWork.UserChallengeHistoryRepository
                .ToDynamicPagination(pageIndex: request.Filter?.Page ?? 0,
                    pageSize: request.Filter?.Limit ?? 10,
                    filter: filter,
                    searchFields: ["ChallengeId", "UserId", "Status"], searchTerm: request.Filter?.Q ?? string.Empty,
                    sortOrders: request.Filter?.OrderBy?.ToDictionary(x => x.OrderColumn ?? string.Empty, x => x.OrderDir == "ASC"),
                    includeFunc: x => x.Include(x => x.Challenge).Include(x => x.User).Include(x => x.Winner));
                return BasePagingResponseModel<UserChallengeHistory>.CreateInstance(Entities, Pagination);


            }
        }

    }
}
