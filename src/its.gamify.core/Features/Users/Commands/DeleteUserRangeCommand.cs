using its.gamify.core;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.domains.Enums;
using MediatR;

namespace its.gamify.api.Features.Users.Commands
{
    public class DeleteUserRangeCommand : IRequest<bool>
    {
        public List<Guid> Ids { get; set; } = [];

        class CommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserRangeCommand, bool>
        {
            public async Task<bool> Handle(DeleteUserRangeCommand request, CancellationToken cancellationToken)
            {
                var users = await unitOfWork.UserRepository.WhereAsync(p => request.Ids.Contains(p.Id), includes: x => x.Role!);
                if (users.Count == 0) throw new InvalidOperationException("Không tìm thấy tài khoản");

                if (users.Any(x => x.Role!.Name == ROLE.ADMIN)) throw new BadRequestException("Không được xóa tài khoản của quản trị viên");
                if (users.Any(x => x.Role!.Name == ROLE.MANAGER)) throw new BadRequestException("Không được xóa tài khoản của nhân viên đào tạo");
                if (users.Any(x => x.Role!.Name == ROLE.TRAININGSTAFF)) throw new BadRequestException("Không được xóa tài khoản của quản lý");

                unitOfWork.UserRepository.SoftRemoveRange(users);
                return await unitOfWork.SaveChangesAsync();

            }
        }

    }
}
