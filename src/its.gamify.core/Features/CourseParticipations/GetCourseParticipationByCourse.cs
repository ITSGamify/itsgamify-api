using System.Linq.Expressions;
using its.gamify.core;
using its.gamify.core.Models.ShareModels;
using its.gamify.core.Services.Interfaces;
using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using MediatR;

namespace its.gamify.api.Features.CourseParticipations
{
    public class GetCourseParticipationByCourse : IRequest<BasePagingResponseModel<CourseParticipation>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public Guid CourseId { get; set; }
        class QueryHandler : IRequestHandler<GetCourseParticipationByCourse, BasePagingResponseModel<CourseParticipation>>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IClaimsService _claimService;
            public QueryHandler(IUnitOfWork unitOfWork, IClaimsService claimsService)
            {
                this.unitOfWork = unitOfWork;
                _claimService = claimsService;
            }
            public async Task<BasePagingResponseModel<CourseParticipation>> Handle(GetCourseParticipationByCourse request, CancellationToken cancellationToken)
            {

                Expression<Func<CourseParticipation, bool>> filter = x =>
                            x.CourseId == request.CourseId &&
                           (_claimService.CurrentRole != ROLE.EMPLOYEE || x.UserId == _claimService.CurrentUser);

                var (Pagination, Entities) = await unitOfWork.CourseParticipationRepository.ToPagination(
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    filter: filter,
                    cancellationToken: cancellationToken,
                    includes: [
                        x => x.Course,
                        x => x.User,
                        x => x.CourseResult!,
                        x=>x.CourseReview!,
                        x => x.LearningProgresses.Where(x=>!x.IsDeleted),
                        ]
                );
                return new BasePagingResponseModel<CourseParticipation>(Entities, Pagination);
            }
        }

    }
}
