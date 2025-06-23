using its.gamify.core.Models.ShareModels;
using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.core.Features.ChallengeParticipations.Queries
{
    public class GetChallengeParticipationQuery : IRequest<BasePagingResponseModel<ChallengeParticipation>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetChallengeParticipationQuery, BasePagingResponseModel<ChallengeParticipation>>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<BasePagingResponseModel<ChallengeParticipation>> Handle(GetChallengeParticipationQuery request, CancellationToken cancellationToken)
            {
                var items = await unitOfWork.ChallengeParticipationRepository.ToPagination(
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    includes: x => x.Challenge, x => x.Employee,
                    cancellationToken: cancellationToken);
                return new BasePagingResponseModel<ChallengeParticipation>(items.Entities, items.Pagination);
            }
        }
    }
}
