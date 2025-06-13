using its.gamify.core.Repositories;
using its.gamify.core.Services.Interfaces;
using its.gamify.domains.Entities;
using its.gamify.infras.Datas;

namespace its.gamify.infras.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            
        }
    }
}
