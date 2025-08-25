using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using MediatR;

namespace its.gamify.core.Features.CourseReviews.Commands;

public class CourseReviewCreateModel
{
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public Guid CourseId { get; set; }
    public Guid CourseParticipationId { get; set; }

}

public class CreateReviewCommand : CourseReviewCreateModel, IRequest<CourseReview>
{

    class CommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateReviewCommand, CourseReview>
    {

        public async Task<CourseReview> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var participation = await unitOfWork.CourseParticipationRepository.GetByIdAsync(request.CourseParticipationId, cancellationToken: cancellationToken)
                ?? throw new Exception("Course participation not found.");

            if (participation.CourseId != request.CourseId)
                throw new Exception("Course participation does not match the course ID.");

            if (participation.Status != COURSE_PARTICIPATION_STATUS.COMPLETED)
                throw new BadRequestException("Bạn chưa hoàn thành khóa học này.");

            var review = unitOfWork.Mapper.Map<CourseReview>(request);

            await unitOfWork.CourseReviewRepository.AddAsync(review, cancellationToken);

            await unitOfWork.SaveChangesAsync();
            return review;
        }
    }
}
