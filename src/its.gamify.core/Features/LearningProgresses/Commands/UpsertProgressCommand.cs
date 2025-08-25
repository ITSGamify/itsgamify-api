using Hangfire;
using its.gamify.core;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.core.Models.Lessons;
using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using MediatR;

namespace its.gamify.api.Features.LearningProgresses.Commands
{
    public class UpsertProgressCommand : LearningProgessUpsertModel, IRequest<LearningProgress>
    {
        public required LearningProgessUpsertModel Model { get; set; }

        class CommandHandler(IUnitOfWork _unitOfWork) : IRequestHandler<UpsertProgressCommand, LearningProgress>
        {
            public async Task<LearningProgress> Handle(UpsertProgressCommand request, CancellationToken cancellationToken)
            {
                Lesson lesson = await _unitOfWork.LessonRepository.GetByIdAsync(request.Model.LessonId)
                                ?? throw new Exception("Can not find lesson");

                var progress = await _unitOfWork
                    .LearningProgressRepository
                    .FirstOrDefaultAsync(x =>
                        x.LessonId == lesson.Id &&
                        x.CourseParticipationId == request.Model.CourseParticipationId
                    );

                if (progress != null)
                {
                    progress.LastAccessed = DateTime.UtcNow;
                    progress.Status = request.Model.Status;
                    progress.VideoTimePosition = (int)request.Model.VideoTimePosition;

                    _unitOfWork.LearningProgressRepository.Update(progress);
                }
                else
                {
                    progress = new LearningProgress
                    {
                        LessonId = lesson.Id,
                        LastAccessed = DateTime.UtcNow,
                        Status = request.Model.Status,
                        CourseParticipationId = request.Model.CourseParticipationId,
                        VideoTimePosition = (int)request.Model.VideoTimePosition
                    };
                    await _unitOfWork.LearningProgressRepository.AddAsync(progress, cancellationToken);
                }

                await _unitOfWork.SaveChangesAsync();
                return progress;

            }

        }
    }
}