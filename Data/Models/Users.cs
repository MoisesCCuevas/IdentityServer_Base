using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace AuthenticationSystem.Data.Models
{
    // The IdentityUser implemented is optional
    public class Users : IdentityUser
    {
        //Users properties
        public string PasswordSalt { get; set; }
        public bool Active { get; set; }

        [NotMapped]
        public string PasswordString { get; set; }

        [NotMapped]
        public Claim[] Claims;
    }
}
