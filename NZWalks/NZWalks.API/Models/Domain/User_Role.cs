namespace NZWalks.API.Models.Domain
{
    public class User_Role
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public Users User { get; set; }

        public Guid RoleId { get; set; }

        public Role Role { get; set; }
    }
}
