using System.ComponentModel.DataAnnotations.Schema;

namespace UdemyProject.Models.Domain
{
    public class UsersDomain
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [NotMapped]
        public List<string> Roles { get; set; }
        public List<UsersRolesDomain> userRolesDomain { get; set; }
  
    }
}
