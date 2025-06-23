using its.gamify.domains.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace its.gamify.core.Features.LearningProgresses.Queries
{
    public class GetLearningProgressByIdQuery : IRequest<LearningProgress>
    {
        public Guid Id { get; set; }
        public class QueryHandler : IRequestHandler<GetLearningProgressByIdQuery, LearningProgress>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<LearningProgress> Handle(GetLearningProgressByIdQuery request, CancellationToken cancellationToken)
            {
                return await unitOfWork.LearningProgressRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            }
        }
    }
}
