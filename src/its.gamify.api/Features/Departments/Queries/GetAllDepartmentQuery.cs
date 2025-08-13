using its.gamify.core;
using its.gamify.core.Models.Departments;
using its.gamify.core.Models.ShareModels;
using MediatR;
using System.Linq.Expressions;

namespace its.gamify.api.Features.Departments.Queries
{
    public class GetAllDepartmentQuery : IRequest<BasePagingResponseModel<DepartmentViewModel>>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string Search { get; set; } = string.Empty;
        class QueryHandler : IRequestHandler<GetAllDepartmentQuery, BasePagingResponseModel<DepartmentViewModel>>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<BasePagingResponseModel<DepartmentViewModel>> Handle(GetAllDepartmentQuery request, CancellationToken cancellationToken)
            {

                Expression<Func<DepartmentViewModel, bool>>? filter = null;
                if (!string.IsNullOrEmpty(request.Search))
                {
                    filter = x => x.Name.Contains(request.Search);
                }
                var res = await unitOfWork.DepartmentRepository.ToPagination(request.PageIndex, request.PageSize);
                var models = unitOfWork.Mapper.Map<List<DepartmentViewModel>>(res.Entities);

                return new BasePagingResponseModel<DepartmentViewModel>(datas: models, pagination: res.Pagination);
            }

        }

    }
}
