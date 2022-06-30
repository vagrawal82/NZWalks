using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class StaticUserRepository : IUserRepository
    {

        private List<Users> users = new List<Users>()
        {
            new Users()
            {
                FirstName = "Read Only",
                LastName ="User",
                Email = "Readonly@user.com",
                Id= new Guid(),
                UserName = "Readonly@user.com",
                Password = "Readonly@user.com",
                Roles = new List<string>{"reader"}
            },

            new Users()
            {
                FirstName = "Read Write",
                LastName ="User",
                Email = "readwrite@user.com",
                Id= new Guid(),
                UserName = "readwrite@user.com",
                Password = "readwrite@user.com",
                Roles = new List<string>{"reader","writer"}
            }
        };

    public async Task<Users> AuthenticateAsync(string username, string password)
        {
            var user = users.Find(x => x.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase) &&
            x.Password == password);

            return user;
        }

      
    }
}
