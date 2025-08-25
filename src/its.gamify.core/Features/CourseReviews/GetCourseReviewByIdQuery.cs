using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.domains.Entities;
using MediatR;


namespace its.gamify.core.Features.CourseReviews;

public class GetCourseReviewByIdQuery : IRequest<CourseReview>
{
    public Guid Id { get; set; }
    public class QueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCourseReviewByIdQuery, CourseReview>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<CourseReview> Handle(GetCourseReviewByIdQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.CourseReviewRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken) ?? throw new BadRequestException("Không tìm thấy review!");
        }
    }
}
