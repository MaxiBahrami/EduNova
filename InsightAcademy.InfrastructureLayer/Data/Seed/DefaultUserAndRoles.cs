using InsightAcademy.DomainLayer.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace InsightAcademy.InfrastructureLayer.Data.Seed;
    public class DefaultUserAndRoles : IHostedService
    {
        private readonly IServiceProvider _services;
        public DefaultUserAndRoles(IServiceProvider services)
        {
            _services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
        string adminEmail = "Admin@gmail.com";
        string adminPassword = "Pa$$w0rd";
            string[] rolesToAdd = { Roles.SuperAdmin,Roles.Teacher,Roles.Student };

            using (var scope = _services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                foreach (string roleName in rolesToAdd)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole { Name = roleName, NormalizedName = roleName.ToUpper() });
                    }
                }

                // Check if SuperAdmin user exists
                var superAdmin = await userManager.FindByEmailAsync(adminEmail);
                if (superAdmin == null)
                {
                    // SuperAdmin user doesn't exist, create it
                    superAdmin = new ApplicationUser { FullName ="Admin",UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                    var createResult = await userManager.CreateAsync(superAdmin, adminPassword);

                    if (createResult.Succeeded)
                    {
                        // Assign SuperAdmin role to the SuperAdmin user
                        await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

