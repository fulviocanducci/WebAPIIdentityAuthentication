using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class UserModel
    {
        public UserModel()
        {
        }

        public UserModel(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public UserModel(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
        public Guid SecurityStamp() => Guid.NewGuid();

        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-mail required")]
        [EmailAddress(ErrorMessage = "E-mail invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*[0-9])(?=.*[@#_])[a-zA-Z][\w@#]{6,}[a-zA-Z0-9]$", ErrorMessage = "Password layout invalid")]
        public string Password { get; set; }
    }
}
