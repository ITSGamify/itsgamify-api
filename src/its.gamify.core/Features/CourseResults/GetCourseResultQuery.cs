using its.gamify.core.Models.ShareModels;
using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.core.Features.CourseResults.Queries
{
    public class GetCourseResultQuery : IRequest<BasePagingResponseModel<CourseResult>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetCourseResultQuery, BasePagingResponseModel<CourseResult>>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<BasePagingResponseModel<CourseResult>> Handle(GetCourseResultQuery request, CancellationToken cancellationToken)
            {
                var items = await unitOfWork.CourseResultRepository.ToPagination(
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    includes: x => x.User, x => x.Course, x => x.CourseParticipation,
                    cancellationToken: cancellationToken);
                return new BasePagingResponseModel<CourseResult>(items.Entities, items.Pagination);
            }
        }
    }
}
