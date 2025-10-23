using System.ComponentModel.DataAnnotations;

namespace part_2.Models
{
    public class RegisterViews
    {
        [Key]
        public int Id { get; set; }

        public required string FullName { get; set; }

        public required string Email { get; set; }

        [MinLength(6)]
        public required string  Password { get; set; }
        //public required string  PasswordHash { get; set; }


        public required string Role { get; set; }

    }
}
