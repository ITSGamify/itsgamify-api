using Firebase.Auth;
using its.gamify.domains.Entities;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Text.Json;

namespace its.gamify.core.Features.AvailablesData
{
    public static class UltilsFunction
    {
        public static List<T> PaginationFunction<T>(this List<T> source, int pageNumber = 0, int pageSize = 10)
        {
            if (source == null) return new List<T>();
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            return source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
    public class Ultils
    {
        public void AddList()
        {
            List<QuizAnswer> quizAnswers = new List<QuizAnswer>();
            for (int i = 0; i < 60; i++)
            {
                quizAnswers.Add(new QuizAnswer
                {
                    Id = Guid.NewGuid(),
                    Answer = $"Answer {((char)('A' + (i % 4))).ToString()}",
                    IsCorrect = (i % 2 == 0),
                    QuestionId = Guid.NewGuid(),
                    QuizResultId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsDeleted = false,
                    CreatedBy = Guid.NewGuid(),
                    UpdatedBy = Guid.NewGuid()
                });
            }
            for (int i = 1; i <= 10; i++)
            {
                leaderBoards.Add(new LeadearBoard
                {
                    Name = $"LeaderBoard {i}",
                    Description = $"Description for LeaderBoard {i}."
                });
            }
            for (int i = 1; i <= 10; i++)
            {
                badges.Add(new Badge
                {
                    Name = $"Badge {i}",
                    Description = $"Description for Badge {i}.",
                    ClaimedDate = DateTime.Now.AddDays(-i), // Ngày được trao giảm dần
                    UserId = Guid.NewGuid(),
                });
            }
            for (int i = 1; i <= 10; i++)
            {
                employeeMetrics.Add(new EmployeeMetric
                {
                    Description = $"Employee Metric for User {i}.",
                    UserId = Guid.NewGuid()
                });
            }
            for (int i = 1; i <= 10; i++)
            {
                notifications.Add(new Notification
                {
                    Title = $"Notification {i}",
                    Message = $"Message for Notification {i}.",
                    Precedence = i, // Độ ưu tiên tăng dần
                    UserId = Guid.NewGuid()
                });
            }

        }
        private readonly string _filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Feature", "AvailableData.json");
        public List<Quarter> quarters;
        public List<QuizResult> quizResults;
        public List<Quiz> quizzes;
        public List<Question> questions;
        public List<Course> courses;
        public List<Department> departments;
        public List<Category> categories;
        public List<LeadearBoard> leaderBoards = new List<LeadearBoard>();
        public List<Badge> badges = new List<Badge>();
        public List<EmployeeMetric> employeeMetrics = new List<EmployeeMetric>();
        public List<Notification> notifications = new List<Notification>();
        public Ultils()
        {
            AddList();

            quarters = new List<Quarter>
        {
            new Quarter { Id = Guid.NewGuid(), Name = "Q1 2025", Year = 2025, StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 3, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q2 2025", Year = 2025, StartDate = new DateTime(2025, 4, 1), EndDate = new DateTime(2025, 6, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q3 2025", Year = 2025, StartDate = new DateTime(2025, 7, 1), EndDate = new DateTime(2025, 9, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q4 2025", Year = 2025, StartDate = new DateTime(2025, 10, 1), EndDate = new DateTime(2025, 12, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q1 2026", Year = 2026, StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 3, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q2 2026", Year = 2026, StartDate = new DateTime(2026, 4, 1), EndDate = new DateTime(2026, 6, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q3 2026", Year = 2026, StartDate = new DateTime(2026, 7, 1), EndDate = new DateTime(2026, 9, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q4 2026", Year = 2026, StartDate = new DateTime(2026, 10, 1), EndDate = new DateTime(2026, 12, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q1 2027", Year = 2027, StartDate = new DateTime(2027, 1, 1), EndDate = new DateTime(2027, 3, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q2 2027", Year = 2027, StartDate = new DateTime(2027, 4, 1), EndDate = new DateTime(2027, 6, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q3 2027", Year = 2027, StartDate = new DateTime(2027, 7, 1), EndDate = new DateTime(2027, 9, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q4 2027", Year = 2027, StartDate = new DateTime(2027, 10, 1), EndDate = new DateTime(2027, 12, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q1 2028", Year = 2028, StartDate = new DateTime(2028, 1, 1), EndDate = new DateTime(2028, 3, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q2 2028", Year = 2028, StartDate = new DateTime(2028, 4, 1), EndDate = new DateTime(2028, 6, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q3 2028", Year = 2028, StartDate = new DateTime(2028, 7, 1), EndDate = new DateTime(2028, 9, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q4 2028", Year = 2028, StartDate = new DateTime(2028, 10, 1), EndDate = new DateTime(2028, 12, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q1 2029", Year = 2029, StartDate = new DateTime(2029, 1, 1), EndDate = new DateTime(2029, 3, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q2 2029", Year = 2029, StartDate = new DateTime(2029, 4, 1), EndDate = new DateTime(2029, 6, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q3 2029", Year = 2029, StartDate = new DateTime(2029, 7, 1), EndDate = new DateTime(2029, 9, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q4 2029", Year = 2029, StartDate = new DateTime(2029, 10, 1), EndDate = new DateTime(2029, 12, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q1 2030", Year = 2030, StartDate = new DateTime(2030, 1, 1), EndDate = new DateTime(2030, 3, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q2 2030", Year = 2030, StartDate = new DateTime(2030, 4, 1), EndDate = new DateTime(2030, 6, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q3 2030", Year = 2030, StartDate = new DateTime(2030, 7, 1), EndDate = new DateTime(2030, 9, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q4 2030", Year = 2030, StartDate = new DateTime(2030, 10, 1), EndDate = new DateTime(2030, 12, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q1 2031", Year = 2031, StartDate = new DateTime(2031, 1, 1), EndDate = new DateTime(2031, 3, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q2 2031", Year = 2031, StartDate = new DateTime(2031, 4, 1), EndDate = new DateTime(2031, 6, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q3 2031", Year = 2031, StartDate = new DateTime(2031, 7, 1), EndDate = new DateTime(2031, 9, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q4 2031", Year = 2031, StartDate = new DateTime(2031, 10, 1), EndDate = new DateTime(2031, 12, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q1 2032", Year = 2032, StartDate = new DateTime(2032, 1, 1), EndDate = new DateTime(2032, 3, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q2 2032", Year = 2032, StartDate = new DateTime(2032, 4, 1), EndDate = new DateTime(2032, 6, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q3 2032", Year = 2032, StartDate = new DateTime(2032, 7, 1), EndDate = new DateTime(2032, 9, 30), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quarter { Id = Guid.NewGuid(), Name = "Q4 2032", Year = 2032, StartDate = new DateTime(2032, 10, 1), EndDate = new DateTime(2032, 12, 31), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() }
        };

            quizResults = new List<QuizResult>
        {
            new QuizResult { Id = Guid.NewGuid(), Score = 85.0, CompletedDate = new DateTime(2025, 6, 1), IsPassed = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new QuizResult { Id = Guid.NewGuid(), Score = 70.0, CompletedDate = new DateTime(2025, 6, 2), IsPassed = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new QuizResult { Id = Guid.NewGuid(), Score = 90.0, CompletedDate = new DateTime(2025, 6, 3), IsPassed = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new QuizResult { Id = Guid.NewGuid(), Score = 75.0, CompletedDate = new DateTime(2025, 6, 4), IsPassed = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new QuizResult { Id = Guid.NewGuid(), Score = 60.0, CompletedDate = new DateTime(2025, 6, 5), IsPassed = false, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new QuizResult { Id = Guid.NewGuid(), Score = 95.0, CompletedDate = new DateTime(2025, 6, 6), IsPassed = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new QuizResult { Id = Guid.NewGuid(), Score = 80.0, CompletedDate = new DateTime(2025, 6, 7), IsPassed = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new QuizResult { Id = Guid.NewGuid(), Score = 55.0, CompletedDate = new DateTime(2025, 6, 8), IsPassed = false, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new QuizResult { Id = Guid.NewGuid(), Score = 100.0, CompletedDate = new DateTime(2025, 6, 9), IsPassed = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new QuizResult { Id = Guid.NewGuid(), Score = 65.0, CompletedDate = new DateTime(2025, 6, 10), IsPassed = false, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() }
        };

            quizzes = new List<Quiz>
        {
            new Quiz { Id = Guid.NewGuid(), TotalMarks = 100.0, PassedMarks = 60.0, TotalQuestions = 10, LessonId = Guid.NewGuid(), ChallengIdId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quiz { Id = Guid.NewGuid(), TotalMarks = 100.0, PassedMarks = 70.0, TotalQuestions = 15, LessonId = Guid.NewGuid(), ChallengIdId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quiz { Id = Guid.NewGuid(), TotalMarks = 100.0, PassedMarks = 65.0, TotalQuestions = 12, LessonId = Guid.NewGuid(), ChallengIdId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quiz { Id = Guid.NewGuid(), TotalMarks = 100.0, PassedMarks = 75.0, TotalQuestions = 20, LessonId = Guid.NewGuid(), ChallengIdId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quiz { Id = Guid.NewGuid(), TotalMarks = 100.0, PassedMarks = 80.0, TotalQuestions = 18, LessonId = Guid.NewGuid(), ChallengIdId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Quiz { Id = Guid.NewGuid(), TotalMarks = 100.0, PassedMarks = 85.0, TotalQuestions = 25, LessonId = Guid.NewGuid(), ChallengIdId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() }
        };

            questions = new List<Question>
        {
            new Question { Id = Guid.NewGuid(), Content = "What is the capital of France?", OptionA = "Berlin", OptionB = "Madrid", OptionC = "Paris", OptionD = "Lisbon", CorrectAnswer = "C", Explanation = "Paris is the capital of France.", QuestionBankId = Guid.NewGuid(), QuizId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Question { Id = Guid.NewGuid(), Content = "What is 2 + 2?", OptionA = "3", OptionB = "4", OptionC = "5", OptionD = "6", CorrectAnswer = "B", Explanation = "2 + 2 equals 4.", QuestionBankId = Guid.NewGuid(), QuizId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Question { Id = Guid.NewGuid(), Content = "What is the largest planet in our solar system?", OptionA = "Earth", OptionB = "Mars", OptionC = "Jupiter", OptionD = "Saturn", CorrectAnswer = "C", Explanation = "Jupiter is the largest planet in our solar system.", QuestionBankId = Guid.NewGuid(), QuizId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Question { Id = Guid.NewGuid(), Content = "What is the boiling point of water?", OptionA = "0°C", OptionB = "100°C", OptionC = "50°C", OptionD = "25°C", CorrectAnswer = "B", Explanation = "The boiling point of water is 100°C.", QuestionBankId = Guid.NewGuid(), QuizId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Question { Id = Guid.NewGuid(), Content = "What is the chemical symbol for water?", OptionA = "O2", OptionB = "H2O", OptionC = "CO2", OptionD = "NaCl", CorrectAnswer = "B", Explanation = "The chemical symbol for water is H2O.", QuestionBankId = Guid.NewGuid(), QuizId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Question { Id = Guid.NewGuid(), Content = "What is the speed of light?", OptionA = "300,000 km/s", OptionB = "150,000 km/s", OptionC = "1,000,000 km/s", OptionD = "200,000 km/s", CorrectAnswer = "A", Explanation = "The speed of light is approximately 300,000 km/s.", QuestionBankId = Guid.NewGuid(), QuizId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() }
        };

            courses = new List<Course>
        {
            new Course { Id = Guid.NewGuid(), Title = "Introduction to Programming", DurationInHours = 40.0, Description = "Learn the basics of programming.", QuarterId = quarters[0].Id, DifficultyLevelId = Guid.NewGuid(), CategoryId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Course { Id = Guid.NewGuid(), Title = "Advanced Data Structures", DurationInHours = 60.0, Description = "Deep dive into data structures.", QuarterId = quarters[1].Id, DifficultyLevelId = Guid.NewGuid(), CategoryId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Course { Id = Guid.NewGuid(), Title = "Machine Learning Basics", DurationInHours = 50.0, Description = "Introduction to machine learning concepts.", QuarterId = quarters[2].Id, DifficultyLevelId = Guid.NewGuid(), CategoryId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Course { Id = Guid.NewGuid(), Title = "Web Development", DurationInHours = 45.0, Description = "Learn to build web applications.", QuarterId = quarters[3].Id, DifficultyLevelId = Guid.NewGuid(), CategoryId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Course { Id = Guid.NewGuid(), Title = "Data Science with Python", DurationInHours = 55.0, Description = "Learn data science techniques using Python.", QuarterId = quarters[4].Id, DifficultyLevelId = Guid.NewGuid(), CategoryId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Course { Id = Guid.NewGuid(), Title = "Cybersecurity Fundamentals", DurationInHours = 30.0, Description = "Introduction to cybersecurity principles.", QuarterId = quarters[5].Id, DifficultyLevelId = Guid.NewGuid(), CategoryId = Guid.NewGuid(), CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() }
        };

            departments = new List<Department>
        {
            new Department { Id = Guid.NewGuid(), Name = "Computer Science", Description = "Department of Computer Science.", Location = "Building A", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Department { Id = Guid.NewGuid(), Name = "Mathematics", Description = "Department of Mathematics.", Location = "Building B", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Department { Id = Guid.NewGuid(), Name = "Physics", Description = "Department of Physics.", Location = "Building C", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Department { Id = Guid.NewGuid(), Name = "Chemistry", Description = "Department of Chemistry.", Location = "Building D", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Department { Id = Guid.NewGuid(), Name = "Biology", Description = "Department of Biology.", Location = "Building E", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Department { Id = Guid.NewGuid(), Name = "History", Description = "Department of History.", Location = "Building F", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Department { Id = Guid.NewGuid(), Name = "Philosophy", Description = "Department of Philosophy.", Location = "Building G", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Department { Id = Guid.NewGuid(), Name = "Sociology", Description = "Department of Sociology.", Location = "Building H", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Department { Id = Guid.NewGuid(), Name = "Political Science", Description = "Department of Political Science.", Location = "Building I", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Department { Id = Guid.NewGuid(), Name = "Linguistics", Description = "Department of Linguistics.", Location = "Building J", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Department { Id = Guid.NewGuid(), Name = "Art History", Description = "Department of Art History.", Location = "Building K", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() }
        };
            categories = new List<Category>
        {
            new Category { Id = Guid.NewGuid(), Name = "Technology", Description = "Courses related to technology and software development.", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Category { Id = Guid.NewGuid(), Name = "Science", Description = "Courses related to various scientific disciplines.", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Category { Id = Guid.NewGuid(), Name = "Mathematics", Description = "Courses related to mathematical concepts and theories.", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Category { Id = Guid.NewGuid(), Name = "Arts", Description = "Courses related to various forms of art and design.", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Category { Id = Guid.NewGuid(), Name = "Literature", Description = "Courses related to literature and writing.", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Category { Id = Guid.NewGuid(), Name = "History", Description = "Courses related to historical events and analysis.", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Category { Id = Guid.NewGuid(), Name = "Philosophy", Description = "Courses related to philosophical concepts and theories.", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Category { Id = Guid.NewGuid(), Name = "Business", Description = "Courses related to business management and entrepreneurship.", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Category { Id = Guid.NewGuid(), Name = "Health", Description = "Courses related to health and wellness.", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() },
            new Category { Id = Guid.NewGuid(), Name = "Engineering", Description = "Courses related to various engineering disciplines.", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDeleted = false, CreatedBy = Guid.NewGuid(), UpdatedBy = Guid.NewGuid() }
        };

        }
       
       /* public async Task<List<Quarter>> GetAllQuarterssAsync()
        {
            if (!File.Exists(_filePath))
                return new List<Quarter>();

            var json = await File.ReadAllTextAsync(_filePath);
            var objectReadable = JsonSerializer.Deserialize<QuizData>(json);
            var result = objectReadable?.quarters;
            return result ?? new List<Quarter>();
        }*/
    }
    /*public class QuizData
    {
        public List<Quarter> quarters;
        public List<QuizAnswer> quizAnswers;
        public List<QuizResult> quizResults;
        public List<Quiz> quizzes;
        public List<Question> questions;
        public List<Course> courses;
        public List<Department> departments;
    }*/
}
