using its.gamify.core.Models.ShareModels;
using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.core.Features.LearningProgresses.Queries
{
    public class GetLearningProgressQuery : IRequest<BasePagingResponseModel<LearningProgress>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetLearningProgressQuery, BasePagingResponseModel<LearningProgress>>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<BasePagingResponseModel<LearningProgress>> Handle(GetLearningProgressQuery request, CancellationToken cancellationToken)
            {
                var items = await unitOfWork.LearningProgressRepository.ToPagination(
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    includes: x => x.CourseParticipation, x => x.QuizResult,
                    cancellationToken: cancellationToken);
                return new BasePagingResponseModel<LearningProgress>(items.Entities, items.Pagination);
            }
        }
    }
}
