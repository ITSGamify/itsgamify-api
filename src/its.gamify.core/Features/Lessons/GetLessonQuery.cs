using its.gamify.core.Models.ShareModels;
using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.core.Features.Lessons.Queries
{
    public class GetLessonQuery : IRequest<BasePagingResponseModel<Lesson>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetLessonQuery, BasePagingResponseModel<Lesson>>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<BasePagingResponseModel<Lesson>> Handle(GetLessonQuery request, CancellationToken cancellationToken)
            {
                var lessons = await unitOfWork.LessonRepository.ToPagination(
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    includes: x => x.LearningProgress, x => x.CourseSection,
                    cancellationToken: cancellationToken);
                return new BasePagingResponseModel<Lesson>(lessons.Entities, lessons.Pagination);
            }
        }
    }
}
