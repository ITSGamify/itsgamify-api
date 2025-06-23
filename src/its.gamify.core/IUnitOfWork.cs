using AutoMapper;
using its.gamify.core.Repositories;

namespace its.gamify.core
{
    public interface IUnitOfWork
    {
        public IMapper Mapper { get; }
        public IRoleRepository RoleRepository { get; }
        public ICourseRepository CourseRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }
        public IUserRepository UserRepository { get; }
        public IPracticeTagRepository PracticeTagRepository { get; }
        public IPracticeRepository PracticeRepository { get; }
        public IBadgeRepository BadgeRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IDifficultyRepository DifficultyRepository { get; }
        public IEmployeeMetricRepository EmployeeMetricRepository { get; }
        public ILeaderBoardRepository LeaderBoardRepository { get; }
        public ILessonRepository LessonRepository { get; }
        public ICourseSectionRepository CourseSectionRepository { get; }
        public ILearningProgressRepository LearningProgressRepository { get; }
        public ICourseReviewRepository CourseReviewRepository { get; }
        public ICourseParticipationRepository CourseParticipationRepository { get; }
        public IWishListRepository WishListRepository { get; }
        public ICourseResultRepository CourseResultRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public IQuarterRepository QuarterRepository { get; }
        public IQuestionRepository QuestionRepository { get; }
        public IQuizRepository QuizRepository { get; }
        public IQuizAnswerRepository QuizAnswerRepository { get; }
        public IQuizResultRepository QuizResultRepository { get; }
        public IChallengeRepository ChallengeRepository { get; }
        public IChallengeParticipationRepository ChallengeParticipationRepository { get; }
        Task<bool> SaveChangesAsync();
        Task SeedData();
    }
}
