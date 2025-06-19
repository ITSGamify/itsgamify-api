using its.gamify.core;
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
                return unitOfWork.Mapper.Map<UserViewModel>(await unitOfWork.UserRepository.GetByIdAsync(request.Id));
            }
        }
    }
}
