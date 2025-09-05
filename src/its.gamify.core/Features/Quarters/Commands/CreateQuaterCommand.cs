using FluentValidation;
using its.gamify.core;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.core.Models.Quarters;
using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.api.Features.Quarters.Commands
{
    public class CreateQuaterCommand : QuarterCreateModel, IRequest<Quarter>
    {
        class CommandValidation : AbstractValidator<CreateQuaterCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.EndDate).NotNull().NotEmpty().GreaterThan(x => x.StartDate).WithMessage("End date must be before start date.");
                RuleFor(x => x.EndDate).NotNull().NotEmpty().GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("End date must be before today.");
                RuleFor(x => x.Name).NotNull().NotEmpty();
            }
        }
        class CommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateQuaterCommand, Quarter>
        {

            public async Task AutoGenerateMetrics(Guid quarterId)
            {

                var users = await unitOfWork.UserRepository.GetAllAsync(withDeleted: true);

                // Tạo UserMetric cho mỗi người dùng
                foreach (var user in users)
                {
                    var userMetric = new UserMetric
                    {
                        UserId = user.Id,
                        QuarterId = quarterId
                    };

                    await unitOfWork.UserMetricRepository.AddAsync(userMetric);
                }

                // Lưu các thay đổi vào cơ sở dữ liệu
                await unitOfWork.SaveChangesAsync();
            }

            public async Task<Quarter> Handle(CreateQuaterCommand request, CancellationToken cancellationToken)
            {
                var quarter = unitOfWork.Mapper.Map<Quarter>(request);
                await unitOfWork.QuarterRepository.AddAsync(quarter, cancellationToken);
                await unitOfWork.SaveChangesAsync();
                await AutoGenerateMetrics(quarter.Id);
                return quarter;
            }
        }
    }
}
