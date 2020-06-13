using System.Threading.Tasks;

using tripgator.accounts.Models;


namespace tripgator.accounts.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> Authenticate(string email);
    }
}