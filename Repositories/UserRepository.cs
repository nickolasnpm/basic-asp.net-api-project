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
            // Data Collation in Database (Case Sensitive Query)
            // UserDomain? user is equal to the Object User that owns that username property
            // From this we can know: Username, EmailAddress, Password, FirstName, LastName
            
            if (user == null)
            { 
                return null;
            }  

            List<UsersRolesDomain> userRoles = await _dBContextClasses.UsersRolesTable.
                Where(x => x.UserID == user.Id).ToListAsync();
                // UserID refer to the ID of user object in the UsersRolesTable    
                // user.Id contains Id value from the user object obtained from the UserDomain? user
                // List<UserRolesDomain> userRoles is equal to the roles of user that owns that ID property
                // From this we can know: userRolesDomain ID & RoleID for different role.
                // 1 UserID might have relationship with 2 RoleID. If 2 UserID found, list count = 2

            if (userRoles.Any())
            {
                user.Roles = new List<string>();
                // To declare that user.Roles in the UserDomain will be a new List of string

                foreach (var userrole in userRoles)
                {
                   RolesDomain? role = await _dBContextClasses.RolesTable.
                        FirstOrDefaultAsync(x => x.Id == userrole.RoleID);
                    // x.Id refers to the ID in the roleDB 
                    // userrole.RoleID refers to the ID of the userrole object inside the List<UserRolesDomain> userRoles
                    // RoleDomain? role is equal to the roles of user that owns that ID property
                    // From this we can know: first role & second role

                    if (role != null)
                    {
                        user.Roles.Add(role.Title);
                        // push the type of role into user.Roles list declare above [ user.Roles = new List<string>() ]
                    }

                    // the process continued until the last index of userRoles which is 2. 
                }
            }

            return user;
            // user contains properties such as username, email, password, firstname, lastname, and list of Roles as defined in Models.Domain.UserDomain
            // The value is passed to the authentication controllers for token generation
        }

        
    }
}
