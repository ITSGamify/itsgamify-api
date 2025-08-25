using MediatR;

namespace its.gamify.core.Features.Notifications.Commands;

public class ReadAllNotificationCommand : IRequest<bool>
{
    public required NotificationUpdateModel Model { get; set; }

    class CommandHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ReadAllNotificationCommand, bool>
    {

        public async Task<bool> Handle(ReadAllNotificationCommand request,
            CancellationToken cancellationToken)
        {
            var notifications = await _unitOfWork.NotificationRepository.GetAllAsync();

            if (notifications.Count == 0) return true;

            foreach (var notification in notifications)
            {
                notification.IsRead = request.Model.IsRead;
            }

            _unitOfWork.NotificationRepository.UpdateRange(notifications);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
