using its.gamify.domains.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace its.gamify.core.Features.PracticeTags.Queries
{
    public class GetPracticeTagByIdQuery : IRequest<PracticeTag>
    {
        public Guid Id { get; set; }
        public class QueryHandler : IRequestHandler<GetPracticeTagByIdQuery, PracticeTag>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<PracticeTag> Handle(GetPracticeTagByIdQuery request, CancellationToken cancellationToken)
            {
                return await unitOfWork.PracticeTagRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            }
        }
    }
}
