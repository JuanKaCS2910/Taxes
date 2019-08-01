using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using Taxes.Models;

namespace Taxes.Clases
{
    public class Utilities : IDisposable
    {
        private static ApplicationDbContext userContext = new ApplicationDbContext();

        /// <summary>
        /// Método Statico es que Yo no tengo que instanciar la clase.
        /// </summary>
        /// <param name="roleName"></param>
        public static void CheckRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }
        }

        public static void CheckSuperUser()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByName("juansocla@hotmail.com");
            if (userASP == null)
            {
                CreateUserASP("juansocla@hotmail.com", "Admin");
            }
            userManager.AddToRole(userASP.Id, "Admin");
        }

        public static void CreateUserASP(string email, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = new ApplicationUser
            {
                Email = email,
                UserName = email,
            };

            userManager.Create(userASP, email);
            userManager.AddToRole(userASP.Id, roleName);
        }

        public void Dispose()
        {
            userContext.Dispose();
        }
    }
}