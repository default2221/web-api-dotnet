using System.ComponentModel.DataAnnotations;

namespace WebAPi.Dtos
{
    public class UserRequest
    {
        [Required]
        public string FirstName { get; set; } = default!;

        [Required]
        public string LastName { get; set; } = default!;

        [Required]
        public string UserName { get; set; } = default!;
    }
}
