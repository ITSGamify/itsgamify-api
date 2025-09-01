using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.core.Models.ShareModels;
using its.gamify.core.Services.Interfaces;
using its.gamify.core.Utilities;
using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace its.gamify.core.Features.Challenges
{

    public class ChallengeQuery : FilterQuery
    {
        public string? Categories { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class GetChallengeQuery : IRequest<BasePagingResponseModel<Challenge>>
    {
        public ChallengeQuery? Filter { get; set; }

        public class QueryHandler(IUnitOfWork unitOfWork, IClaimsService claimsService) : IRequestHandler<GetChallengeQuery, BasePagingResponseModel<Challenge>>
        {

            public async Task<BasePagingResponseModel<Challenge>> Handle(GetChallengeQuery request, CancellationToken cancellationToken)
            {
                bool checkRole = claimsService.CurrentRole == ROLE.ADMIN || claimsService.CurrentRole == ROLE.TRAININGSTAFF ||
                    claimsService.CurrentRole == ROLE.MANAGER;
                Expression<Func<Challenge, bool>>? filter = x =>
                    !x.Course.IsDeleted && x.Course.Status == COURSE_STATUS.PUBLISHED && x.Course.IsDraft == false;
                Dictionary<string, bool>? sortOrders = request.Filter?.OrderBy?.ToDictionary(x => x.OrderColumn ?? string.Empty, x => x.OrderDir == "ASC");

                Func<IQueryable<Challenge>, IIncludableQueryable<Challenge, object>>? includeFunc =
                x =>
                    x.Include(x => x.Course).Include(x => x.Category!);

                if (!string.IsNullOrEmpty(request.Filter!.Categories))
                {
                    var categoryIds = JsonConvert.DeserializeObject<List<Guid>>(request.Filter!.Categories!);
                    if (categoryIds != null && categoryIds.Count != 0)
                    {
                        Expression<Func<Challenge, bool>> filter_cate = x => categoryIds != null && categoryIds.Count != 0 && categoryIds.Contains(x.CategoryId);
                        filter = filter != null ? FilterCustom.CombineFilters(filter, filter_cate) : filter_cate;
                    }
                }

                Expression<Func<Challenge, bool>> filterDeleted = x => x.IsDeleted == !request.Filter!.IsActive;
                filter = filter != null ? FilterCustom.CombineFilters(filter, filterDeleted) : filterDeleted;

                if (!checkRole)
                {
                    var courseIds = (await GetCourseByUserId()).Select(x => x.Id);
                    Expression<Func<Challenge, bool>> filterByCourse = x => courseIds.Contains(x.CourseId);
                    filter = filter != null ? FilterCustom.CombineFilters(filter, filterByCourse) : filterByCourse;
                }

                var (Pagination, Entities) = await unitOfWork
                                .ChallengeRepository
                                .ToDynamicPagination(request.Filter?.Page ?? 0,
                                    request.Filter?.Limit ?? 10,
                                    filter: filter,
                                    searchTerm: request.Filter?.Q,
                                    searchFields: ["Title", "Description"],
                                    sortOrders: sortOrders,
                                    includeFunc: x =>
                                        x.Include(x => x.Course).ThenInclude(x => x.CourseResults.Where(x => x.UserId == claimsService.CurrentUser))
                                        .Include(x => x.Rooms.Where(x => !x.IsDeleted))
                                        .Include(x => x.Category!),
                                    withDeleted: checkRole
                                );
                return new BasePagingResponseModel<Challenge>(Entities, Pagination);
            }


            private async Task<List<Course>> GetCourseByUserId()
            {
                var user = await unitOfWork.UserRepository.GetByIdAsync(claimsService.CurrentUser) ?? throw new BadRequestException("Người dùng không tồn tại!");


                Expression<Func<Course, bool>>? filter = null;

                if (claimsService.CurrentRole == ROLE.EMPLOYEE)
                {
                    filter = x => x.Status == COURSE_STATUS.PUBLISHED && x.IsDraft == false &&
                        (x.CourseType == COURSE_TYPE.ALL ||
                        (x.CourseType == COURSE_TYPE.DEPARTMENTONLY && x.CourseDepartments!.Any(cd => cd.DepartmentId == user.DepartmentId && cd.IsDeleted == false)));
                }

                else if (claimsService.CurrentRole == ROLE.LEADER)
                {
                    filter = x => x.Status == COURSE_STATUS.PUBLISHED && x.IsDraft == false &&
                                (x.CourseType == COURSE_TYPE.ALL || x.CourseType == COURSE_TYPE.LEADERONLY ||
                                (x.CourseType == COURSE_TYPE.DEPARTMENTONLY && x.CourseDepartments!.Any(cd => cd.DepartmentId == user.DepartmentId && cd.IsDeleted == false)));

                }

                return await unitOfWork.CourseRepository.WhereAsync(filter: filter!);

            }
        }
    }
}
