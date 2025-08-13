using its.gamify.api.Features.Lessons.Commands;
using its.gamify.core;
using its.gamify.core.IntegrationServices.Interfaces;
using its.gamify.core.Models.CourseSections;
using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.api.Features.CourseSections.Commands
{
    public class UpsertCourseSectionCommand : CourseSectionCreateModel, IRequest<CourseSection>
    {
        public Guid CourseId { get; set; }
        class CommandHandler : IRequestHandler<UpsertCourseSectionCommand, CourseSection>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IFirebaseService firebaseSerivce;
            private readonly IMediator mediator;
            public CommandHandler(IUnitOfWork unitOfWork,
                IFirebaseService firebaseService,
                IMediator mediator)
            {
                this.mediator = mediator;
                this.firebaseSerivce = firebaseService;
                this.unitOfWork = unitOfWork;
            }
            public async Task<CourseSection> Handle(UpsertCourseSectionCommand request, CancellationToken cancellationToken)
            {
                bool isUpdate = request.CreateId != null;
                CourseSection? current = null;
                if (isUpdate)
                {
                    current = await unitOfWork.CourseSectionRepository.GetByIdAsync(request.CreateId!.Value,
                        false,
                        cancellationToken,
                        [x => x.Lessons]);
                }
                var courseSection = isUpdate ? current : unitOfWork.Mapper.Map<CourseSection>(request);

                courseSection!.CourseId = request.CourseId;
                courseSection.Lessons = [];

                if (isUpdate)
                {
                    unitOfWork.CourseSectionRepository.Update(courseSection);
                }
                else
                    await unitOfWork.CourseSectionRepository.AddAsync(courseSection);

                await unitOfWork.SaveChangesAsync();
                var lessons = await mediator.Send(new UpsertLessonsCommand()
                {
                    Models = request.Lessons ?? [],
                    CourseSectionId = courseSection.Id
                });

                courseSection.Lessons = lessons;
                return courseSection;

            }
        }
    }
}
