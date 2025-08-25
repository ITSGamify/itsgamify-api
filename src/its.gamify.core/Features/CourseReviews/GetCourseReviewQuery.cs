using its.gamify.core.Models.ShareModels;
using its.gamify.domains.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace its.gamify.core.Features.CourseReviews;

public class GetCourseReviewQuery : IRequest<BasePagingResponseModel<CourseReview>>
{
    public FilterQuery Filter { get; set; } = new();
    public Guid CourseId { get; set; }
    public class QueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCourseReviewQuery, BasePagingResponseModel<CourseReview>>
    {

        public async Task<BasePagingResponseModel<CourseReview>> Handle(GetCourseReviewQuery request, CancellationToken cancellationToken)
        {
            var (Pagination, Entities) = await unitOfWork.CourseReviewRepository.ToDynamicPagination(
                pageIndex: request.Filter.Page ?? 0,
                pageSize: request.Filter.Limit ?? 10,
                filter: x => x.CourseId == request.CourseId,
                includeFunc: x => x.Include(x => x.CourseParticipation).ThenInclude(x => x.User),
                cancellationToken: cancellationToken);

            return new BasePagingResponseModel<CourseReview>(Entities, Pagination);
        }
    }
}
