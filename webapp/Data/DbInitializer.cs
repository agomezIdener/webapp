using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;

namespace webapp.Data
{
    public class DbInitializer
    {

        /// <summary>
        /// Initializes the database
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var dbContextFactory = serviceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
            using var applicationDbContext = dbContextFactory.CreateDbContext();
            var isConnected = false;

            while (isConnected == false)
            {
                try
                {
                    applicationDbContext.Database.Migrate();
                    isConnected = true;
                }
                catch (Exception ex)
                {
                    //logger.LogError("An error occurred while migrating the database.", ex);
                    throw;
                }
                Thread.Sleep(1_000);
            }

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            try
            {
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await SeedRolesAsync(userManager, roleManager);
                await SeedAdminAsync(userManager, roleManager);
                await SeedUserAsync(userManager, roleManager);


            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }


        }


        public static async Task SeedRolesAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.UserRoles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.UserRoles.User.ToString()));
        }

        public static async Task SeedAdminAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new IdentityUser
            {
                UserName = "admin@webapp.com",
                Email = "admin@webapp.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Admin23.");
                    await userManager.AddToRoleAsync(defaultUser, Roles.UserRoles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.UserRoles.User.ToString());
                }

            }
        }

        public static async Task SeedUserAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new IdentityUser
            {
                UserName = "user@webapp.com",
                Email = "user@webapp.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "User23.");
                    await userManager.AddToRoleAsync(defaultUser, Roles.UserRoles.User.ToString());
                }

            }
        }
    }
}
