namespace UdemyProject.Models.Domain
{
    public class RolesDomain
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<UsersRolesDomain> userRolesDomain { get; set; }
    }
}
