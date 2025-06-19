using its.gamify.core;
using its.gamify.core.Models.ShareModels;
using its.gamify.core.Models.Users;
using MediatR;

namespace its.gamify.api.Features.Users.Queries
{
    public class GetAllUserQuery : IRequest<BasePagingResponseModel<UserViewModel>>
    {

        class QueryHandler : IRequestHandler<GetAllUserQuery, BasePagingResponseModel<UserViewModel>>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<BasePagingResponseModel<UserViewModel>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
            {
                var res = await unitOfWork.UserRepository.ToPagination(includes: x => x.Department!);
                var resModel = unitOfWork.Mapper.Map<List<UserViewModel>>(res.Item2);
                return new BasePagingResponseModel<UserViewModel>(datas: resModel, pagination: res.Item1);
            }
        }
    }
}
