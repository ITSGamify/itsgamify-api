using its.gamify.core;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using MediatR;

namespace its.gamify.api.Features.Courses.Commands
{
    public class DeleteCourseCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        class CommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCourseCommand, bool>
        {

            public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
            {
                var course = await unitOfWork.CourseRepository.GetByIdAsync(request.Id, includes: [x => x.Challenges.Where(c => !c.IsDeleted), x => x.CourseReviews.Where(cr => !cr.IsDeleted)])
                    ?? throw new BadRequestException("Khóa học không tồn tại!");

                if (course.Challenges.Any(c => !c.IsDeleted))
                {
                    throw new BadRequestException("Khóa học không thể xóa vì có các thử thách đang sử dụng!");
                }

                unitOfWork.CourseRepository.SoftRemove(course!);
                return await unitOfWork.SaveChangesAsync();


            }
        }

    }
}
