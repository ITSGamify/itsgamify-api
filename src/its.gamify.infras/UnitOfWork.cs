using AutoMapper;
using its.gamify.core;
using its.gamify.core.Repositories;
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
        private readonly ILearningMaterialRepository learningMaterialRepository;
        private readonly IFileRepository _fileRepository;

        private readonly IMapper mapper;
        public UnitOfWork(AppDbContext dbContext, ICourseRepository courseRepository, IDepartmentRepository departmentRepository,
            IServiceProvider serviceProvider)
        {
            _fileRepository = serviceProvider.GetRequiredService<IFileRepository>();
            roleRepository = serviceProvider.GetRequiredService<IRoleRepository>();
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            mapper = serviceProvider.GetRequiredService<IMapper>();
            _appDbContext = dbContext;
            learningMaterialRepository = serviceProvider.GetRequiredService<ILearningMaterialRepository>();
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
        public IFileRepository FileRepository => _fileRepository;
        public IQuizAnswerRepository QuizAnswerRepository => _quizAnswerRepository;
        public IQuizResultRepository QuizResultRepository => _quizResultRepository;
        public IChallengeRepository ChallengeRepository => _challengeRepository;
        public ILearningMaterialRepository LearningMaterialRepository => learningMaterialRepository;
        public IChallengeParticipationRepository ChallengeParticipationRepository => _challengeParticipationRepository;
        public async Task<bool> SaveChangesAsync()
        => await _appDbContext.SaveChangesAsync() > 0;

        public async Task SeedData()
        {
            // Track seeded entity IDs for FK relationships
            var seededIds = new Dictionary<string, List<Guid>>();

            // Seed Roles
            var roles = new List<domains.Entities.Role>
            {
                new domains.Entities.Role { Id = Guid.NewGuid(), Name = "Admin", Description = "", IsDeleted = false },
                new domains.Entities.Role { Id = Guid.NewGuid(), Name = "Manager", Description = "", IsDeleted = false },
                new domains.Entities.Role { Id = Guid.NewGuid(), Name = "Employee", Description = "", IsDeleted = false }
            };
            if (!(await this.roleRepository.GetAllAsync()).Any())
            {
                await this.roleRepository.AddRangeAsync(roles);
                await this.SaveChangesAsync();
            }
            seededIds["Role"] = roles.Select(r => r.Id).ToList();

            // Seed Departments
            var departments = new List<domains.Entities.Department>
            {
                new domains.Entities.Department { Id = Guid.NewGuid(), Name = "IT", Description = "IT Department", Location = "HQ" },
                new domains.Entities.Department { Id = Guid.NewGuid(), Name = "HR", Description = "HR Department", Location = "HQ" }
            };
            if (!(await this._departmentRepository.GetAllAsync()).Any())
            {
                await this._departmentRepository.AddRangeAsync(departments);
                await this.SaveChangesAsync();
            }
            seededIds["Department"] = departments.Select(d => d.Id).ToList();

            // Seed Users with DepartmentId and RoleId FK
            var users = new List<domains.Entities.User>
            {
                new domains.Entities.User { Id = Guid.NewGuid(), EmpployeeCode = "EMP001", Username = "admin", Email = "admin@example.com", FirstName = "Admin", LastName = "User", Status = "Active", DepartmentId = departments[0].Id, RoleId = roles[0].Id },
                new domains.Entities.User { Id = Guid.NewGuid(), EmpployeeCode = "EMP002", Username = "manager", Email = "manager@example.com", FirstName = "Manager", LastName = "User", Status = "Active", DepartmentId = departments[1].Id, RoleId = roles[1].Id }
            };
            if (!(await this._userRepository.GetAllAsync()).Any())
            {
                await this._userRepository.AddRangeAsync(users);
                await this.SaveChangesAsync();
            }
            seededIds["User"] = users.Select(u => u.Id).ToList();

            // Seed Categories
            var categories = new List<domains.Entities.Category>
            {
                new domains.Entities.Category { Id = Guid.NewGuid(), Name = "Programming", Description = "Programming Courses" },
                new domains.Entities.Category { Id = Guid.NewGuid(), Name = "Soft Skills", Description = "Soft Skills Courses" }
            };
            if (!(await this._categoryRepository.GetAllAsync()).Any())
            {
                await this._categoryRepository.AddRangeAsync(categories);
                await this.SaveChangesAsync();
            }
            seededIds["Category"] = categories.Select(c => c.Id).ToList();

            // Seed Difficulties
            var difficulties = new List<domains.Entities.Difficulty>
            {
                new domains.Entities.Difficulty { Id = Guid.NewGuid(), Name = "Beginner", Description = "" },
                new domains.Entities.Difficulty { Id = Guid.NewGuid(), Name = "Intermediate", Description = "" },
                new domains.Entities.Difficulty { Id = Guid.NewGuid(), Name = "Advanced", Description = "" }
            };
            if (!(await this._difficultyRepository.GetAllAsync()).Any())
            {
                await this._difficultyRepository.AddRangeAsync(difficulties);
                await this.SaveChangesAsync();
            }
            seededIds["Difficulty"] = difficulties.Select(d => d.Id).ToList();

            // Seed Quarters
            var quarters = new List<domains.Entities.Quarter>
            {
                new domains.Entities.Quarter { Id = Guid.NewGuid(), Name = "Q1", Year = DateTime.Now.Year }
            };
            if (!(await this._quarterRepository.GetAllAsync()).Any())
            {
                await this._quarterRepository.AddRangeAsync(quarters);
                await this.SaveChangesAsync();
            }
            seededIds["Quarter"] = quarters.Select(q => q.Id).ToList();

            // Seed Courses with FK to Category, Difficulty, Quarter
            var courses = new List<domains.Entities.Course>
            {
                new domains.Entities.Course { Id = Guid.NewGuid(), Title = "Sample Course", DurationInHours = 10, Description = "Description", QuarterId = quarters[0].Id, CategoryId = categories[0].Id }
            };
            if (!(await this._courseRepository.GetAllAsync()).Any())
            {
                await this._courseRepository.AddRangeAsync(courses);
                await this.SaveChangesAsync();
            }
            seededIds["Course"] = courses.Select(c => c.Id).ToList();

            // Seed CourseSections with FK to Course
            var courseSections = new List<domains.Entities.CourseSection>
            {
                new domains.Entities.CourseSection { Id = Guid.NewGuid(), Title = "Section 1", OrderedNumber = 1, CourseId = courses[0].Id }
            };
            if (!(await this._courseSectionRepository.GetAllAsync()).Any())
            {
                await this._courseSectionRepository.AddRangeAsync(courseSections);
                await this.SaveChangesAsync();
            }
            seededIds["CourseSection"] = courseSections.Select(cs => cs.Id).ToList();

            // Seed CourseParticipations with FK to User and Course
            var courseParticipations = new List<domains.Entities.CourseParticipation>
            {
                new domains.Entities.CourseParticipation { Id = Guid.NewGuid(), EnrolledDate = DateTime.Now, Status = "Enrolled", UserId = users[0].Id, CourseId = courses[0].Id }
            };
            if (!(await this._courseParticipationRepository.GetAllAsync()).Any())
            {
                await this._courseParticipationRepository.AddRangeAsync(courseParticipations);
                await this.SaveChangesAsync();
            }
            seededIds["CourseParticipation"] = courseParticipations.Select(cp => cp.Id).ToList();

            // Seed LearningProgresses with FK to CourseParticipation
            var learningProgresses = new List<domains.Entities.LearningProgress>
            {
                new domains.Entities.LearningProgress { Id = Guid.NewGuid(), Percentage = 100, LastAccessed = DateTime.Now, CourseParticipationId = courseParticipations[0].Id }
            };
            if (!(await this._learningProgressRepository.GetAllAsync()).Any())
            {
                await this._learningProgressRepository.AddRangeAsync(learningProgresses);
                await this.SaveChangesAsync();
            }
            seededIds["LearningProgress"] = learningProgresses.Select(lp => lp.Id).ToList();

            // Seed Lessons with FK to CourseSection and LearningProgress (if required)
            var lessons = new List<domains.Entities.Lesson>
            {
                new domains.Entities.Lesson { Id = Guid.NewGuid(), Title = "Sample Lesson", Description = "Description", DurationInMinutes = 60, Type = "Video", Url = "http://example.com", CourseSectionId = courseSections[0].Id, LearningProgressId = learningProgresses[0].Id }
            };
            if (!(await this._lessonRepository.GetAllAsync()).Any())
            {
                await this._lessonRepository.AddRangeAsync(lessons);
                await this.SaveChangesAsync();
            }
            seededIds["Lesson"] = lessons.Select(l => l.Id).ToList();

            // Seed Challenges
            var challenges = new List<domains.Entities.Challenge>
            {
                new domains.Entities.Challenge { Id = Guid.NewGuid(), Title = "Sample Challenge", Description = "Description" }
            };
            if (!(await this._challengeRepository.GetAllAsync()).Any())
            {
                await this._challengeRepository.AddRangeAsync(challenges);
                await this.SaveChangesAsync();
            }
            seededIds["Challenge"] = challenges.Select(c => c.Id).ToList();

            // Seed Quizzes with FK to Lesson and Challenge
            var quizzes = new List<domains.Entities.Quiz>
            {
                new domains.Entities.Quiz { Id = Guid.NewGuid(), TotalMarks = 10, PassedMarks = 5, TotalQuestions = 1, LessonId = lessons[0].Id, ChallengIdId = challenges[0].Id }
            };
            if (!(await this._quizRepository.GetAllAsync()).Any())
            {
                await this._quizRepository.AddRangeAsync(quizzes);
                await this.SaveChangesAsync();
            }
            seededIds["Quiz"] = quizzes.Select(q => q.Id).ToList();

            // Seed Questions with FK to Quiz
            var questions = new List<domains.Entities.Question>
            {
                new domains.Entities.Question { Id = Guid.NewGuid(), Content = "Sample Question", OptionA = "A", OptionB = "B", OptionC = "C", OptionD = "D", CorrectAnswer = "A", Explanation = "Sample", QuizId = quizzes[0].Id }
            };
            if (!(await this._questionRepository.GetAllAsync()).Any())
            {
                await this._questionRepository.AddRangeAsync(questions);
                await this.SaveChangesAsync();
            }
            seededIds["Question"] = questions.Select(q => q.Id).ToList();

            // Seed QuizResults
            var quizResults = new List<domains.Entities.QuizResult>
            {
                new domains.Entities.QuizResult { Id = Guid.NewGuid(), Score = 100, CompletedDate = DateTime.Now, IsPassed = true }
            };
            if (!(await this._quizResultRepository.GetAllAsync()).Any())
            {
                await this._quizResultRepository.AddRangeAsync(quizResults);
                await this.SaveChangesAsync();
            }
            seededIds["QuizResult"] = quizResults.Select(qr => qr.Id).ToList();

            // Seed QuizAnswers with FK to Question and QuizResult
            var quizAnswers = new List<domains.Entities.QuizAnswer>
            {
                new domains.Entities.QuizAnswer { Id = Guid.NewGuid(), Answer = "A", IsCorrect = true, QuestionId = questions[0].Id, QuizResultId = quizResults[0].Id }
            };
            if (!(await this._quizAnswerRepository.GetAllAsync()).Any())
            {
                await this._quizAnswerRepository.AddRangeAsync(quizAnswers);
                await this.SaveChangesAsync();
            }
            seededIds["QuizAnswer"] = quizAnswers.Select(qa => qa.Id).ToList();

            // Seed Practices with FK to Course and CourseSection
            var practices = new List<domains.Entities.Practice>
            {
                new domains.Entities.Practice { Id = Guid.NewGuid(), Title = "Sample Practice", Description = "Description", CourseId = courses[0].Id, CourseSectionId = courseSections[0].Id }
            };
            if (!(await this._practiceRepository.GetAllAsync()).Any())
            {
                await this._practiceRepository.AddRangeAsync(practices);
                await this.SaveChangesAsync();
            }
            seededIds["Practice"] = practices.Select(p => p.Id).ToList();

            // Seed PracticeTags with FK to Practice
            var practiceTags = new List<domains.Entities.PracticeTag>
            {
                new domains.Entities.PracticeTag { Id = Guid.NewGuid(), TagName = "Sample Tag", PracticeId = practices[0].Id }
            };
            if (!(await this.practiceTagRepository.GetAllAsync()).Any())
            {
                await this.practiceTagRepository.AddRangeAsync(practiceTags);
                await this.SaveChangesAsync();
            }
            seededIds["PracticeTag"] = practiceTags.Select(pt => pt.Id).ToList();

            // Seed WishLists with FK to User and Course
            var wishLists = new List<domains.Entities.WishList>
            {
                new domains.Entities.WishList { Id = Guid.NewGuid(), Name = "Sample WishList", UserId = users[0].Id, CourseId = courses[0].Id }
            };
            if (!(await this._wishListRepository.GetAllAsync()).Any())
            {
                await this._wishListRepository.AddRangeAsync(wishLists);
                await this.SaveChangesAsync();
            }
            seededIds["WishList"] = wishLists.Select(w => w.Id).ToList();

            // Seed Notifications with FK to User
            var notifications = new List<domains.Entities.Notification>
            {
                new domains.Entities.Notification { Id = Guid.NewGuid(), Title = "Sample Notification", Message = "Message", Precedence = 1, IsNotified = false, IsRead = false, UserId = users[0].Id }
            };
            if (!(await this._notificationRepository.GetAllAsync()).Any())
            {
                await this._notificationRepository.AddRangeAsync(notifications);
                await this.SaveChangesAsync();
            }
            seededIds["Notification"] = notifications.Select(n => n.Id).ToList();

            // Remove cyclic references for JSON serialization (if any navigation properties are set)
            // This is only needed if you are serializing entities to JSON and navigation properties are causing cycles.
            // In seeding, you should NOT set navigation properties (object references), only FK IDs.
            // If you have set navigation properties, set them to null after seeding, e.g.:
            // foreach (var lesson in lessons) lesson.LearningProgress = null;
            // foreach (var courseSection in courseSections) courseSection.Course = null;
            // ...repeat for other entities as needed...
            // But ideally, do not set navigation properties at all during seeding.

            // Seed LearningMaterials with FK to Course
            var learningMaterials = new List<domains.Entities.LearningMaterial>
            {
                new domains.Entities.LearningMaterial { Id = Guid.NewGuid(), Title = "Sample Material", Description = "Description", Type = "Video", Url = "http://example.com", CourseId = courses[0].Id }
            };
            if (!(await this.learningMaterialRepository.GetAllAsync()).Any())
            {
                await this.learningMaterialRepository.AddRangeAsync(learningMaterials);
                await this.SaveChangesAsync();
            }
            seededIds["LearningMaterial"] = learningMaterials.Select(lm => lm.Id).ToList();

            // Seed LeadearBoards
            var leadearBoards = new List<domains.Entities.LeadearBoard>
            {
                new domains.Entities.LeadearBoard { Id = Guid.NewGuid(), Name = "Sample Board", Description = "Description" }
            };
            if (!(await this._leaderBoardRepository.GetAllAsync()).Any())
            {
                await this._leaderBoardRepository.AddRangeAsync(leadearBoards);
                await this.SaveChangesAsync();
            }
            seededIds["LeadearBoard"] = leadearBoards.Select(lb => lb.Id).ToList();

            // Seed EmployeeMetrics with FK to User
            var employeeMetrics = new List<domains.Entities.EmployeeMetric>
            {
                new domains.Entities.EmployeeMetric { Id = Guid.NewGuid(), Description = "Sample Metric", UserId = users[0].Id }
            };
            if (!(await this._employeeMetricRepository.GetAllAsync()).Any())
            {
                await this._employeeMetricRepository.AddRangeAsync(employeeMetrics);
                await this.SaveChangesAsync();
            }
            seededIds["EmployeeMetric"] = employeeMetrics.Select(em => em.Id).ToList();

            // Seed CourseReviews with FK to Course and CourseParticipation
            var courseReviews = new List<domains.Entities.CourseReview>
            {
                new domains.Entities.CourseReview { Id = Guid.NewGuid(), Rating = 5, Comment = "Great!", ReviewdDate = DateTime.Now, CourseId = courses[0].Id, CourseParticipationId = courseParticipations[0].Id }
            };
            if (!(await this._courseReviewRepository.GetAllAsync()).Any())
            {
                await this._courseReviewRepository.AddRangeAsync(courseReviews);
                await this.SaveChangesAsync();
            }
            seededIds["CourseReview"] = courseReviews.Select(cr => cr.Id).ToList();

            // Seed CourseResults with FK to User, Course, CourseParticipation
            var courseResults = new List<domains.Entities.CourseResult>
            {
                new domains.Entities.CourseResult { Id = Guid.NewGuid(), Scrore = 100, IsPassed = true, CompletedDate = DateTime.Now, UserId = users[0].Id, CourseId = courses[0].Id, CourseParticipationId = courseParticipations[0].Id }
            };
            if (!(await this._courseResultRepository.GetAllAsync()).Any())
            {
                await this._courseResultRepository.AddRangeAsync(courseResults);
                await this.SaveChangesAsync();
            }
            seededIds["CourseResult"] = courseResults.Select(cr => cr.Id).ToList();

            // Seed ChallengeParticipations with FK to Challenge and User (Employee)
            var challengeParticipations = new List<domains.Entities.ChallengeParticipation>
            {
                new domains.Entities.ChallengeParticipation { Id = Guid.NewGuid(), Status = "Active", ChallengeId = challenges[0].Id, EmployeeId = users[0].Id }
            };
            if (!(await this._challengeParticipationRepository.GetAllAsync()).Any())
            {
                await this._challengeParticipationRepository.AddRangeAsync(challengeParticipations);
                await this.SaveChangesAsync();
            }
            seededIds["ChallengeParticipation"] = challengeParticipations.Select(cp => cp.Id).ToList();

            // Seed Badges with FK to User
            var badges = new List<domains.Entities.Badge>
            {
                new domains.Entities.Badge { Id = Guid.NewGuid(), Name = "Sample Badge", Description = "Description", ClaimedDate = DateTime.Now, UserId = users[0].Id }
            };
            if (!(await this._badgeRepository.GetAllAsync()).Any())
            {
                await this._badgeRepository.AddRangeAsync(badges);
                await this.SaveChangesAsync();
            }
            seededIds["Badge"] = badges.Select(b => b.Id).ToList();

            await this.SaveChangesAsync();
        }
    }
}

// In your Program.cs (or Startup.cs for older .NET versions), configure JSON options to ignore cycles:
// For .NET 6+ minimal hosting model:
//
// var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddControllers().AddJsonOptions(options =>
// {
//     options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
// });
// var app = builder.Build();
// app.MapControllers();
// app.Run();
//
// For .NET 5 and earlier, or if you prefer Startup.cs:
//
// public class Startup
// {
//     public void ConfigureServices(IServiceCollection services)
//     {
//         services.AddControllers().AddJsonOptions(options =>
//         {
//             options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
//         });
//     }
//
//     public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//     {
//         if (env.IsDevelopment())
//         {
//             app.UseDeveloperExceptionPage();
//         }
//         else
//         {
//             app.UseExceptionHandler("/Home/Error");
//             app.UseHsts();
//         }
//         app.UseHttpsRedirection();
//         app.UseStaticFiles();
//
//         app.UseRouting();
//
//         app.UseAuthorization();
//
//         app.UseEndpoints(endpoints =>
//         {
//             endpoints.MapControllers();
//         });
//     }
// }
