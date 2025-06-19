using its.gamify.core;
using its.gamify.core.Models.Users;
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

                var user = await unitOfWork.UserRepository.GetByIdAsync(request.Id);
                if (user is not null)
                {
                    unitOfWork.Mapper.Map(request.Model, user);
                    unitOfWork.UserRepository.Update(user);
                    return await unitOfWork.SaveChangesAsync();
                }
                else throw new InvalidOperationException("User not found");

            }
        }
    }
}
