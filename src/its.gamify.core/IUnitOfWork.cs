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
        Task<bool> SaveChangesAsync();
        Task SeedData();
    }
}
