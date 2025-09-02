using its.gamify.core.Features.Badges.Commands;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.core.Services.Interfaces;
using its.gamify.core.Utilities;
using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using MediatR;

namespace its.gamify.core.Services;

public interface IQuarterService
{
    Task AutoGenerateQuarter();
    Task CreateCurrentQuarter();
    Task SumarizeFinalQuarter();
}


public class QuarterService(IUnitOfWork _unitOfWork, ICurrentTime _currentTime, IMediator mediator) : IQuarterService
{

    public async Task AutoGenerateQuarter()
    {
        var currentTime = _currentTime.GetCurrentTime;

        var currentQuarter = await _unitOfWork.QuarterRepository
                         .FirstOrDefaultAsync(q => q.StartDate <= currentTime && q.EndDate >= currentTime);

        // Nếu không tìm thấy quarter hiện tại, tạo quarter cho quý hiện tại
        if (currentQuarter == null)
        {
            await CreateCurrentQuarter();
            return;
        }

        // Tính toán thông tin cho quý kế tiếp
        var nextQuarterStartDate = currentQuarter.EndDate?.AddDays(3);
        var nextQuarterEndDate = nextQuarterStartDate?.AddMonths(3).AddDays(-1);

        if (!nextQuarterStartDate.HasValue || !nextQuarterEndDate.HasValue)
            throw new BadRequestException("Không thể tính toán ngày cho quý kế tiếp!");

        // Kiểm tra xem quý kế tiếp đã tồn tại chưa
        var existingNextQuarter = await _unitOfWork.QuarterRepository
                                 .FirstOrDefaultAsync(q => q.StartDate == nextQuarterStartDate);

        if (existingNextQuarter != null)
            throw new BadRequestException("Quý kế tiếp đã tồn tại!");

        // Xác định tên và năm cho quý kế tiếp
        var nextYear = nextQuarterStartDate.Value.Year;
        var quarterNumber = DateTimeUtilities.GetQuarterNumber(nextQuarterStartDate.Value.Month);
        var quarterName = $"Qúy {quarterNumber}";

        // Tạo quarter mới
        var newQuarter = new Quarter
        {
            Name = quarterName,
            Year = nextYear,
            StartDate = nextQuarterStartDate,
            EndDate = nextQuarterEndDate
        };

        await _unitOfWork.QuarterRepository.AddAsync(newQuarter);
        await _unitOfWork.SaveChangesAsync();
        await AutoGenerateMetrics(newQuarter.Id);
    }
    public async Task AutoGenerateMetrics(Guid quarterId)
    {
        // Kiểm tra quarter có tồn tại không
        var quarter = await _unitOfWork.QuarterRepository.GetByIdAsync(quarterId) ?? throw new NotFoundException($"Không tìm thấy Quarter với ID: {quarterId}");

        // Lấy danh sách tất cả người dùng
        var users = await _unitOfWork.UserRepository.GetAllAsync(withDeleted: true);

        // Tạo UserMetric cho mỗi người dùng
        foreach (var user in users)
        {
            var userMetric = new UserMetric
            {
                UserId = user.Id,
                QuarterId = quarter.Id
            };

            await _unitOfWork.UserMetricRepository.AddAsync(userMetric);
        }

        // Lưu các thay đổi vào cơ sở dữ liệu
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task CreateCurrentQuarter()
    {
        var currentTime = _currentTime.GetCurrentTime;
        var currentYear = currentTime.Year;
        var currentMonth = currentTime.Month;
        var quarterNumber = DateTimeUtilities.GetQuarterNumber(currentMonth);

        // Tính toán ngày bắt đầu và kết thúc của quý hiện tại
        var startMonth = (quarterNumber - 1) * 3 + 1;
        var startDate = new DateTime(currentYear, startMonth, 1).AddDays(2);
        var endDate = startDate.AddMonths(3).AddDays(-3);

        var quarterName = $"Qúy {quarterNumber}";

        // Kiểm tra xem quarter hiện tại đã tồn tại chưa (theo StartDate)
        var existingQuarter = await _unitOfWork.QuarterRepository
                             .FirstOrDefaultAsync(q => q.StartDate == startDate);

        if (existingQuarter != null)
            throw new BadRequestException("Quarter hiện tại đã tồn tại!");

        var currentQuarter = new Quarter
        {
            Name = quarterName,
            Year = currentYear,
            StartDate = startDate,
            EndDate = endDate
        };

        await _unitOfWork.QuarterRepository.AddAsync(currentQuarter);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SumarizeFinalQuarter()
    {
        var currentTime = _currentTime.GetCurrentTime;

        var currentQuarter = await _unitOfWork.QuarterRepository
                         .FirstOrDefaultAsync(q => q.StartDate <= currentTime && q.EndDate >= currentTime);

        if (currentQuarter == null)
        {
            return;
        }

        var tenMinutesBeforeEndOfQuarter = currentQuarter.EndDate!.Value.AddMinutes(-10);

        if (currentTime < tenMinutesBeforeEndOfQuarter)
        {
            return;
        }

        await mediator.Send(new CreateBadgeCommand()
        {
            Model = new CreateBadgeModel
            {
                Type = BadgeType.OUTSTANDING_ACHIEVEMENT,
                UserId = Guid.NewGuid()
            }
        });


        await mediator.Send(new CreateBadgeCommand()
        {
            Model = new CreateBadgeModel
            {
                Type = BadgeType.TOP_CHALLENGER,
                UserId = Guid.NewGuid()
            }
        });
    }

}