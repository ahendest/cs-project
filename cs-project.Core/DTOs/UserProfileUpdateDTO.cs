using System.ComponentModel.DataAnnotations;

namespace cs_project.Core.DTOs
{
    public class UserProfileUpdateDTO
    {
        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}

