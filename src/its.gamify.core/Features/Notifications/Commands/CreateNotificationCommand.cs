using its.gamify.core.SingalR;
using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace its.gamify.core.Features.Notifications.Commands;

public class NotificationCreateModel
{
    public required string Type { get; set; }
    public required Guid UserId { get; set; }
}

public class CreateNotificationCommand : IRequest<Notification>
{
    public required NotificationCreateModel Model { get; set; }

    class CommandHandler(IUnitOfWork _unitOfWork, IHubContext<GameHub> _hubContext) : IRequestHandler<CreateNotificationCommand, Notification>
    {

        public async Task<Notification> Handle(CreateNotificationCommand request,
            CancellationToken cancellationToken)
        {
            var notification = await _unitOfWork.NotificationRepository.AddAsync(
                 new Notification
                 {
                     Title = NotificationType.GetTitleByType(request.Model.Type),
                     Message = NotificationType.GetContentByType(request.Model.Type),
                     Type = request.Model.Type,
                     UserId = request.Model.UserId,
                 }, cancellationToken);

            await _unitOfWork.SaveChangesAsync();

            var json = JsonConvert.SerializeObject(notification, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                {
                    NamingStrategy = new Newtonsoft.Json.Serialization.SnakeCaseNamingStrategy()
                }
            });


            await _hubContext.Clients
                .All
                .SendAsync("NotificationMessage", json, cancellationToken: cancellationToken);
            return notification;
        }
    }
}
