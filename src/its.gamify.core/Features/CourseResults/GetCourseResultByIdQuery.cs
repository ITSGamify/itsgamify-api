using its.gamify.domains.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace its.gamify.core.Features.CourseResults.Queries
{
    public class GetCourseResultByIdQuery : IRequest<CourseResult>
    {
        public Guid Id { get; set; }
        public class QueryHandler : IRequestHandler<GetCourseResultByIdQuery, CourseResult>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<CourseResult> Handle(GetCourseResultByIdQuery request, CancellationToken cancellationToken)
            {
                return await unitOfWork.CourseResultRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            }
        }
    }
}
