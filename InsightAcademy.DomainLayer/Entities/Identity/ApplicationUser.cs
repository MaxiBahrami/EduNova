using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsightAcademy.DomainLayer.Entities.Identity
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [StringLength(200)]
        public string FullName { get; set; }
        public string Website { get; set; }
        public string StreetAdress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ProfileImageUrl { get; set; }
        
    }
}
