using its.gamify.core.Models.Courses;

namespace its.gamify.core.Services.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseViewModel>> GetAll(int page, int limit, string q);
        Task<CourseViewModel> GetCourse(Guid id);
        Task<CourseViewModel> Create(CourseCreateModel item);
        Task<bool> Update(CourseUpdateModel item);
        Task<bool> Delete(Guid Id);
    }
}
