using its.gamify.core;
using its.gamify.core.Models.ShareModels;
using its.gamify.core.Models.Users;
using MediatR;

namespace its.gamify.api.Features.Users.Queries
{
    public class GetAllUserQuery : IRequest<BasePagingResponseModel<UserViewModel>>
    {

        public FilterQuery FilterQuery { get; set; }

        class QueryHandler : IRequestHandler<GetAllUserQuery, BasePagingResponseModel<UserViewModel>>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<BasePagingResponseModel<UserViewModel>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
            {
                var res = await unitOfWork.UserRepository.ToDynamicPagination(
                    pageIndex: request.FilterQuery.Page ?? 0,
                    pageSize: request.FilterQuery.Limit ?? 0,
                    searchFields: ["FirstName", "LastName", "EmpployeeCode", "Email", "Username"],
                    searchTerm: request.FilterQuery.Q,
                    sortOrders: request.FilterQuery.OrderBy.ToDictionary(x => x.OrderColumn, x => x.OrderDir == "ASC"),
                    includes: [x => x.Role!, x => x.Department!]);
                var resModel = unitOfWork.Mapper.Map<List<UserViewModel>>(res.Item2);
                return new BasePagingResponseModel<UserViewModel>(datas: resModel, pagination: res.Item1);
            }
        }
    }
}
