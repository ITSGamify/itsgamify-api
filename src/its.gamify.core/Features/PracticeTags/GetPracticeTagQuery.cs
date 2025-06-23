using its.gamify.core.Models.ShareModels;
using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.core.Features.PracticeTags.Queries
{
    public class GetPracticeTagQuery : IRequest<BasePagingResponseModel<PracticeTag>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetPracticeTagQuery, BasePagingResponseModel<PracticeTag>>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<BasePagingResponseModel<PracticeTag>> Handle(GetPracticeTagQuery request, CancellationToken cancellationToken)
            {
                var items = await unitOfWork.PracticeTagRepository.ToPagination(
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    includes: x => x.Practice,
                    cancellationToken: cancellationToken);
                return new BasePagingResponseModel<PracticeTag>(items.Entities, items.Pagination);
            }
        }
    }
}
