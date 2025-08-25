using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using MediatR;

namespace its.gamify.core.Features.CourseReviews.Commands;


public class DeleteReviewComamnd : IRequest<bool>
{
    public Guid Id { get; set; }
    class CommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteReviewComamnd, bool>
    {
        public async Task<bool> Handle(DeleteReviewComamnd request, CancellationToken cancellationToken)
        {
            var review = await unitOfWork.CourseReviewRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken)
                 ?? throw new BadRequestException("Không tìm thấy đánh giá của bạn!");

            unitOfWork.CourseReviewRepository.SoftRemove(review);

            return await unitOfWork.SaveChangesAsync();
        }
    }
}
