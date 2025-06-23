using its.gamify.core.Models.ShareModels;
using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.core.Features.Practices.Queries
{
    public class GetPracticeQuery : IRequest<BasePagingResponseModel<Practice>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        class QueryHandler : IRequestHandler<GetPracticeQuery, BasePagingResponseModel<Practice>>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<BasePagingResponseModel<Practice>> Handle(GetPracticeQuery request,
                CancellationToken cancellationToken)
            {
                var practices = await unitOfWork.PracticeRepository.ToPagination(pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    includes: x => x.PracticeTags, cancellationToken: cancellationToken);
                return new BasePagingResponseModel<Practice>(practices.Entities, practices.Pagination);
            }
        }
    }
}
