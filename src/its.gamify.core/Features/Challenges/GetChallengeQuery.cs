using its.gamify.core.Models.ShareModels;
using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.core.Features.Challenges.Queries
{
    public class GetChallengeQuery : IRequest<BasePagingResponseModel<Challenge>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetChallengeQuery, BasePagingResponseModel<Challenge>>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<BasePagingResponseModel<Challenge>> Handle(GetChallengeQuery request, CancellationToken cancellationToken)
            {
                var items = await unitOfWork.ChallengeRepository.ToPagination(
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    cancellationToken: cancellationToken);
                return new BasePagingResponseModel<Challenge>(items.Entities, items.Pagination);
            }
        }
    }
}
