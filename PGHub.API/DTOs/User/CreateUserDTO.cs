namespace PGHub.API.DTOs.User
{
    public class CreateUserDTO
    {
        // The AutoMapper library will map only the properties that have the same name in the source and destination classes
        // The extra properties in the source class will be ignored / not mapped / will remain at their default values
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
