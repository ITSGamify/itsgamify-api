using its.gamify.core;
using its.gamify.core.Models.Departments;
using its.gamify.core.Models.Users;
using MediatR;

namespace its.gamify.api.Features.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserViewModel>
    {
        public Guid Id { get; set; }
        class QueryHandler : IRequestHandler<GetUserByIdQuery, UserViewModel>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                UserViewModel user = new UserViewModel();
                user = unitOfWork.Mapper.Map<UserViewModel>(await unitOfWork.UserRepository.GetByIdAsync(request.Id, includes: [x => x.Department!, x => x.Role!]));
                user.Department = unitOfWork.Mapper.Map<DepartmentViewModel>(await unitOfWork.DepartmentRepository.GetByIdAsync(user.DepartmentId));
                user.MetricDescription = (await unitOfWork.EmployeeMetricRepository.FirstOrDefaultAsync(x => x.UserId == request.Id))?.Description!;
                return user;
            }
        }
    }
}
