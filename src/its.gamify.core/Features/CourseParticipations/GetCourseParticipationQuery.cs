using its.gamify.core.Models.ShareModels;
using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.core.Features.CourseParticipations.Queries
{
    public class GetCourseParticipationQuery : IRequest<BasePagingResponseModel<CourseParticipation>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetCourseParticipationQuery, BasePagingResponseModel<CourseParticipation>>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<BasePagingResponseModel<CourseParticipation>> Handle(GetCourseParticipationQuery request, CancellationToken cancellationToken)
            {
                var items = await unitOfWork.CourseParticipationRepository.ToPagination(
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    includes: x => x.User, x => x.Course, x => x.CourseResult, x => x.CourseReview,
                    cancellationToken: cancellationToken);
                return new BasePagingResponseModel<CourseParticipation>(items.Entities, items.Pagination);
            }
        }
    }
}
