using its.gamify.domains.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace its.gamify.core.Features.ChallengeParticipations.Queries
{
    public class GetChallengeParticipationByIdQuery : IRequest<ChallengeParticipation>
    {
        public Guid Id { get; set; }
        public class QueryHandler : IRequestHandler<GetChallengeParticipationByIdQuery, ChallengeParticipation>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<ChallengeParticipation> Handle(GetChallengeParticipationByIdQuery request, CancellationToken cancellationToken)
            {
                return await unitOfWork.ChallengeParticipationRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            }
        }
    }
}
