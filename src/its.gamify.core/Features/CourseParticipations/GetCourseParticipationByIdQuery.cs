using its.gamify.domains.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace its.gamify.core.Features.CourseParticipations.Queries
{
    public class GetCourseParticipationByIdQuery : IRequest<CourseParticipation>
    {
        public Guid Id { get; set; }
        public class QueryHandler : IRequestHandler<GetCourseParticipationByIdQuery, CourseParticipation>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<CourseParticipation> Handle(GetCourseParticipationByIdQuery request, CancellationToken cancellationToken)
            {
                return await unitOfWork.CourseParticipationRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            }
        }
    }
}
