using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

using tripdini.accounts.Models;
using tripdini.accounts.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;

namespace tripdini.accounts.Services
{
    public class AuthService : IAuthService
    {
        private readonly MongoRepository repository = null;

        public AuthService(IOptions<Settings> settings) {
            repository = new MongoRepository(settings);
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return null;

            var builder = Builders<User>.Filter;
            var condition = builder.Eq(x => x.username, username) | builder.Eq(x => x.email, username);
            var user = await repository.users
                        .Find(condition)
                        .FirstOrDefaultAsync();

            // check if username exists
            if (user == null) return null;

            // check if password is correct
            // if ( ! VerifyPasswordHash(password, user.passwordHash, user.passwordSalt)) return null;

            // authentication successful
            return user;
        }

        // Authen by third party (Firebase)
        public async Task<User> Authenticate(string email)
        {
            if (string.IsNullOrEmpty(email)) return null;

            var builder = Builders<User>.Filter;
            var condition = builder.Eq(x => x.email, email);
            var user = await repository.users
                        .Find(condition)
                        .FirstOrDefaultAsync();

            // check if username exists
            if (user == null) return null;

            // authentication successful
            return user;
        }

        // private helper methods
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}