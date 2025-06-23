using its.gamify.domains.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace its.gamify.core.Features.Challenges.Queries
{
    public class GetChallengeByIdQuery : IRequest<Challenge>
    {
        public Guid Id { get; set; }
        public class QueryHandler : IRequestHandler<GetChallengeByIdQuery, Challenge>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<Challenge> Handle(GetChallengeByIdQuery request, CancellationToken cancellationToken)
            {
                return await unitOfWork.ChallengeRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            }
        }
    }
}
