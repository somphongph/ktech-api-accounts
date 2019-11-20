using System.Threading.Tasks;

using tripdini.accounts.Models;


namespace tripdini.accounts.Services.Interfaces
{
    public interface IAccountService
    {
        // Task<List<Profile>> GetAll();
        Task<Signature> GetSignatureById(string id);
        Task<Profile> GetProfileById(string id);
        Task<ProfileImage> GetProfileImageById(string id);
        Task<ProfileHeader> GetProfileHeaderById(string id);
        Task<ProfileSummary> GetProfileSummaryById(string id);
        Task<User> Create(Register register);
        Task<bool> UpdateProfile(string id, Profile profile);
        Task<bool> UpdateProfileImage(string id, ProfileImage profileImage);
        // Task Delete(string id);
    }
}