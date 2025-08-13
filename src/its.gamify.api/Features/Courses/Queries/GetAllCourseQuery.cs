using its.gamify.core;
using its.gamify.core.Features.AvailablesData;
using its.gamify.core.Models.ShareModels;
using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.api.Features.Users.Queries
{

    public class GetAllCourseQuery : IRequest<BasePagingResponseModel<Course>>
    {
        public FilterQuery filterQuery { get; set; }

        class QueryHandler : IRequestHandler<GetAllCourseQuery, BasePagingResponseModel<Course>>
        {
            private readonly IUnitOfWork unitOfWork;
            private Ultils data;
            public QueryHandler(IUnitOfWork unitOfWork, Ultils data)
            {
                this.unitOfWork = unitOfWork;
                this.data = data;
            }
            public async Task<BasePagingResponseModel<Course>> Handle(GetAllCourseQuery request, CancellationToken cancellationToken)
            {

                var searchTerm = request.filterQuery.Q;

                // Build sort dictionary from query
                Dictionary<string, bool> sortOrders = new();
                if (request.filterQuery.OrderBy != null)
                {
                    foreach (var order in request.filterQuery.OrderBy)
                    {
                        sortOrders[order.OrderColumn] = order.OrderDir.Equals("DESC", StringComparison.OrdinalIgnoreCase);
                    }
                }
                var res = await unitOfWork.CourseRepository.ToDynamicPagination(request.filterQuery.Page ?? 0, request.filterQuery.Limit ?? 0, searchTerm: searchTerm, searchFields: ["Title", "Description", "LongDescription"], includes: [x => x.Category!,
                 x => x.Quarter, x => x.CourseSections], sortOrders: sortOrders);

                return new BasePagingResponseModel<Course>(datas: res.Entities, pagination: res.Pagination);
            }

        }

    }

}
