using FluentValidation;
using its.gamify.core;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.core.Models.Users;
using its.gamify.core.Services.Interfaces;
using its.gamify.core.Utilities;
using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using MediatR;

namespace its.gamify.api.Features.Users.Commands
{
    public class CreateUserCommand : IRequest<UserViewModel?>
    {
        public UserCreateModel Model { get; set; } = new();
        class CommandValidation : AbstractValidator<CreateUserCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.Model.Email).EmailAddress();
                RuleFor(x => x.Model.Password).NotNull().NotEmpty();
                RuleFor(x => x.Model.DepartmentId).NotNull().NotEmpty();
            }
        }
        class CommandHandler : IRequestHandler<CreateUserCommand, UserViewModel?>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IAuthService authService;
            public CommandHandler(IUnitOfWork unitOfWork,
                IAuthService authService)
            {
                this.authService = authService;
                this.unitOfWork = unitOfWork;

            }
            public async Task<UserViewModel?> Handle(CreateUserCommand request,
                CancellationToken cancellationToken)
            {
                bool checkDupEmail = (await unitOfWork.UserRepository.WhereAsync(x => x.Email.ToLower().Trim() == request.Model.Email.ToLower().Trim())) != null;
                if (checkDupEmail) throw new BadRequestException("Email đã được sử dụng!");
                var user = unitOfWork.Mapper.Map<User>(request.Model);
                var roles = await unitOfWork.RoleRepository.GetAllAsync();
                var leaderRole = roles.FirstOrDefault(x => x.Name == RoleEnum.LEADER.ToString());
                var dept = await unitOfWork.DepartmentRepository.FirstOrDefaultAsync(x => x.Id == user.DepartmentId,
                    false, cancellationToken, [x => x.Users!]);

                var leader = dept?.Users?.FirstOrDefault(x => x.RoleId == roles.First(x => x.Name == RoleEnum.LEADER.ToString()).Id);
                if (request.Model.RoleId == roles.First(x => x.Name == RoleEnum.LEADER.ToString()).Id)
                {
                    if (leader is not null)
                    {
                        throw new BadRequestException("Phòng ban đã có leader");
                    }
                }

                await unitOfWork.UserRepository.AddAsync(user, cancellationToken);
                if (!string.IsNullOrEmpty(request.Model.Password))

                    await authService.SignUpAsync(user.Email, request.Model.Password);

                var quarter = (await unitOfWork.QuarterRepository
                    .FirstOrDefaultAsync(q => q.StartDate <= DateTime.UtcNow && q.EndDate >= DateTime.UtcNow))
                    ?? throw new BadRequestException("No current quarter found");
                var userMetric = new UserMetric()
                {
                    UserId = user.Id,
                    QuarterId = quarter?.Id ?? Guid.Empty,
                };

                await unitOfWork.UserMetricRepository.AddAsync(userMetric, cancellationToken);
                await unitOfWork.SaveChangesAsync();
                return unitOfWork.Mapper.Map<UserViewModel>(user);

            }
        }
    }
}
