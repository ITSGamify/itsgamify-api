using Firebase.Auth;
using its.gamify.core.Models.Auth;
using its.gamify.core.Models.Users;
using its.gamify.core.Services.Interfaces;
using its.gamify.core.Utilities;
using its.gamify.domains.Models;
using Microsoft.Extensions.DependencyInjection;

namespace its.gamify.core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly AppSetting appSetting;

        public AuthService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
        {
            this.appSetting = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<AppSetting>();
            this.unitOfWork = unitOfWork;

        }

        public async Task<User?> GetUserAsync(string email, string password, CancellationToken cancellationToken)
        {
            var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(appSetting.FirebaseConfig.ApiKey));
            var user = await auth.SignInWithEmailAndPasswordAsync(email, password);

            return user.User;
        }

        public async Task<AuthResponseModel> LoginAsync(string email, string password, CancellationToken cancellationToken)
        {
            var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(appSetting.FirebaseConfig.ApiKey));
            var user = await auth.SignInWithEmailAndPasswordAsync(email, password);
            var userInDb = await unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == email, includes: x => x.Role!);
            if (user is not null && userInDb is not null)
            {

                string newToken = TokenGenerator.GenerateToken(user: userInDb, role: userInDb.Role?.Name ?? string.Empty);
                return new AuthResponseModel
                {
                    Token = newToken,
                    User = unitOfWork.Mapper.Map<UserViewModel>(userInDb)
                };
            }
            throw new InvalidOperationException("Invalid");
        }

        public async Task<AuthResponseModel> LoginGoogleAsync(string token, CancellationToken cancellationToken)
        {
            var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(appSetting.FirebaseConfig.ApiKey));

            var user = await auth.GetUserAsync(token)
                ?? throw new Exception($"Error at {nameof(AuthService)}: Firebase can not get this user");

            if (user is null)
                throw new Exception($"Error at: {nameof(IAuthService)}_ User not exist on firebase authentication");

            var userInDb = await unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == user.Email, includes: x => x.Role!);

            if (userInDb is not null)
            {
                string newToken = TokenGenerator.GenerateToken(user: userInDb, role: userInDb.Role?.Name ?? string.Empty);
                return new AuthResponseModel
                {
                    Token = newToken,
                    User = unitOfWork.Mapper.Map<UserViewModel>(userInDb)
                };
            }
            else throw new InvalidOperationException("User are not created in Database yet");

        }

        public async Task SignUpAsync(string email, string password)
        {
            var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(appSetting.FirebaseConfig.ApiKey));
            await auth.CreateUserWithEmailAndPasswordAsync(email, password);
        }
    }
}
