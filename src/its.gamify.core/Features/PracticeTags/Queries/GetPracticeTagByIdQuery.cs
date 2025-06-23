using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.core.Features.PracticeTags.Queries
{
    public class GetPracticeTagByIdQuery : IRequest<PracticeTag>
    {
        public Guid Id { get; set; } = Guid.Empty;
        class QueryHandler : IRequestHandler<GetPracticeTagByIdQuery, PracticeTag>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }

            public async Task<PracticeTag> Handle(GetPracticeTagByIdQuery request, CancellationToken cancellationToken)
            {
                return await unitOfWork.PracticeTagRepository.GetByIdAsync(request.Id, false, cancellationToken)
                    ?? throw new InvalidOperationException("Không tìm thấy practice tag với Id: " + request.Id);
            }
            }
        }
    }
