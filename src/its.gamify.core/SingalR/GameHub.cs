using Microsoft.AspNetCore.SignalR;
using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using Newtonsoft.Json;
using its.gamify.core.Services.Interfaces;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using MediatR;
using its.gamify.core.Features.Badges.Commands;

namespace its.gamify.core.SingalR;

public class GameHub(IUnitOfWork _unitOfWork, ICurrentTime currentTime, IMediator mediator) : Hub
{
    private static readonly Dictionary<string, HashSet<string>> _roomConnections = [];
    private static readonly Dictionary<string, string> _connectionToUser = [];
    private static readonly Dictionary<string, List<Question>> _roomQuestions = [];


    public async Task JoinRoom(Guid roomId, Guid userId)
    {

        // Tìm room bằng roomId
        var room = await _unitOfWork.RoomRepository
            .FirstOrDefaultAsync(r => r.Id == roomId && !r.IsDeleted, includes: x => x.Challenge);

        if (room == null)
        {
            await Clients.Caller.SendAsync("Error", "Room không tồn tại hoặc đã bị xóa.");
            return;
        }

        // Kiểm tra user đã out room chưa
        var existingRoomUser = await _unitOfWork.RoomUserRepository
            .FirstOrDefaultAsync(ru => ru.RoomId == room.Id && ru.UserId == userId && !ru.IsOutRoom);

        if (existingRoomUser == null)
        {
            await Clients.Caller.SendAsync("Error", "Bạn không có quyền truy cập phòng này.");
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, $"room_{room.Id}");

        // Track connection
        var roomIdStr = room.Id.ToString();
        if (!_roomConnections.ContainsKey(roomIdStr))
            _roomConnections[roomIdStr] = [];
        _roomConnections[roomIdStr].Add(Context.ConnectionId);
        _connectionToUser[Context.ConnectionId] = userId.ToString();

        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

        // Cập nhật RoomUser
        existingRoomUser.IsOutRoom = false;
        existingRoomUser.CurrentScore = 0;
        existingRoomUser.CorrectAnswers = 0;
        existingRoomUser.IsCurrentQuestionAnswered = false;
        _unitOfWork.RoomUserRepository.Update(existingRoomUser);

        // Load questions cho room nếu chưa có
        if (!_roomQuestions.ContainsKey(roomIdStr))
        {
            var questions = await _unitOfWork.QuestionRepository
                .WhereAsync(q => q.CourseId == room.Challenge.CourseId && !q.IsHidden);

            _roomQuestions[roomIdStr] = [.. questions
                .OrderBy(x => Guid.NewGuid()) // Random order
                .Take(room.QuestionCount)];
        }

        await _unitOfWork.SaveChangesAsync();

        await Clients.Group($"room_{room.Id}").SendAsync("Notify", $"Người chơi {user!.FullName} đã tham gia!");

        string jsonRoom = await GetRoomJsonAsync(room.Id);
        await Clients.Group($"room_{room.Id}").SendAsync("RoomUpdated", jsonRoom);
    }
    public async Task<List<Question>> InitialMatch(Guid roomId)
    {
        var room = await _unitOfWork.RoomRepository
            .FirstOrDefaultAsync(r => r.Id == roomId && !r.IsDeleted, includes: x => x.Challenge) ?? throw new NotFoundException("Room không tồn tại hoặc đã bị xóa.");

        var roomIdStr = roomId.ToString();

        if (_roomQuestions.TryGetValue(roomIdStr, out List<Question>? value))
        {
            return value;
        }

        // Nếu chưa có, tạo danh sách câu hỏi mới - Back up nếu chưa có questions
        var questions = await _unitOfWork.QuestionRepository
            .WhereAsync(q => q.CourseId == room.Challenge.CourseId && !q.IsHidden);

        var selectedQuestions = questions
            .OrderBy(x => Guid.NewGuid()) // Random order
            .Take(room.QuestionCount)
            .ToList();

        // Lưu danh sách câu hỏi vào bộ nhớ
        _roomQuestions[roomIdStr] = selectedQuestions;

        // Reset room và players
        room.Status = ROOM_STATUS.WAITING;
        room.CurrentQuestionIndex = 0;
        _unitOfWork.RoomRepository.Update(room);

        // Reset tất cả players
        var roomUsers = await _unitOfWork.RoomUserRepository
            .WhereAsync(ru => ru.RoomId == roomId && !ru.IsOutRoom);

        foreach (var roomUser in roomUsers)
        {
            roomUser.CurrentScore = 0;
            roomUser.CorrectAnswers = 0;
            roomUser.IsCurrentQuestionAnswered = false;
            _unitOfWork.RoomUserRepository.Update(roomUser);
        }

        await _unitOfWork.SaveChangesAsync();

        return selectedQuestions;
    }
    public async Task StartMatch(Guid roomId)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
        if (room == null)
        {
            await Clients.Caller.SendAsync("Error", "Room không tồn tại.");
            return;
        }

        // Kiểm tra có đủ người chơi không
        var players = await _unitOfWork.RoomUserRepository
            .WhereAsync(ru => ru.RoomId == roomId && !ru.IsOutRoom);

        var playersCount = players.Count;
        if (playersCount < 2)
        {
            await Clients.Caller.SendAsync("Error", "Cần ít nhất 2 người chơi để bắt đầu.");
            return;
        }

        // Reset room và players
        room.Status = ROOM_STATUS.PLAYING;
        room.CurrentQuestionIndex = 0;

        // Reset tất cả players
        var roomUsers = await _unitOfWork.RoomUserRepository
            .WhereAsync(ru => ru.RoomId == roomId && !ru.IsOutRoom);

        foreach (var roomUser in roomUsers)
        {
            roomUser.CurrentScore = 0;
            roomUser.CorrectAnswers = 0;
            roomUser.IsCurrentQuestionAnswered = false;
            _unitOfWork.RoomUserRepository.Update(roomUser);
        }

        _unitOfWork.RoomRepository.Update(room);
        await _unitOfWork.SaveChangesAsync();

        string jsonRoom = await GetRoomJsonAsync(roomId);
        await Clients.Group($"room_{roomId}").SendAsync("RoomUpdated", jsonRoom);
    }
    public async Task SubmitAnswer(Guid roomId, Guid userId, int currentQuestion, int points)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
        if (room == null || room.Status != ROOM_STATUS.PLAYING)
        {
            await Clients.Caller.SendAsync("Error", "Game không hợp lệ.");
            return;
        }

        var roomUser = await _unitOfWork.RoomUserRepository
            .FirstOrDefaultAsync(ru => ru.RoomId == roomId && ru.UserId == userId);

        if (roomUser == null || roomUser.IsCurrentQuestionAnswered)
        {
            await Clients.Caller.SendAsync("Error", "Không thể trả lời.");
            return;
        }

        // Lấy câu hỏi hiện tại
        var roomIdStr = roomId.ToString();
        if (!_roomQuestions.ContainsKey(roomIdStr) ||
            room.CurrentQuestionIndex >= _roomQuestions[roomIdStr].Count)
        {
            await Clients.Caller.SendAsync("Error", "Câu hỏi không hợp lệ.");
            return;
        }


        // Cập nhật điểm
        roomUser.IsCurrentQuestionAnswered = true;

        roomUser.CurrentScore += points; // Điểm cố định hoặc tính theo thời gian
        roomUser.CorrectAnswers++;

        // Kiểm tra tất cả đã trả lời chưa
        await CheckAllPlayersAnswered(roomId, roomUser);
    }

    private async Task CheckAllPlayersAnswered(Guid roomId, RoomUser roomUser)
    {
        // Lấy tất cả players đang active trong room
        var activePlayers = await _unitOfWork.RoomUserRepository
            .WhereAsync(ru => ru.RoomId == roomId && !ru.IsOutRoom);
        foreach (var ru in activePlayers)
        {
            if (roomUser.Id == ru.Id)
            {
                ru.IsCurrentQuestionAnswered = roomUser.IsCurrentQuestionAnswered;
                ru.CurrentScore = roomUser.CurrentScore;
                ru.CorrectAnswers = roomUser.CorrectAnswers;
            }
        }

        // Kiểm tra tất cả đã trả lời chưa
        var allAnswered = activePlayers.All(ru => ru.IsCurrentQuestionAnswered);

        if (allAnswered && activePlayers.Count != 0)
        {
            await MoveToNextQuestion(roomId, activePlayers);
        }
        else
        {
            _unitOfWork.RoomUserRepository.Update(roomUser);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task MoveToNextQuestion(Guid roomId, List<RoomUser> activePlayers)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
        if (room == null) return;

        room.CurrentQuestionIndex++;

        var roomIdStr = roomId.ToString();
        bool isLastQuestion = !_roomQuestions.ContainsKey(roomIdStr) ||
                             room.CurrentQuestionIndex >= _roomQuestions[roomIdStr].Count;

        if (isLastQuestion)
        {
            await EndMatch(roomId);
            return;
        }

        // Reset trạng thái trả lời cho câu tiếp theo
        foreach (var ru in activePlayers)
        {
            ru.IsCurrentQuestionAnswered = false;
        }

        _unitOfWork.RoomUserRepository.UpdateRange(activePlayers);
        _unitOfWork.RoomRepository.Update(room);
        await _unitOfWork.SaveChangesAsync();
        string jsonRoom = await GetRoomJsonAsync(roomId);
        await Clients.Group($"room_{roomId}").SendAsync("RoomUpdated", jsonRoom);
    }

    private async Task EndMatch(Guid roomId)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
        if (room == null) return;

        room.Status = ROOM_STATUS.FINISHED;
        _unitOfWork.RoomRepository.Update(room);

        // Lưu lịch sử game
        var roomUsers = await _unitOfWork.RoomUserRepository
            .WhereAsync(ru => ru.RoomId == roomId && !ru.IsOutRoom,
                        includes: [ru => ru.User]);

        var winner = roomUsers.OrderByDescending(ru => ru.CurrentScore).First();

        var rankedUsers = roomUsers
            .OrderByDescending(ru => ru.CurrentScore)
            .ThenByDescending(ru => ru.CorrectAnswers)
            .Select((ru, index) => new { User = ru, Rank = index + 1 })
            .ToList();

        foreach (var rankedUser in rankedUsers)
        {
            var roomUser = rankedUser.User;

            bool isWinner = roomUser.UserId == winner.UserId;
            int pointsToAdd;
            if (isWinner)
            {
                pointsToAdd = room.BetPoints * (rankedUsers.Count - 1);
            }
            else
            {
                pointsToAdd = -room.BetPoints;
            }
            await _unitOfWork.UserChallengeHistoryRepository.AddAsync(new UserChallengeHistory
            {
                UserId = roomUser.UserId,
                WinnerId = winner.UserId,
                ChallengeId = room.ChallengeId,
                YourScore = roomUser.CurrentScore,
                WinnerScore = winner.CurrentScore,
                Points = pointsToAdd,
                Rank = rankedUser.Rank,
                AverageCorrect = roomUser.CorrectAnswers / (double)_roomQuestions[roomId.ToString()].Count,
                Status = isWinner ?
                    UserChallengeHistoryEnum.WIN : UserChallengeHistoryEnum.LOSE
            });

            await UpdateUserMetric(roomUser.UserId, roomUser.UserId == winner.UserId, room.BetPoints, rankedUsers.Count);

            await mediator.Send(new CreateBadgeCommand()
            {
                Model = new CreateBadgeModel { Type = BadgeType.COMBO_MASTER, UserId = roomUser.UserId }
            });
            await mediator.Send(new CreateBadgeCommand()
            {
                Model = new CreateBadgeModel { Type = BadgeType.INVINCIBLE, UserId = roomUser.UserId }
            });
            await mediator.Send(new CreateBadgeCommand()
            {
                Model = new CreateBadgeModel { Type = BadgeType.FIRST_VICTORY, UserId = roomUser.UserId }
            });

        }

        await _unitOfWork.SaveChangesAsync();

        // Gửi kết quả
        var results = roomUsers.Select(ru => new
        {
            userId = ru.UserId,
            userName = ru.User.FullName,
            score = ru.CurrentScore,
            correctAnswers = ru.CorrectAnswers,
            isWinner = ru.UserId == winner.UserId
        }).OrderByDescending(r => r.score);

        await Clients.Group($"room_{roomId}").SendAsync("GameEnded", results);

        string jsonRoom = await GetRoomJsonAsync(roomId);
        await Clients.Group($"room_{roomId}").SendAsync("RoomUpdated", jsonRoom);


        foreach (var rankedUser in rankedUsers)
        {
            var roomUser = rankedUser.User;
            await mediator.Send(new CreateBadgeCommand()
            {
                Model = new CreateBadgeModel { Type = BadgeType.COMBO_MASTER, UserId = roomUser.UserId }
            });
            await mediator.Send(new CreateBadgeCommand()
            {
                Model = new CreateBadgeModel { Type = BadgeType.INVINCIBLE, UserId = roomUser.UserId }
            });
            await mediator.Send(new CreateBadgeCommand()
            {
                Model = new CreateBadgeModel { Type = BadgeType.FIRST_VICTORY, UserId = roomUser.UserId }
            });

        }

    }

    public async Task OutRoom(Guid roomId, Guid userId)
    {
        bool IsDeleted = false;
        var roomUser = await _unitOfWork.RoomUserRepository
            .FirstOrDefaultAsync(ru => ru.RoomId == roomId && ru.UserId == userId);

        if (roomUser != null)
        {
            roomUser.IsOutRoom = true;
            _unitOfWork.RoomUserRepository.Update(roomUser);

            // Nếu là host thì chuyển host cho người khác
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
            if (room?.HostUserId == userId)
            {
                var newHost = await _unitOfWork.RoomUserRepository
                    .FirstOrDefaultAsync(ru => ru.RoomId == roomId && !ru.IsOutRoom && ru.UserId != userId);

                if (newHost != null)
                {
                    room.HostUserId = newHost.UserId;
                    _unitOfWork.RoomRepository.Update(room);
                }
                else
                {
                    IsDeleted = true;
                    // Không còn ai thì xóa room
                    _unitOfWork.RoomRepository.SoftRemove(room);
                    _roomQuestions.Remove(roomId.ToString());
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"room_{roomId}");

        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        await Clients.Group($"room_{roomId}").SendAsync("Notify", $"Người chơi {user!.FullName} đã rời khỏi phòng!");
        if (IsDeleted) return;
        string jsonRoom = await GetRoomJsonAsync(roomId);
        await Clients.Group($"room_{roomId}").SendAsync("RoomUpdated", jsonRoom);
    }

    private async Task UpdateUserMetric(Guid userId, bool isWinner, int points, int totalPlayers = 0)
    {
        var quarter = await _unitOfWork.QuarterRepository
               .FirstOrDefaultAsync(q =>
                    q.StartDate <= currentTime.GetCurrentTime &&
                    q.EndDate >= currentTime.GetCurrentTime
                ) ?? throw new BadRequestException("Không tìm thấy quý!");

        var metric = await _unitOfWork.UserMetricRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.QuarterId == quarter.Id)
            ?? throw new NotFoundException("Không tìm thấy chỉ số người dùng!");

        metric.ChallengeParticipateNum += 1;
        if (isWinner)
        {
            int pointsToAdd = points * (totalPlayers - 1);
            metric.PointInQuarter += pointsToAdd;
        }
        else
        {
            metric.PointInQuarter -= points;
        }
        metric.WinNum += isWinner ? 1 : 0;
        metric.LoseNum += !isWinner ? 1 : 0;
        if (!isWinner)
        {
            metric.HighestWinStreak = metric.WinStreak;
        }
        metric.WinStreak = isWinner ? (metric.WinStreak + 1) : 0;

        _unitOfWork.UserMetricRepository.Update(metric);
    }

    private async Task<string> GetRoomJsonAsync(Guid id)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(
            id,
            includes: [x => x.Challenge, x => x.HostUser!, x => x.RoomUsers!.Where(x => !x.IsOutRoom && !x.IsDeleted)]
        );

        if (room!.RoomUsers != null)
        {
            foreach (var roomUser in room.RoomUsers)
            {
                roomUser.User = await _unitOfWork.UserRepository.GetByIdAsync(roomUser.UserId) ?? null!;
            }
        }
        return JsonConvert.SerializeObject(room, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
            {
                NamingStrategy = new Newtonsoft.Json.Serialization.SnakeCaseNamingStrategy()
            }
        });
    }

}
