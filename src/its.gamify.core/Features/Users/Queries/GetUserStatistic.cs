using System.Linq.Expressions;
using its.gamify.core;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.core.Models.ShareModels;
using its.gamify.core.Models.Users;
using its.gamify.core.Services.Interfaces;
using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using MediatR;

namespace its.gamify.api.Features.Users.Queries
{
    public class UserStatisticViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int TotalProgress { get; set; }
        public string AvatarInitials { get; set; } = string.Empty;
        public int Completed { get; set; }
        public int Overdue { get; set; }
        public List<CourseStatisticViewModel> Courses { get; set; } = [];
    }

    public class CourseStatisticViewModel
    {
        public string Name { get; set; } = string.Empty;
        public int Progress { get; set; }
        public string Deadline { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Remaining { get; set; } = string.Empty;
    }

    public class GetUserStatistic : IRequest<UserStatisticViewModel>
    {
        public required Guid QuarterId { get; set; }
        public required Guid UserId { get; set; }

        class QueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserStatistic, UserStatisticViewModel>
        {
            public async Task<UserStatisticViewModel> Handle(GetUserStatistic request, CancellationToken cancellationToken)
            {
                var quarter = await unitOfWork.QuarterRepository.GetByIdAsync(request.QuarterId) ?? throw new BadRequestException("Không tìm thấy quý!");

                var user = await unitOfWork
                    .UserRepository
                    .GetByIdAsync(request.UserId, includes: [x => x.Department!, x => x.Role!]) ?? throw new BadRequestException("Không tìm thấy người dùng!");

                Expression<Func<Course, bool>>? filter = null;


                if (user.Role!.Name == ROLE.EMPLOYEE)
                {
                    filter = x => x.Status == COURSE_STATUS.PUBLISHED &&
                        x.IsDraft == false &&
                        (x.CourseType == COURSE_TYPE.ALL ||
                            (x.CourseType == COURSE_TYPE.DEPARTMENTONLY
                             && x.CourseDepartments.Any(x => x.DepartmentId == user.DepartmentId && !x.IsDeleted)
                             && x.Status == COURSE_STATUS.PUBLISHED
                             && x.IsDraft == false
                             && x.QuarterId == request.QuarterId));

                }

                else if (user.Role!.Name == ROLE.LEADER)
                {
                    filter = x => x.Status == COURSE_STATUS.PUBLISHED &&
                                x.IsDraft == false && x.QuarterId == request.QuarterId
                                || (x.CourseDepartments.Any(x => x.DepartmentId == user.DepartmentId && !x.IsDeleted)
                                    && x.CourseType == CourseTypeEnum.DEPARTMENTONLY.ToString()
                                    && x.Status == COURSE_STATUS.PUBLISHED &&
                                    x.IsDraft == false);
                }
                // Lấy khóa học của phòng ban của người dùng
                var departmentCourses = await unitOfWork.CourseRepository.WhereAsync(filter!, includes: [x => x.Quarter]);

                // Lấy thông tin tham gia khóa học của người dùng
                var courseParticipations = await unitOfWork.CourseParticipationRepository
                    .WhereAsync(x => x.UserId == user.Id &&
                                    x.CreatedDate >= quarter.StartDate &&
                                    x.CreatedDate <= quarter.EndDate,
                                includes: [x => x.Course, x => x.LearningProgresses, x => x.CourseResult!]);

                // Tính toán thống kê
                int completedCourses = courseParticipations.Count(cp =>
                    cp.Status == CourseParticipationStatusEnum.COMPLETED.ToString());

                int overdueCourses = courseParticipations.Count(cp =>
                    cp.Status != CourseParticipationStatusEnum.COMPLETED.ToString() &&
                    cp.Deadline < DateTime.Now);

                // Tính tổng tiến độ
                int totalProgress = 0;
                // if (courseParticipations.Count != 0)
                // {
                //     totalProgress = (int)courseParticipations.Average(cp =>
                //     {
                //         if (cp.Status == CourseParticipationStatusEnum.COMPLETED.ToString())
                //             return 100;

                //         if (cp.LearningProgresses.Count != 0)
                //         {
                //             int completedCount = cp.LearningProgresses.Count(lp => lp.Status == PROGRESS_STATUS.COMPLETED);
                //             int totalCount = cp.LearningProgresses.Count;
                //             return totalCount > 0 ? (int)((completedCount * 100.0) / totalCount) : 0;
                //         }

                //         return 0;
                //     });
                // }

                // Tạo danh sách khóa học với thông tin chi tiết
                int completed = 0;
                int in_completed = 0;
                var courseDetails = new List<CourseStatisticViewModel>();
                foreach (var cp in courseParticipations)
                {
                    string status = "Chưa bắt đầu";
                    int progress = 0;

                    if (cp.Status == CourseParticipationStatusEnum.COMPLETED.ToString())
                    {
                        status = "Hoàn thành";
                        progress = 100;
                        completed += 1;
                    }
                    else if (cp.LearningProgresses.Count > 0)
                    {
                        status = "Đang học";
                        int completedCount = cp.LearningProgresses.Count(lp => lp.Status == PROGRESS_STATUS.COMPLETED);
                        int totalCount = cp.LearningProgresses.Count;

                        var modules = await unitOfWork
                            .CourseSectionRepository
                            .WhereAsync(x => x.CourseId == cp.CourseId, includes: x => x.Lessons.Where(x => !x.IsDeleted));
                        int totalLessons = modules.Sum(module => module.Lessons.Count);

                        // Calculate progress based on completed lessons relative to total lessons
                        progress = totalLessons > 0 ? (int)((completedCount * 100.0) / totalLessons) : 0;
                        in_completed += 1;
                    }

                    // Tính số ngày còn lại
                    string remaining = "Hôm nay";
                    if (cp.Deadline > DateTime.Now)
                    {
                        int daysRemaining = (cp.Deadline - DateTime.Now).Days;
                        remaining = daysRemaining <= 0 ? "Hôm nay" : $"{daysRemaining} ngày";
                    }
                    else if (cp.Status != CourseParticipationStatusEnum.COMPLETED.ToString())
                    {
                        remaining = "Quá hạn";
                    }

                    courseDetails.Add(new CourseStatisticViewModel
                    {
                        Name = cp.Course.Title,
                        Progress = progress,
                        Deadline = cp.Deadline.ToString("yyyy-MM-dd"),
                        Status = status,
                        Remaining = remaining
                    });
                }

                // Thêm các khóa học của phòng ban mà người dùng chưa tham gia
                foreach (var course in departmentCourses.Where(c => !courseParticipations.Any(cp => cp.CourseId == c.Id)))
                {
                    // Tính deadline mặc định (ví dụ: 30 ngày từ hiện tại)
                    DateTime defaultDeadline = course.Quarter.EndDate!.Value;
                    int daysRemaining = (defaultDeadline - DateTime.Now).Days;

                    courseDetails.Add(new CourseStatisticViewModel
                    {
                        Name = course.Title,
                        Progress = 0,
                        Deadline = defaultDeadline.ToString("yyyy-MM-dd"),
                        Status = "Chưa bắt đầu",
                        Remaining = $"{daysRemaining} ngày"
                    });
                    in_completed += 1;
                }

                // Tạo initials từ tên người dùng
                string initials = string.Join("", user.FullName.Split(' ')
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Select(s => s[0]));
                if (initials.Length > 2)
                    initials = initials.Substring(0, 2);
                totalProgress = in_completed + completed == 0 ? 0 : (int)((completed * 100.0) / (in_completed + completed));
                return new UserStatisticViewModel
                {
                    Name = user.FullName,
                    Role = user.Role!.Name,
                    TotalProgress = totalProgress,
                    AvatarInitials = initials,
                    Completed = completedCourses,
                    Overdue = overdueCourses,
                    Courses = courseDetails
                };
            }
        }
    }
}
