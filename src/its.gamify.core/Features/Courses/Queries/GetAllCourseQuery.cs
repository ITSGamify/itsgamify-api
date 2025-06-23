using its.gamify.core;
using its.gamify.core.Features.AvailablesData;
using its.gamify.core.Models.Courses;
using its.gamify.core.Models.ShareModels;
using its.gamify.core.Models.Users;
using its.gamify.domains.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace its.gamify.api.Features.Users.Queries
{

    public class GetAllCourseQuery : IRequest<BasePagingResponseModel<CourseViewModel>>
    {
       
        class QueryHandler : IRequestHandler<GetAllCourseQuery, BasePagingResponseModel<CourseViewModel>>
        {
            private readonly IUnitOfWork unitOfWork;
            private Ultils data;
            public QueryHandler(IUnitOfWork unitOfWork, Ultils data)
            {
                this.unitOfWork = unitOfWork;
                this.data= data;
            }
            public async Task<BasePagingResponseModel<CourseViewModel>> Handle(GetAllCourseQuery request, CancellationToken cancellationToken)
            {
                var res = await unitOfWork.CourseRepository.ToPagination(includes: x => x.Category!);
                var resModel = unitOfWork.Mapper.Map<List<CourseViewModel>>(res.Item2);
                return new BasePagingResponseModel<CourseViewModel>(datas: resModel, pagination: res.Item1);
            }

        }
       
    }

}
