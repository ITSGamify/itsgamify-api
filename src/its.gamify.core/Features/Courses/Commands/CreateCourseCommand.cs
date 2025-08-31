using FluentValidation;
using its.gamify.api.Features.CourseSections.Commands;
using its.gamify.core;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.core.IntegrationServices.Interfaces;
using its.gamify.core.Models.Courses;
using its.gamify.core.Utilities;
using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using MediatR;

namespace its.gamify.api.Features.Courses.Commands
{
    public class CreateCourseCommand : CourseCreateModels, IRequest<Course>
    {

        class CommandValidation : AbstractValidator<CreateCourseCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CategoryId).NotNull().NotEmpty().WithMessage("Vui lòng nhập category id");
            }
        }
        class CommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCourseCommand, Course>
        {

            public async Task<Course> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
            {
                var course = unitOfWork.Mapper.Map<Course>(request);
                course.Status = CourseStatusEnum.INITIAL.ToString();
                course.ThumbnailImage = (await unitOfWork.FileRepository.FirstOrDefaultAsync(x => x.Id == request.ThumbnailId)
                    ?? throw new InvalidOperationException("Không tìm thấy image thumbnail")).Url;
                course.IntroVideo = (await unitOfWork.FileRepository.FirstOrDefaultAsync(x => x.Id == request.IntroVideoId)
                    ?? throw new InvalidOperationException("Không tìm thấy Intro Video với Id " + request.IntroVideoId)).Url;
                course.ThumbnailId = request.ThumbnailId;
                course.IntroVideoId = request.IntroVideoId;
                bool checkDupName = (await unitOfWork.CourseRepository.WhereAsync(x => x.Title.ToLower().Trim() == request.Title.ToLower().Trim())) != null;
                if (checkDupName) throw new BadRequestException("Tên khóa học đã tồn tại!");
                CourseMetric cm = new()
                {
                    CourseId = course.Id,
                    SaveCount = 0,
                };

                await unitOfWork.CourseMetricRepository.AddAsync(cm, cancellationToken);

                await unitOfWork.CourseRepository.AddAsync(course, cancellationToken);

                if (request.DepartmentIds.Count > 0 && request.CourseType == CourseTypeEnum.DEPARTMENTONLY.ToString())
                {
                    var course_departments = new List<CourseDepartment>();
                    foreach (var item in request.DepartmentIds)
                    {
                        course_departments.Add(new CourseDepartment
                        {
                            CourseId = course.Id,
                            DepartmentId = item
                        });
                    }
                    await unitOfWork.CourseDepartmentRepository.AddRangeAsync(course_departments, cancellationToken);
                }

                await unitOfWork.SaveChangesAsync();
                return course;
            }
        }
    }
}
