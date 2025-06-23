using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.core.Features.Practices.Queries
{
    public class GetPracticeByIdQuery : IRequest<Practice?>
    {
        public Guid Id { get; set; }
        public class QueryHandler : IRequestHandler<GetPracticeByIdQuery, Practice?>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<Practice?> Handle(GetPracticeByIdQuery request, CancellationToken cancellationToken)
            {
                return await unitOfWork.PracticeRepository.FirstOrDefaultAsync(x => x.Id == request.Id,
                    includes: x => x.PracticeTags,
                    cancellationToken: cancellationToken);
            }
        }

    }
}
