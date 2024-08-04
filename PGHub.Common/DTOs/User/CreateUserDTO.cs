using System.ComponentModel.DataAnnotations;

namespace PGHub.Common.DTOs.User
{
    public class CreateUserDTO
    {
        // The AutoMapper library will map only the properties that have the same name in the source and destination classes
        // The extra properties in the source class will be ignored / not mapped / will remain at their default values
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
