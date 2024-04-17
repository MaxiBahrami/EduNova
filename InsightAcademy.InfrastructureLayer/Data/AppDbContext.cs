using InsightAcademy.ApplicationLayer.Services;
using InsightAcademy.DomainLayer.Entities;
using InsightAcademy.DomainLayer.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace InsightAcademy.InfrastructureLayer.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Tutor> Tutor { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<TutorSubject> TutorSubject { get; set; }
        public DbSet<WishList> WishList { get; set; }
     
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
          
    }

    }

