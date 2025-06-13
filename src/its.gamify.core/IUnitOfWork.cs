using its.gamify.core.Repositories;

namespace its.gamify.core
{
    public interface IUnitOfWork
    {
        public ICourseRepository CourseRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }
        Task<bool> SaveChangesAsync();
    }
}
