using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(string email, string password);
        Task<string> AuthenticateUserAsync(string email, string password);
    }
}