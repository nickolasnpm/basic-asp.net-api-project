using UdemyProject.Models.Domain;

namespace UdemyProject.Repositories
{
    public interface ITokenHandler
    {
        Task<string> CreateTokenAsync(UsersDomain userDomain);
    }
}
