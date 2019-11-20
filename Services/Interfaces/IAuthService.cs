using System.Threading.Tasks;

using tripdini.accounts.Models;


namespace tripdini.accounts.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> Authenticate(string email);
    }
}