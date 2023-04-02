using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UdemyProject.Data;
using UdemyProject.Models.Domain;
using UdemyProject.Repositories;

namespace UdemyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenHandler _tokenHandler;
        private readonly DBContextClass _dBContextClass;
        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler, DBContextClass dBContextClass)
        {
            _userRepository = userRepository;
            _tokenHandler = tokenHandler;
            _dBContextClass = dBContextClass;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync (Models.DTO.RegisterRequest registerRequest)
        {

            _userRepository.CreatePasswordHash(registerRequest.Password, out byte[] PasswordHash, out byte[] PasswordSalt);
            // Password hashing and salting

            UsersDomain? userDomain = new UsersDomain()
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt
            };
            // pass the value to UsersDomain

            #region Check Email Address
            List<string> checkEmail = new List<string>();

            foreach (var email in _dBContextClass.UsersTable)
            {
                checkEmail.Add(email.EmailAddress);
            }

            if(checkEmail.Contains(registerRequest.EmailAddress))
            {
                return BadRequest("Email Address Already Exists");
            }
            else
            {
                userDomain.EmailAddress = registerRequest.EmailAddress;
            }

            #endregion

            #region Check Username
            List<string> checkUsername = new List<string>();

            foreach(var username in _dBContextClass.UsersTable)
            {
                checkUsername.Add(username.Username);
                // add all username that exists in UsersTable to list checkUsername
            }

            if(checkUsername.Contains(registerRequest.Username))
            {
                return BadRequest("Username Already Exists");
                // If the username already esist, return this
            }
            else
            {
                userDomain.Username = registerRequest.Username;
                // if the username does not exist, create new one
            }
            #endregion

            #region Check Roles
            List<string> checkRoles = new List<string>();

            registerRequest.Roles.ForEach(roleInput =>
            {
                checkRoles.Add(roleInput.Title);
            });

            userDomain.Roles = checkRoles;
            userDomain = await _userRepository.RegisterUser(userDomain);

            List<string> checkRolesDB = new List<string>();

            foreach (var existingRole in _dBContextClass.RolesTable)
            {
                checkRolesDB.Add(existingRole.Title);
                // add every roles in the RolesTable to list checkRolesDB
            }

            RolesDomain? rolesDomain = new RolesDomain();
            UsersRolesDomain? userRolesDomain = new UsersRolesDomain();

            foreach (var role in userDomain.Roles)
            {
                if (checkRolesDB.Contains(role))
                {
                    RolesDomain? roleInDatabase = await _dBContextClass.RolesTable.FirstOrDefaultAsync(x => x.Title == role);
                    // if checkRolesDB contains type of role inserted by user via userDomain.Roles, do this

                    rolesDomain.Id = roleInDatabase.Id;
                    rolesDomain.Title = roleInDatabase.Title;
                    // pass the value found to this
                }
                else
                {
                    rolesDomain.Title = role;
                    rolesDomain = await _userRepository.RegisterRole(rolesDomain);
                    // if checkRolesDB does not contains type of role inserted by user via userDomain.Roles, do this 
                    // pass the value found to this. Save it into the database
                }
                userRolesDomain.UserID = userDomain.Id;
                userRolesDomain.RoleID = rolesDomain.Id;
                userRolesDomain = await _userRepository.RegisterUserRole(userRolesDomain);
                // create new userRolesDomain entry per new registration
            }

            #endregion

            return Ok(registerRequest);
        }


        [HttpPost] 
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {

            UsersDomain? authenticatedUser = await _userRepository.AuthenticateAsync(loginRequest.Username);
            // check the existence of inserted username

            if (authenticatedUser != null)
            {
                if(!(_userRepository.VerifyPasswordHash(loginRequest.Password, authenticatedUser.PasswordHash, authenticatedUser.PasswordSalt)))
                {
                    return BadRequest("Wrong Password");
                }
                // verify the password hash in the case username exist in database
                
                var token = await _tokenHandler.CreateTokenAsync(authenticatedUser); 
                // create token for user authorization

                return Ok(token); 
            }

            return BadRequest("Username or Password is incorrect");
        }
    }
}
