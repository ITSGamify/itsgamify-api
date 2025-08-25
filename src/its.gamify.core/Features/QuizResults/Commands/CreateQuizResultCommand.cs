using FluentValidation;
using Hangfire;
using its.gamify.core;
using its.gamify.core.Features.Notifications.Commands;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.core.Models.QuizAnswers;
using its.gamify.core.Models.QuizResults;
using its.gamify.core.Services.Interfaces;
using its.gamify.core.Utilities;
using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using MediatR;
using System.Threading.Tasks;

namespace its.gamify.api.Features.QuizResults.Commands
{
    public class CreateQuizResultCommand : IRequest<QuizResult>
    {
        public QuizResultCreateModel Model { get; set; } = new();
        class CommandValidation : AbstractValidator<QuizResultViewModel>
        {
            public CommandValidation()
            {
                RuleFor(x => x.QuizId).NotEmpty().WithMessage("Quiz ID is required");
                RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required");
                RuleForEach(x => x.Answers).SetValidator(new QuizAnswerValidator());
            }
        }

        public class QuizAnswerValidator : AbstractValidator<QuizAnswerCreateModel>
        {
            public QuizAnswerValidator()
            {
                RuleFor(x => x.QuestionId).NotEmpty().WithMessage("Question ID is required");
                RuleFor(x => x.Answer).Null();
            }
        }
        class CommandHandler(IUnitOfWork _unitOfWork, IBackgroundJobClient _backgroundJobClient, IMediator mediator) : IRequestHandler<CreateQuizResultCommand, QuizResult>
        {
            public async Task<QuizResult> Handle(CreateQuizResultCommand request, CancellationToken cancellationToken)
            {
                var quiz = await _unitOfWork.QuizRepository
                    .GetByIdAsync(request.Model.QuizId, includes: x => x.Questions.Where(x => !x.IsDeleted))
                    ?? throw new Exception("Invalid quiz id");

                var answers = request.Model.Answers;

                // Tạo và tính toán kết quả bài kiểm tra
                var quizResult = TrackQuizResult(quiz, answers);

                // Lưu kết quả vào database
                await _unitOfWork.QuizResultRepository.AddAsync(quizResult, cancellationToken);
                if (request.Model.Type == QUIZ_RESULT_TYPE.LESSON)
                {
                    var progress = await _unitOfWork
                        .LearningProgressRepository
                        .FirstOrDefaultAsync(x => x.CourseParticipationId == request.Model.ParticipationId && x.LessonId == request.Model.TypeId);

                    if (progress == null)
                    {

                        await _unitOfWork.LearningProgressRepository.AddAsync(new LearningProgress
                        {
                            Status = quizResult.IsPassed ? PROGRESS_STATUS.COMPLETED : PROGRESS_STATUS.IN_PROGRESS,
                            CourseParticipationId = request.Model.ParticipationId,
                            LessonId = request.Model.TypeId,
                            QuizResultId = quizResult.Id
                        }, cancellationToken);
                    }
                    else if (progress!.QuizResultId == null || progress.Status == PROGRESS_STATUS.IN_PROGRESS)
                    {
                        progress.Status = quizResult.IsPassed ? PROGRESS_STATUS.COMPLETED : PROGRESS_STATUS.IN_PROGRESS;
                        _unitOfWork.LearningProgressRepository.Update(progress);
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                _backgroundJobClient.Enqueue(() => CompletedCourse(request.Model.ParticipationId!));
                return quizResult;
            }
            public async Task CompletedCourse(Guid participationId)
            {
                var participation = await _unitOfWork
                    .CourseParticipationRepository
                    .GetByIdAsync(participationId, includes: x => x.LearningProgresses.Where(x => !x.IsDeleted))
                ?? throw new BadRequestException("Chưa tham gia khóa học");

                var modules = await _unitOfWork
                    .CourseSectionRepository
                    .WhereAsync(x => x.CourseId == participation.CourseId, includes: x => x.Lessons.Where(x => !x.IsDeleted));
                var quarter = await _unitOfWork.QuarterRepository
                                       .FirstOrDefaultAsync(q => q.StartDate <= DateTime.UtcNow && q.EndDate >= DateTime.UtcNow)
                                       ?? throw new BadRequestException("No current quarter found");

                var metric = await _unitOfWork
                                      .UserMetricRepository
                                      .FirstOrDefaultAsync(x => x.UserId == participation.UserId && x.QuarterId == quarter.Id) ?? throw new BadRequestException("No user metric found");

                int totalLessons = modules.Sum(module => module.Lessons.Count);
                int completedLesson = participation.LearningProgresses.Where(x => x.Status == PROGRESS_STATUS.COMPLETED).Count();
                if (totalLessons == completedLesson)
                {
                    metric.CourseCompletedNum += 1;
                    metric.PointInQuarter += 1000;
                    _unitOfWork.UserMetricRepository.Update(metric);

                    participation.Status = COURSE_PARTICIPATION_STATUS.COMPLETED;
                    _unitOfWork.CourseParticipationRepository.Update(participation);
                    var course_result = new CourseResult
                    {
                        Scrore = 10,
                        IsPassed = true,
                        CompletedDate = DateTime.UtcNow,
                        CourseId = participation.CourseId,
                        UserId = participation.UserId,
                        CourseParticipationId = participation.Id,
                        CourseNumber = StringUtilities.GenerateRandomCode(8)
                    };

                    await _unitOfWork.CourseResultRepository.AddAsync(course_result);
                    await _unitOfWork.SaveChangesAsync();
                    await mediator.Send(new CreateNotificationCommand()
                    {
                        Model = new NotificationCreateModel
                        {
                            UserId = participation.UserId,
                            Type = NotificationType.COURSE_COMPLETED,

                        }
                    });

                    await mediator.Send(new CreateNotificationCommand()
                    {
                        Model = new NotificationCreateModel
                        {
                            UserId = participation.UserId,
                            Type = NotificationType.POINTS_BONUS,

                        }
                    });
                }
            }
            private static QuizResult TrackQuizResult(Quiz quiz, IEnumerable<QuizAnswerCreateModel> userAnswers)
            {
                // Tạo đối tượng QuizResult mới
                var quizResult = new QuizResult
                {
                    CompletedDate = DateTime.UtcNow,
                    QuizAnswers = []
                };

                int correctAnswers = 0;
                int totalAnsweredQuestions = 0;
                var questionDict = quiz.Questions.ToDictionary(q => q.Id);

                // Xử lý từng câu trả lời của người dùng
                foreach (var userAnswer in userAnswers)
                {
                    // Kiểm tra xem câu hỏi có tồn tại trong quiz không
                    if (questionDict.TryGetValue(userAnswer.QuestionId, out var question))
                    {
                        totalAnsweredQuestions++;

                        // Tạo đối tượng QuizAnswer
                        var quizAnswer = new QuizAnswer
                        {
                            QuestionId = userAnswer.QuestionId,
                            Answer = userAnswer.Answer!,
                            IsCorrect = false,
                            QuizResultId = quizResult.Id
                        };

                        // Kiểm tra câu trả lời có đúng không bằng cách so sánh với CorrectAnswer
                        if (question.CorrectAnswer.Equals(userAnswer.Answer, StringComparison.OrdinalIgnoreCase))
                        {
                            quizAnswer.IsCorrect = true;
                            correctAnswers++; // Đếm số câu trả lời đúng
                        }

                        // Thêm câu trả lời vào danh sách
                        quizResult.QuizAnswers.Add(quizAnswer);
                    }
                }

                // Tính điểm theo thang điểm 10
                double totalScore = totalAnsweredQuestions > 0
                    ? Math.Round((double)correctAnswers / totalAnsweredQuestions * 10, 2)
                    : 0;

                // Cập nhật điểm số và trạng thái đỗ/trượt
                quizResult.Score = totalScore;
                quizResult.IsPassed = totalScore >= quiz.PassedMark;

                return quizResult;
            }
        }
    }
}
