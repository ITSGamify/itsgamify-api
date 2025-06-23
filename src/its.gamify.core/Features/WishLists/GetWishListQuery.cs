using its.gamify.core.Models.ShareModels;
using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.core.Features.WishLists.Queries
{
    public class GetWishListQuery : IRequest<BasePagingResponseModel<WishList>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetWishListQuery, BasePagingResponseModel<WishList>>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<BasePagingResponseModel<WishList>> Handle(GetWishListQuery request, CancellationToken cancellationToken)
            {
                var items = await unitOfWork.WishListRepository.ToPagination(
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    includes: x => x.Course, x => x.User,
                    cancellationToken: cancellationToken);
                return new BasePagingResponseModel<WishList>(items.Entities, items.Pagination);
            }
        }
    }
}
