﻿using System.ComponentModel.DataAnnotations;

namespace PGHub.Common.DTOs.User
{
    public class UpdateUserDTO
    {
        // public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
