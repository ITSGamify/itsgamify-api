using its.gamify.domains.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace its.gamify.core.Features.Lessons.Queries
{
    public class GetLessonByIdQuery : IRequest<Lesson>
    {
        public Guid Id { get; set; }
        public class QueryHandler : IRequestHandler<GetLessonByIdQuery, Lesson>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<Lesson> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
            {
                return await unitOfWork.LessonRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            }
        }
    }
}
