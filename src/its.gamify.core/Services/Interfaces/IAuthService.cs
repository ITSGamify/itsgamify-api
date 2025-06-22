using its.gamify.core.Models.Auth;

namespace its.gamify.core.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthResponseModel> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
        public Task<AuthResponseModel> LoginGoogleAsync(string token, CancellationToken cancellationToken = default);
        Task SignUpAsync(string email, string password);
        Task<Firebase.Auth.User?> GetUserAsync(string email, string password, CancellationToken cancellationToken);
    }
}
