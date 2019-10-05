using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

using apiaccounts.Models;
using apiaccounts.Services.Interfaces;


namespace apiaccounts.Services
{
    public class AccountService : IAccountService
    {
        private readonly MongoRepository repository = null;

        public AccountService(IOptions<Settings> settings) {
            repository = new MongoRepository(settings);
        }

        // public async Task<List<Profile>> GetAll() {
        //     var fields = Builders<User>.Projection
        //                 .Include(x => x.name)
        //                 .Include(x => x.email)
        //                 .Include(x => x.username);

        //     return await repository.users
        //                 .Find(new BsonDocument())
        //                 .Project<Profile>(fields)
        //                 .ToListAsync();
        // }

        public async Task<Signature> GetSignatureById(string id) {

            var condition = Builders<User>.Filter.Eq(x => x.id, id);
            var fields = Builders<User>.Projection
                        .Include(x => x.name)
                        .Include(x => x.username)
                        .Include(x => x.profileImage);

            return await repository.users
                        .Find(condition)
                        .Project<Signature>(fields)
                        .FirstOrDefaultAsync();
        }

        public async Task<Profile> GetProfileById(string id) {

            var condition = Builders<User>.Filter.Eq(x => x.id, id);
            var fields = Builders<User>.Projection
                        .Include(x => x.name)
                        .Include(x => x.email)
                        .Include(x => x.username);

            return await repository.users
                        .Find(condition)
                        .Project<Profile>(fields)
                        .FirstOrDefaultAsync();
        }

        public async Task<ProfileImage> GetProfileImageById(string id) {

            var condition = Builders<User>.Filter.Eq(x => x.id, id);
            var fields = Builders<User>.Projection
                        .Include(x => x.profileImage);

            return await repository.users
                        .Find(condition)
                        .Project<ProfileImage>(fields)
                        .FirstOrDefaultAsync();
        }

        public async Task<ProfileHeader> GetProfileHeaderById(string id) {

            var condition = Builders<User>.Filter.Eq(x => x.id, id);
            var fields = Builders<User>.Projection
                        .Include(x => x.name)
                        .Include(x => x.profileImage);

            return await repository.users
                        .Find(condition)
                        .Project<ProfileHeader>(fields)
                        .FirstOrDefaultAsync();
        }

        public async Task<ProfileSummary> GetProfileSummaryById(string id) {

            var condition = Builders<User>.Filter.Eq(x => x.id, id);
            var fields = Builders<User>.Projection
                        .Include(x => x.name)
                        .Include(x => x.profileImage);

            return await repository.users
                        .Find(condition)
                        .Project<ProfileSummary>(fields)
                        .FirstOrDefaultAsync();
        }


        public async Task Create(Register register) {
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHash(register.password, out passwordHash, out passwordSalt);

            var profileImage = new ProfileImage {
                storeName = "default",
                contentType = "application/octet-stream",
            };

            var user = new User {
                name = register.name,
                email = register.email,
                profileImage = profileImage,
                passwordHash = passwordHash,
                passwordSalt = passwordSalt,
            };

            try
            {
                await repository.users
                    .InsertOneAsync(user);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateProfile(string id, Profile profile) {
            var filter = Builders<User>.Filter.Eq(s => s.id, id);
            var update = Builders<User>.Update
                        .Set(s => s.name, profile.name)
                        .Set(s => s.email, profile.email)
                        .Set(s => s.username, profile.username);
                        // .CurrentDate(s => s.UpdatedOn);
            try
            {
                UpdateResult actionResult
                    = await repository.users.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateProfileImage(string id, ProfileImage profileImage) {
            var filter = Builders<User>.Filter.Eq(s => s.id, id);
            var update = Builders<User>.Update
                        .Set(s => s.profileImage, profileImage);

            try
            {
                UpdateResult actionResult
                    = await repository.users.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // public async Task Delete(string id) {
        //     await repository.users
        //         .DeleteOneAsync(x => x.id == id);
        // }

        // private helper methods
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}