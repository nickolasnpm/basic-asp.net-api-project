using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using UdemyProject.Data;
using UdemyProject.Models.Domain;

namespace UdemyProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContextClass _dBContextClasses;
        public UserRepository(DBContextClass dBContextClasses)
        {
            _dBContextClasses = dBContextClasses;
        }

        public async Task<UsersDomain> RegisterUser(UsersDomain user)
        {
            user.Id = Guid.NewGuid();
            await _dBContextClasses.UsersTable.AddAsync(user);
            await _dBContextClasses.SaveChangesAsync();
            return user;
        }
        public async Task<RolesDomain> RegisterRole(RolesDomain role)
        {
            role.Id = Guid.NewGuid();
            await _dBContextClasses.RolesTable.AddAsync(role);
            await _dBContextClasses.SaveChangesAsync();
            return role;
        }

        public async Task<UsersRolesDomain> RegisterUserRole(UsersRolesDomain userRole)
        {
            userRole.Id = Guid.NewGuid();
            await _dBContextClasses.UsersRolesTable.AddAsync(userRole);
            await _dBContextClasses.SaveChangesAsync();
            return userRole;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordSalt = hmac.Key;
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public async Task<UsersDomain> AuthenticateAsync(string username)
        {

            UsersDomain? user = await _dBContextClasses.UsersTable.FirstOrDefaultAsync(x =>
            EF.Functions.Collate(x.Username, "SQL_Latin1_General_CP1_CS_AS") == username);
            
            if (user == null)
            { 
                return null;
            }  

            List<UsersRolesDomain> userRoles = await _dBContextClasses.UsersRolesTable.
                Where(x => x.UserID == user.Id).ToListAsync();

            if (userRoles.Any())
            {
                user.Roles = new List<string>();

                foreach (var userrole in userRoles)
                {
                   RolesDomain? role = await _dBContextClasses.RolesTable.
                        FirstOrDefaultAsync(x => x.Id == userrole.RoleID);

                    if (role != null)
                    {
                        user.Roles.Add(role.Title);
                    }
                }
            }

            return user;

        }
    }
}
