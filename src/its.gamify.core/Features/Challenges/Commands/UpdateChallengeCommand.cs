using FluentValidation;
using its.gamify.core.Features.Questions.Commands;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.core.Models.Challenges;
using its.gamify.domains.Entities;
using MediatR;


namespace its.gamify.core.Features.Challenges.Commands
{
    public class UpdateChallengeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public ChallengeUpdateModel Model { get; set; } = new();
        class CommandValidate : AbstractValidator<UpdateChallengeCommand>
        {
            public CommandValidate()
            {
                RuleFor(x => x.Model.Title).NotEmpty().NotNull().WithMessage("Vui lòng nhập tên cho thử thách"); ;
            }
        }
        class CommandHandler(IUnitOfWork unitOfWork, IMediator mediator) : IRequestHandler<UpdateChallengeCommand, bool>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;

            public async Task<bool> Handle(UpdateChallengeCommand request, CancellationToken cancellationToken)
            {
                var challenge = await unitOfWork.ChallengeRepository.GetByIdAsync(request.Id) ?? throw new BadRequestException("Không tìm thấy thử thách!");
                bool checkDupName = (await unitOfWork.ChallengeRepository.WhereAsync(x => x.Title.ToLower().Trim() == request.Model.Title.ToLower().Trim() && x.Id != request.Id)) != null;
                if (checkDupName) throw new BadRequestException("Tên thử thách đã tồn tại!");
                var mapper = unitOfWork.Mapper.Map(request.Model, new Challenge());
                if (challenge == mapper) return true;
                unitOfWork.Mapper.Map(request.Model, challenge);
                unitOfWork.ChallengeRepository.Update(challenge);

                if (request.Model.UpdatedQuestions.Count > 0)
                {
                    await mediator.Send(new UpdateQuestionCommand()
                    {
                        Models = request.Model.UpdatedQuestions
                    }, cancellationToken);
                }

                if (request.Model.NewQuestions.Count > 0)
                {
                    await mediator.Send(new CreateQuestionCommand()
                    {
                        Models = request.Model.NewQuestions
                    }, cancellationToken);
                }

                return await unitOfWork.SaveChangesAsync();

            }
        }
    }
}
