using System.Threading.Tasks;

public interface IAuthService
{
    Task<bool> RegisterUserAsync(string email, string password);
    Task<string> AuthenticateUserAsync(string email, string password);
}
