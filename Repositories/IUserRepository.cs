using System.Data;
using UdemyProject.Models.Domain;

namespace UdemyProject.Repositories
{
    public interface IUserRepository
    {
        Task<UsersDomain> RegisterUser(UsersDomain user);
        Task<RolesDomain> RegisterRole(RolesDomain role);
        Task<UsersRolesDomain> RegisterUserRole(UsersRolesDomain userRole);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        Task<UsersDomain> AuthenticateAsync(string username);
    }
}
