using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using MediatR;

namespace its.gamify.core.Features.Notifications.Commands;

public class NotificationUpdateModel
{
    public required bool IsRead { get; set; }
}

public class UpdateNotificationCommand : IRequest<Notification>
{
    public required Guid Id { get; set; }
    public required NotificationUpdateModel Model { get; set; }

    class CommandHandler(IUnitOfWork _unitOfWork) : IRequestHandler<UpdateNotificationCommand, Notification>
    {

        public async Task<Notification> Handle(UpdateNotificationCommand request,
            CancellationToken cancellationToken)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(request.Id)
                ?? throw new BadRequestException("Không tìm thấy thông báo!");
            notification.IsRead = request.Model.IsRead;

            _unitOfWork.NotificationRepository.Update(notification);
            await _unitOfWork.SaveChangesAsync();
            return notification;
        }
    }
}
