namespace UdemyProject.Models.Domain
{
    public class UsersRolesDomain
    {
        public Guid Id { get; set; }
        public Guid UserID { get; set; }
        public UsersDomain UserDomain { get; set; }
        public Guid RoleID { get; set; }
        public RolesDomain RoleDomain { get; set; }
    }
}
