using FluentValidation;
using its.gamify.core.Features.Questions.Commands;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.core.Models.Challenges;
using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.core.Features.Challenges.Commands
{
    public class CreateChallengeCommand : ChallengeCreateModel, IRequest<Challenge>
    {
        class CommandValidation : AbstractValidator<CreateChallengeCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CourseId).NotNull().NotEmpty().WithMessage("Vui lòng nhập course id");
                RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("Vui lòng nhập tên cho thử thách");
                RuleFor(x => x.NumOfRoom).GreaterThan(0).WithMessage("Số lượng phòng trong thử thách phải lớn hơn 0");
            }
        }
        class CommandHandler(IUnitOfWork unitOfWork, IMediator mediator) : IRequestHandler<CreateChallengeCommand, Challenge>
        {
            public async Task<Challenge> Handle(CreateChallengeCommand request, CancellationToken cancellationToken)
            {
                await unitOfWork.CourseRepository.EnsureExistsIfIdNotEmpty(request.CourseId);
                bool checkDupName = (await unitOfWork.ChallengeRepository.WhereAsync(x => x.Title.ToLower().Trim() == request.Title.ToLower().Trim())) != null;
                if (checkDupName) throw new BadRequestException("Tên thử thách đã tồn tại!");
                var challenge = unitOfWork.Mapper.Map<Challenge>(request);
                await unitOfWork.ChallengeRepository.AddAsync(challenge, cancellationToken);
                await unitOfWork.SaveChangesAsync();

                if (request.UpdatedQuestions.Count > 0)
                {
                    await mediator.Send(new UpdateQuestionCommand()
                    {
                        Models = request.UpdatedQuestions
                    }, cancellationToken);
                }

                if (request.NewQuestions.Count > 0)
                {
                    await mediator.Send(new CreateQuestionCommand()
                    {
                        Models = request.NewQuestions
                    }, cancellationToken);
                }

                return challenge;
            }
        }
    }
}
