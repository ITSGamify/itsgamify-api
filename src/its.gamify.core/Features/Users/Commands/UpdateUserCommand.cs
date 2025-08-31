using AutoMapper;
using its.gamify.core;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.core.Models.Users;
using its.gamify.domains.Enums;
using MediatR;

namespace its.gamify.api.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public UserUpdateModel Model { get; set; } = new();

        class CommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateUserCommand, bool>
        {
            public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                // 1. Lấy user về check (Nếu không tồn tại quăng lỗi)
                var user = await unitOfWork.UserRepository.GetByIdAsync(request.Id, includes: [x => x.Role!])
                    ?? throw new BadRequestException("Không tìm thấy người dùng!");
                //2. Kiểm tra xem email đã tồn tại chưa
                bool checkDupEmail = (await unitOfWork.UserRepository.WhereAsync(x => x.Email.ToLower().Trim() == request.Model.Email.ToLower().Trim() && x.Id != request.Id)) != null;
                if (checkDupEmail) throw new Exception("Email đã được sử dụng!");
                // 3. Check Role từ request vs user.RoleId vừa lấy về
                if (request.Model.RoleId != user.RoleId)
                {
                    var roles = await unitOfWork.RoleRepository.GetAllAsync();
                    var leaderRole = roles.First(x => x.Name == RoleEnum.LEADER.ToString());

                    // Check xem role đó có phải leader không
                    if (request.Model.RoleId == leaderRole.Id)
                    {
                        // Nếu đúng kiểm tra phòng ban đó có leader chưa => Có quăng lỗi
                        var dept = await unitOfWork.DepartmentRepository.FirstOrDefaultAsync(
                            x => x.Id == user.DepartmentId,
                            false,
                            cancellationToken,
                            [x => x.Users!]);

                        var existingLeader = dept?.Users?.FirstOrDefault(x => x.RoleId == leaderRole.Id);
                        if (existingLeader is not null)
                        {
                            throw new BadRequestException("Phòng ban đã có leader");
                        }
                    }
                }

                // 4. Map data từ request sang user
                mapper.Map(request.Model, user);
                user.Role = null;

                // 5. Update user
                unitOfWork.UserRepository.Update(user);
                return await unitOfWork.SaveChangesAsync();
            }
        }
    }
}