using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.core.Features.PracticeTags.Queries
{
    public class GetPracticeTagByIdQuery : IRequest<PracticeTag>
    {
        class QueryHandler : IRequestHandler<PracticeTag>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public Task Handle(PracticeTag request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
