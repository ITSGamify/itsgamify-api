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
        }
        public ICourseRepository CourseRepository => _courseRepository;
        public IMapper Mapper => mapper;
        public IUserRepository UserRepository => _userRepository;
        public IDepartmentRepository DepartmentRepository => _departmentRepository;
        public IRoleRepository RoleRepository => roleRepository;
        public IPracticeRepository PracticeRepository => _practiceRepository;
        public IPracticeTagRepository PracticeTagRepository => practiceTagRepository;
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
