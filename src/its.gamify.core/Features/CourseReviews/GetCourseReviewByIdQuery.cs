using its.gamify.domains.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace its.gamify.core.Features.CourseReviews.Queries
{
    public class GetCourseReviewByIdQuery : IRequest<CourseReview>
    {
        public Guid Id { get; set; }
        public class QueryHandler : IRequestHandler<GetCourseReviewByIdQuery, CourseReview>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<CourseReview> Handle(GetCourseReviewByIdQuery request, CancellationToken cancellationToken)
            {
                return await unitOfWork.CourseReviewRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            }
        }
    }
}
