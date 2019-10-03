using System.Threading.Tasks;

using apiaccounts.Models;


namespace apiaccounts.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> Authenticate(string username, string password);
    }
}