using System.ComponentModel.DataAnnotations;

namespace PGHub.Common.DTOs.User
{
    public class UpdateUserDTO
    {
        // public Guid Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100), MinLength(3)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100), MinLength(3)]
        public string LastName { get; set; }
    }
}
