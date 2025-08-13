using FluentValidation;
using its.gamify.api.Features.CourseSections.Commands;
using its.gamify.api.Features.LearningMaterials.Commands;
using its.gamify.core;
using its.gamify.core.Models.Courses;
using its.gamify.core.Models.CourseSections;
using its.gamify.core.Models.Lessons;
using its.gamify.core.Models.Questions;
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
                    RuleFor(x => x.Model.ThumbNailImageId).NotEmpty().WithMessage("Vui lòng nhập ảnh thumbnail");
                    RuleFor(x => x.Model.IntroductionVideoId).NotEmpty().WithMessage("Vui lòng nhập intro video");
                    RuleFor(x => x.Model.CategoryId).NotEmpty().WithMessage("Vui lòng chọn danh mục");
                    RuleFor(x => x.Model.Description).NotEmpty().WithMessage("Vui lòng nhập mô tả ngắn");
                    RuleFor(x => x.Model.LongDescription).NotEmpty().WithMessage("Vui lòng nhập mô tả");
                });
                When(x => x.Model.Status == CourseStatusEnum.CONTENT.ToString(), () =>
                {
                    RuleForEach(x => x.Model.CourseSectionCreate).NotNull().WithMessage("Module đang trống")
                        .SetValidator(new CourseSectionValidator());

                });
            }
            class CourseSectionValidator : AbstractValidator<CourseSectionCreateModel>
            {
                public CourseSectionValidator()
                {

                    RuleFor(x => x.Description).NotEmpty().WithMessage("Mô tả của module đang trống");
                    RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề của module đang trống");
                    RuleForEach(x => x.Lessons).NotEmpty().SetValidator(new LessonValidator());
                }
            }
            class LessonValidator : AbstractValidator<LessonCreateModel>
            {
                public LessonValidator()
                {
                    When(x => x.Type == LearningMaterialType.VIDEO.ToString(), () =>
                    {
                        RuleFor(x => x.VideoUrl).NotEmpty().WithMessage("Video Url đang trống");
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

        class CommandHandler : IRequestHandler<UpdateCourseCommand, bool>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IMediator mediator;
            public CommandHandler(IUnitOfWork unitOfWork,
                IMediator mediator)
            {
                this.mediator = mediator;
                this.unitOfWork = unitOfWork;
            }
            public async Task<bool> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
            {
                var course = await unitOfWork.CourseRepository.FirstOrDefaultAsync(x => x.Id == request.Model.Id)
                    ?? throw new InvalidOperationException("Không tìm thấy course với Id " + request.Model.Id);
                if (request.Model.CourseSectionCreate?.Count > 0
                    && course.Status == CourseStatusEnum.INITIAL.ToString())
                {
                    course.Status = CourseStatusEnum.CONTENT.ToString();
                }
                unitOfWork.Mapper.Map(request.Model, course);

                foreach (var courseSection in request.Model.CourseSectionCreate ?? [])
                {
                    await mediator.Send(new UpsertCourseSectionCommand()
                    {
                        CourseId = request.Model.Id,
                        CreateId = courseSection.CreateId,
                        Description = courseSection.Description,
                        Lessons = courseSection.Lessons,
                        Title = courseSection.Title,
                    });
                }
                if (request.Model.LearningMaterialIds?.Count > 0)
                {
                    var learningMate = await mediator.Send(new UpsertLearningMaterials()
                    {
                        CourseId = request.Model.Id,
                        FileIds = request.Model.LearningMaterialIds,
                    });
                    course.Status = CourseStatusEnum.MATERIAL.ToString();

                    unitOfWork.CourseRepository.Update(course);
                    await unitOfWork.SaveChangesAsync();
                }




                return true;


            }
        }
    }
}
