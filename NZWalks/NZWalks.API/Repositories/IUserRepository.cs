using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IUserRepository
    {
        Task<Users> AuthenticateAsync(string username, string password);
    }
}
