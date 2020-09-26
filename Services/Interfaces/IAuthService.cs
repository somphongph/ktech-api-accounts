using System.Threading.Tasks;

using ktech.accounts.Models;


namespace ktech.accounts.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> Authenticate(string email);
    }
}