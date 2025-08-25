using its.gamify.core.Models.ShareModels;
using its.gamify.core.Services.Interfaces;
using its.gamify.domains.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace its.gamify.core.Features.Notifications.Queries;

public class GetAllNotificationQuery : IRequest<BasePagingResponseModel<Notification>>
{
    public required FilterQuery Filter { get; set; }

    class QueryHandler(IUnitOfWork unitOfWork, IClaimsService claimsService) : IRequestHandler<GetAllNotificationQuery, BasePagingResponseModel<Notification>>
    {

        public async Task<BasePagingResponseModel<Notification>> Handle(GetAllNotificationQuery request, CancellationToken cancellationToken)
        {

            var (Pagination, Entities) = await unitOfWork.NotificationRepository.ToDynamicPagination(
                pageIndex: request.Filter!.Page ?? 0,
                pageSize: request.Filter.Limit ?? 0,
                filter: x => x.UserId == claimsService.CurrentUser,
                searchFields: ["Title", "Message"],
                searchTerm: request.Filter.Q,
                sortOrders: request.Filter?.OrderBy?.ToDictionary(x => x.OrderColumn ?? string.Empty, x => x.OrderDir == "ASC") ?? [],
                includeFunc: x => x.Include(n => n.User));

            return new BasePagingResponseModel<Notification>(datas: Entities, pagination: Pagination);
        }

    }

}
