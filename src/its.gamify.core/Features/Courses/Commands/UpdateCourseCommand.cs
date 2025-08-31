using FluentValidation;
using its.gamify.core;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.core.Models.Courses;
using its.gamify.core.Models.CourseSections;
using its.gamify.core.Models.Lessons;
using its.gamify.core.Models.Questions;
using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using MediatR;

namespace its.gamify.api.Features.Courses.Commands
{
    public class UpdateCourseCommand : IRequest<bool>
    {

        public CourseUpdateModel Model { get; set; } = new();
        class CommandValidate : AbstractValidator<UpdateCourseCommand>
        {
            public CommandValidate()
            {
                When(x => x.Model.Status == CourseStatusEnum.INITIAL.ToString(), () =>
                {
                    RuleFor(x => x.Model.Title).NotEmpty().WithMessage("Vui lòng nhập tiêu đề");
                    RuleFor(x => x.Model.ThumbnailId).NotEmpty().WithMessage("Vui lòng nhập ảnh thumbnail");
                    RuleFor(x => x.Model.IntroVideoId).NotEmpty().WithMessage("Vui lòng nhập intro video");
                    RuleFor(x => x.Model.CategoryId).NotEmpty().WithMessage("Vui lòng chọn danh mục");
                    RuleFor(x => x.Model.Description).NotEmpty().WithMessage("Vui lòng nhập mô tả ngắn");
                    RuleFor(x => x.Model.LongDescription).NotEmpty().WithMessage("Vui lòng nhập mô tả");
                });
                When(x => x.Model.Status == CourseStatusEnum.CONTENT.ToString(), () =>
                {
                    RuleForEach(x => x.Model.CourseSections).NotNull().WithMessage("Module đang trống")
                        .SetValidator(new CourseSectionValidator());
                });

            }
            class CourseSectionValidator : AbstractValidator<CourseSectionUpdateModel>
            {
                public CourseSectionValidator()
                {

                    RuleFor(x => x.Description).NotEmpty().WithMessage("Mô tả của module đang trống");
                    RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề của module đang trống");
                    RuleFor(x => x.OrderedNumber).NotEmpty().WithMessage("Số thứ tự của module đang trống");
                    RuleForEach(x => x.Lessons).NotEmpty().SetValidator(new LessonValidator());
                }
            }
            class LessonValidator : AbstractValidator<LessonUpdateModel>
            {
                public LessonValidator()
                {
                    When(x => x.Type == LearningMaterialType.VIDEO.ToString(), () =>
                    {
                        RuleFor(x => x.Url).NotEmpty().WithMessage("Video Url đang trống");
                    });
                    When(x => x.Type == LearningMaterialType.DOCUMENT.ToString(), () =>
                    {
                        RuleFor(x => x.Content).NotEmpty().WithMessage("Nội dung bài học đang trống");
                    });
                    When(x => x.Type == LearningMaterialType.QUIZ.ToString(), () =>
                    {
                        RuleForEach(x => x.QuestionModels).SetValidator(new QuestionValidator());
                    });

                }
            }
            class QuestionValidator : AbstractValidator<QuestionUpsertModel>
            {
                public QuestionValidator()
                {
                    RuleFor(x => x.Content).NotEmpty().WithMessage("Nội dung câu hỏi đang trống");
                }
            }

        }

        class CommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateCourseCommand, bool>
        {
            public async Task<bool> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
            {
                var course = await unitOfWork.CourseRepository.FirstOrDefaultAsync(x => x.Id == request.Model.Id)
                    ?? throw new InvalidOperationException("Không tìm thấy course với Id " + request.Model.Id);
                unitOfWork.Mapper.Map(request.Model, course);

                course.ThumbnailImage = (await unitOfWork.FileRepository.FirstOrDefaultAsync(x => x.Id == request.Model.ThumbnailId)
                    ?? throw new InvalidOperationException("Không tìm thấy image thumbnail")).Url;
                course.IntroVideo = (await unitOfWork.FileRepository.FirstOrDefaultAsync(x => x.Id == request.Model.IntroVideoId)
                    ?? throw new InvalidOperationException("Không tìm thấy Intro Video với Id ")).Url;
                bool checkDupName = (await unitOfWork.CourseRepository.WhereAsync(x => x.Title.ToLower().Trim() == request.Model.Title.ToLower().Trim() && x.Id != request.Model.Id)) != null;
                if (checkDupName) throw new BadRequestException("Tên khóa học đã tồn tại!");
                if (request.Model.IsUpdateDepartment)
                {
                    await UpdateCourseDepartments(request.Model, cancellationToken);
                }

                unitOfWork.CourseRepository.Update(course);
                return await unitOfWork.SaveChangesAsync();

            }


            private async Task UpdateCourseDepartments(CourseUpdateModel model, CancellationToken cancellationToken)
            {
                var course_departments = await unitOfWork.CourseDepartmentRepository.WhereAsync(x => x.CourseId == model.Id);

                if (model.CourseType == CourseTypeEnum.DEPARTMENTONLY.ToString())
                {
                    var existingIds = course_departments.Select(x => x.DepartmentId).ToList();
                    var newDepartmentIds = model.DepartmentIds;

                    var departmentsToRemove = course_departments.Where(cd => !newDepartmentIds.Contains(cd.DepartmentId)).ToList();
                    if (departmentsToRemove.Count != 0)
                    {
                        unitOfWork.CourseDepartmentRepository.SoftRemoveRange(departmentsToRemove);
                    }

                    var departmentsToAdd = newDepartmentIds
                        .Where(id => !existingIds.Contains(id))
                        .Select(id => new CourseDepartment
                        {
                            CourseId = model.Id!.Value,
                            DepartmentId = id
                        })
                        .ToList();

                    if (departmentsToAdd.Count != 0)
                    {
                        await unitOfWork.CourseDepartmentRepository.AddRangeAsync(departmentsToAdd, cancellationToken);
                    }

                }

                if (model.CourseType != CourseTypeEnum.DEPARTMENTONLY.ToString())
                {
                    unitOfWork.CourseDepartmentRepository.SoftRemoveRange(course_departments);
                }

            }
        }
    }
}
