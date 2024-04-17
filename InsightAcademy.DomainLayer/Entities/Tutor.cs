using InsightAcademy.DomainLayer.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsightAcademy.DomainLayer.Entities
{
    public class Tutor : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(200)]
        public string LastName { get; set; }
        [Required]
        [StringLength(500)]
        public string Tagline { get; set; }
        public decimal HourlyRate { get; set; }
        [Required]
        [StringLength(50)]
        public string Country { get; set; }
        [Required]
        [StringLength(50)]
        public string City { get; set; }
        [Required]
        [StringLength(50)]
        public string ZipCode { get; set; }
        [Required]
        [StringLength(500)]
        public string Language { get; set; }
        [Required]
        [StringLength(500)]
        [ValidateNever]

        public string Services { get; set; } = "MyHome";
        [Required]
        [StringLength(1000)]
        public string Introduction { get; set; }
        [Required]
        [StringLength(20)]

        public string PhoneNumber { get; set; } = "4375345";
        [Required]
        [StringLength(50)]

        public string EmailAddress { get; set; } = "hamza@gmail.com";
        [StringLength(500)]
        public string SkypeId { get; set; }
        [StringLength(20)]
        public string WhatsappNumber { get; set; }
        [StringLength(50)]
        public string Website { get; set; }
        public bool IsVerified { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Contact Contact { get; set; }
        public List<Education> Education { get; set; }
        public List<TutorSubject> TutorSubject { get; set; }
  
       
        [NotMapped]
        public string FullName  => FirstName + LastName;

        [NotMapped]
        public List<string> SelectedServices { get; set; }
        public List<WishList> WishLists { get; set; }

    }
}