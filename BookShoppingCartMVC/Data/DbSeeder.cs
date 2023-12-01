using BookShoppingCartMVC.Constants;
using Microsoft.AspNetCore.Identity;

namespace BookShoppingCartMVC.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            //Create User / managing User
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            //Managing Role
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();

            //adding some roles to db
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));


            //create admin user

            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            };

            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb == null) 
            { 
                await userMgr.CreateAsync(admin, "Admin@123");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());

            }

        }
    }
}
