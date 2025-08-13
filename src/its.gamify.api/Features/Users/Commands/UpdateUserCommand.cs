using its.gamify.core;
using its.gamify.core.Models.Users;
using its.gamify.domains.Enums;
using MediatR;

namespace its.gamify.api.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public UserUpdateModel Model { get; set; } = new();
        class CommandHandler : IRequestHandler<UpdateUserCommand, bool>
        {
            private readonly IUnitOfWork unitOfWork;
            public CommandHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {

                var user = await unitOfWork.UserRepository.GetByIdAsync(request.Id, includes: [x => x.Role!]);
                var roles = await unitOfWork.RoleRepository.GetAllAsync();
                if (user is not null)
                {
                    unitOfWork.Mapper.Map(request.Model, user);
                    unitOfWork.UserRepository.Update(user);
                    if (user.Role?.Name == RoleEnum.LEADER.ToString())
                    {

                    }
                    return await unitOfWork.SaveChangesAsync();
                }
                else throw new InvalidOperationException("User not found");

            }
        }
    }
}
