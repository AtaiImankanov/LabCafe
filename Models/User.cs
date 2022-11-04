using Microsoft.AspNetCore.Identity;
using System;

namespace homework_64_Atai.Models
{
    public class User : IdentityUser<int>
    {
        public String Role { get; set; }

    }
}
