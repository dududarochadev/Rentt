using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Rentt.Entities;

namespace Rentt.Infrastructure.Authentication
{
    public class UserStore : IUserStore<User>, IUserPasswordStore<User>, IUserRoleStore<User>
    {
        private readonly IMongoCollection<User> _users;

        public UserStore(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("user");
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await _users.InsertOneAsync(user, cancellationToken: cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            var update = Builders<User>.Update
                .Set(u => u.Email, user.Email)
                .Set(u => u.PasswordHash, user.PasswordHash)
                .Set(u => u.Roles, user.Roles)
                .Set(u => u.UserName, user.UserName)
                .Set(u => u.NormalizedUserName, user.NormalizedUserName);

            var result = await _users.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

            return result.MatchedCount > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Update failed." });
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            await _users.DeleteOneAsync(u => u.Id == user.Id, cancellationToken);
            return IdentityResult.Success;
        }

        public void Dispose() { }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _users.Find(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<User> FindByNameAsync(string userName, CancellationToken cancellationToken)
        {
            var normalizedUserName = userName.ToUpperInvariant();
            return await _users.Find(u => u.NormalizedUserName == normalizedUserName).FirstOrDefaultAsync(cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Roles ?? new List<string>());
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Roles != null && user.Roles.Contains(roleName));
        }

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            if (user.Roles == null)
            {
                user.Roles = new List<string>();
            }

            if (!user.Roles.Contains(roleName))
            {
                user.Roles.Add(roleName);
                await UpdateAsync(user, cancellationToken);
            }
        }

        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            if (user.Roles != null && user.Roles.Contains(roleName))
            {
                user.Roles.Remove(roleName);
                await UpdateAsync(user, cancellationToken);
            }
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
