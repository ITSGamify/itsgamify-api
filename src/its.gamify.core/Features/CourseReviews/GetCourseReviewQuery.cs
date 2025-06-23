using its.gamify.core.Models.ShareModels;
using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.core.Features.CourseReviews.Queries
{
    public class GetCourseReviewQuery : IRequest<BasePagingResponseModel<CourseReview>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetCourseReviewQuery, BasePagingResponseModel<CourseReview>>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<BasePagingResponseModel<CourseReview>> Handle(GetCourseReviewQuery request, CancellationToken cancellationToken)
            {
                var items = await unitOfWork.CourseReviewRepository.ToPagination(
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    includes: x => x.Course, x => x.CourseParticipation,
                    cancellationToken: cancellationToken);
                return new BasePagingResponseModel<CourseReview>(items.Entities, items.Pagination);
            }
        }
    }
}
