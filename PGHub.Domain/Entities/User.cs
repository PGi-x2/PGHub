namespace PGHub.Domain.Entities
{
    public class User
    {
        public User()
        {
            Posts = new List<Post>();      
        }
        public Guid Id { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? PhoneNumber { get; set; } //TBD Later

        public int? RoleType { get; set; }
        public List<Post> Posts { get; set; }
    }
}
