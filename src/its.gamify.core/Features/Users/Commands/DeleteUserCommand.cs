using its.gamify.core;
using MediatR;

namespace its.gamify.api.Features.Users.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        class CommandHandler : IRequestHandler<DeleteUserCommand, bool>
        {
            private readonly IUnitOfWork unitOfWork;
            public CommandHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                var user = await unitOfWork.UserRepository.GetByIdAsync(request.Id);
                if (user is not null)
                {
                    unitOfWork.UserRepository.SoftRemove(user);
                    return await unitOfWork.SaveChangesAsync();
                }
                else throw new InvalidOperationException("User not found");
            }
        }

    }
}
