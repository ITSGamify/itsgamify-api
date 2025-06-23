using AutoMapper;
using its.gamify.core;
using its.gamify.core.Repositories;
using its.gamify.domains.Enums;
using its.gamify.infras.Datas;
using Microsoft.Extensions.DependencyInjection;

namespace its.gamify.infras
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICourseRepository _courseRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IPracticeRepository _practiceRepository;
        private readonly IPracticeTagRepository practiceTagRepository;
        private readonly IBadgeRepository _badgeRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDifficultyRepository _difficultyRepository;
        private readonly IEmployeeMetricRepository _employeeMetricRepository;
        private readonly ILeaderBoardRepository _leaderBoardRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseSectionRepository _courseSectionRepository;
        private readonly ILearningProgressRepository _learningProgressRepository;
        private readonly ICourseReviewRepository _courseReviewRepository;
        private readonly ICourseParticipationRepository _courseParticipationRepository;
        private readonly IWishListRepository _wishListRepository;
        private readonly ICourseResultRepository _courseResultRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IQuarterRepository _quarterRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IQuizAnswerRepository _quizAnswerRepository;
        private readonly IQuizResultRepository _quizResultRepository;
        private readonly IChallengeRepository _challengeRepository;
        private readonly IChallengeParticipationRepository _challengeParticipationRepository;

        private readonly IMapper mapper;
        public UnitOfWork(AppDbContext dbContext, ICourseRepository courseRepository, IDepartmentRepository departmentRepository,
            IServiceProvider serviceProvider)
        {
            roleRepository = serviceProvider.GetRequiredService<IRoleRepository>();
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            mapper = serviceProvider.GetRequiredService<IMapper>();
            _appDbContext = dbContext;
            _courseRepository = courseRepository;
            practiceTagRepository = serviceProvider.GetRequiredService<IPracticeTagRepository>();
            _practiceRepository = serviceProvider.GetRequiredService<IPracticeRepository>();
            _departmentRepository = departmentRepository;
            _badgeRepository = serviceProvider.GetRequiredService<IBadgeRepository>();
            _categoryRepository = serviceProvider.GetRequiredService<ICategoryRepository>();
            _difficultyRepository = serviceProvider.GetRequiredService<IDifficultyRepository>();
            _employeeMetricRepository = serviceProvider.GetRequiredService<IEmployeeMetricRepository>();
            _leaderBoardRepository = serviceProvider.GetRequiredService<ILeaderBoardRepository>();
            _lessonRepository = serviceProvider.GetRequiredService<ILessonRepository>();
            _courseSectionRepository = serviceProvider.GetRequiredService<ICourseSectionRepository>();
            _learningProgressRepository = serviceProvider.GetRequiredService<ILearningProgressRepository>();
            _courseReviewRepository = serviceProvider.GetRequiredService<ICourseReviewRepository>();
            _courseParticipationRepository = serviceProvider.GetRequiredService<ICourseParticipationRepository>();
            _wishListRepository = serviceProvider.GetRequiredService<IWishListRepository>();
            _courseResultRepository = serviceProvider.GetRequiredService<ICourseResultRepository>();
            _notificationRepository = serviceProvider.GetRequiredService<INotificationRepository>();
            _quarterRepository = serviceProvider.GetRequiredService<IQuarterRepository>();
            _questionRepository = serviceProvider.GetRequiredService<IQuestionRepository>();
            _quizRepository = serviceProvider.GetRequiredService<IQuizRepository>();
            _quizAnswerRepository = serviceProvider.GetRequiredService<IQuizAnswerRepository>();
            _quizResultRepository = serviceProvider.GetRequiredService<IQuizResultRepository>();
            _challengeRepository = serviceProvider.GetRequiredService<IChallengeRepository>();
            _challengeParticipationRepository = serviceProvider.GetRequiredService<IChallengeParticipationRepository>();
        }
        public ICourseRepository CourseRepository => _courseRepository;
        public IMapper Mapper => mapper;
        public IUserRepository UserRepository => _userRepository;
        public IDepartmentRepository DepartmentRepository => _departmentRepository;
        public IRoleRepository RoleRepository => roleRepository;
        public IPracticeRepository PracticeRepository => _practiceRepository;
        public IPracticeTagRepository PracticeTagRepository => practiceTagRepository;
        public IBadgeRepository BadgeRepository => _badgeRepository;
        public ICategoryRepository CategoryRepository => _categoryRepository;
        public IDifficultyRepository DifficultyRepository => _difficultyRepository;
        public IEmployeeMetricRepository EmployeeMetricRepository => _employeeMetricRepository;
        public ILeaderBoardRepository LeaderBoardRepository => _leaderBoardRepository;
        public ILessonRepository LessonRepository => _lessonRepository;
        public ICourseSectionRepository CourseSectionRepository => _courseSectionRepository;
        public ILearningProgressRepository LearningProgressRepository => _learningProgressRepository;
        public ICourseReviewRepository CourseReviewRepository => _courseReviewRepository;
        public ICourseParticipationRepository CourseParticipationRepository => _courseParticipationRepository;
        public IWishListRepository WishListRepository => _wishListRepository;
        public ICourseResultRepository CourseResultRepository => _courseResultRepository;
        public INotificationRepository NotificationRepository => _notificationRepository;
        public IQuarterRepository QuarterRepository => _quarterRepository;
        public IQuestionRepository QuestionRepository => _questionRepository;
        public IQuizRepository QuizRepository => _quizRepository;
        public IQuizAnswerRepository QuizAnswerRepository => _quizAnswerRepository;
        public IQuizResultRepository QuizResultRepository => _quizResultRepository;
        public IChallengeRepository ChallengeRepository => _challengeRepository;
        public IChallengeParticipationRepository ChallengeParticipationRepository => _challengeParticipationRepository;
        public async Task<bool> SaveChangesAsync()
        => await _appDbContext.SaveChangesAsync() > 0;

        public async Task SeedData()
        {
            await this.roleRepository.AddRangeAsync(new List<domains.Entities.Role>()
            {
                new domains.Entities.Role()
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    Name = RoleEnum.Manager.ToString(),
                    Description = "",

                },
                 new domains.Entities.Role()
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    Name = RoleEnum.Admin.ToString(),
                    Description = "",

                },
                  new domains.Entities.Role()
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    Name = RoleEnum.Employee.ToString(),
                    Description = "",

                }
            });
            await this.SaveChangesAsync();
        }
    }
}
