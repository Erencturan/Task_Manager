using Microsoft.AspNetCore.Identity;

namespace Task_Manager.Infrastructure.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
