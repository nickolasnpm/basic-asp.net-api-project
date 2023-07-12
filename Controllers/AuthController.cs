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

            #region Create userDomain property

            UsersDomain? userDomain = new UsersDomain();

            // get reference email from database
            List<string> checkEmail = new List<string>();
            foreach (var email in _dBContextClass.UsersTable)
            {
                checkEmail.Add(email.EmailAddress);
            }

            // check the email existence 
            if(checkEmail.Contains(registerRequest.EmailAddress))
            {
                return BadRequest("Email Address Already Exists");
            }
            else
            {
                _userRepository.CreatePasswordHash(registerRequest.Password, out byte[] PasswordHash, out byte[] PasswordSalt);

                userDomain.FirstName = registerRequest.FirstName;
                userDomain.LastName = registerRequest.LastName;
                userDomain.Username = registerRequest.Username;
                userDomain.EmailAddress = registerRequest.EmailAddress;
                userDomain.PasswordHash = PasswordHash;
                userDomain.PasswordSalt = PasswordSalt;
                
            }

            // get the role list from registerRequest
            List<string> checkRoles = new();
            registerRequest.Roles.ForEach(roleInput =>
            {
                checkRoles.Add(roleInput.Title);
            });

            // create new userDomain property object
            userDomain.Roles = checkRoles;
            userDomain = await _userRepository.RegisterUser(userDomain);
            
            #endregion

            #region Create RolesDomain & UsersRolesDomain properties
            
            RolesDomain? rolesDomain = new RolesDomain();
            UsersRolesDomain? userRolesDomain = new UsersRolesDomain();

            // get reference role from database
            List<string> checkRolesDB = new List<string>();
            foreach (var existingRole in _dBContextClass.RolesTable)
            {
                checkRolesDB.Add(existingRole.Title);
            }

            foreach (var role in userDomain.Roles)
            {
                // check if checkRolesDB already contains role inserted by user via userDomain.Roles
                if (checkRolesDB.Contains(role))
                {
                    RolesDomain? roleInDatabase = await _dBContextClass.RolesTable.FirstOrDefaultAsync(x => x.Title == role);
                    rolesDomain.Id = roleInDatabase.Id;
                }
                else
                {
                    // if the role does not exist, create new role
                    rolesDomain.Title = role;
                    rolesDomain = await _userRepository.RegisterRole(rolesDomain);
                }
                
                // create new userRolesDomain entry per new registration
                userRolesDomain.UserID = userDomain.Id;
                userRolesDomain.RoleID = rolesDomain.Id;
                userRolesDomain = await _userRepository.RegisterUserRole(userRolesDomain);
            }

            #endregion

            return Ok(registerRequest);
        }

        [HttpPost] 
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {

            // check the existence of inserted username
            UsersDomain? authenticatedUser = await _userRepository.AuthenticateAsync(loginRequest.Username);
            
            if (authenticatedUser != null)
            {
                // verify the password hash in the case username exist in database
                if(!(_userRepository.VerifyPasswordHash(loginRequest.Password, authenticatedUser.PasswordHash, authenticatedUser.PasswordSalt)))
                {
                    return BadRequest("Wrong Password");
                }
                
                // create token for user authorization
                var token = await _tokenHandler.CreateTokenAsync(authenticatedUser);

                return Ok(token); 
            }

            return BadRequest("Username or Password is incorrect");
        }
    }
}
