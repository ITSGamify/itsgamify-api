using its.gamify.core;
using its.gamify.core.Repositories;
using its.gamify.infras.Datas;

namespace its.gamify.infras
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICourseRepository _courseRepository;
        private readonly IDepartmentRepository _departmentRepository;
        public UnitOfWork(AppDbContext dbContext, ICourseRepository courseRepository, IDepartmentRepository departmentRepository)
        {
            _appDbContext = dbContext;
            _courseRepository = courseRepository;
            _departmentRepository = departmentRepository;
        }
        public ICourseRepository CourseRepository => _courseRepository;
        public IDepartmentRepository DepartmentRepository => _departmentRepository;
        public async Task<bool> SaveChangesAsync()
        => await _appDbContext.SaveChangesAsync() > 0;
    }
}
