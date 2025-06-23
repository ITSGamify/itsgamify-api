using its.gamify.domains.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace its.gamify.core.Features.LearningMaterials.Queries
{
    public class GetLearningMaterialByIdQuery : IRequest<LearningMaterial>
    {
        public Guid Id { get; set; }
        public class QueryHandler : IRequestHandler<GetLearningMaterialByIdQuery, LearningMaterial>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<LearningMaterial> Handle(GetLearningMaterialByIdQuery request, CancellationToken cancellationToken)
            {
                return await unitOfWork.LearningMaterialRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            }
        }
    }
}
