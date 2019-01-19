using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Identity
{
    public static class IdentityDataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var passwordHasher = new PasswordHasher<SystemUser>();
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            string[] roles = new string[] { UserRoleTypes.Admin.ToString(),
                UserRoleTypes.Patient.ToString(),
                UserRoleTypes.Doctor.ToString(),
                UserRoleTypes.Nurse.ToString(),
                UserRoleTypes.InternalPharmacist.ToString(),
                UserRoleTypes.ExternalPharmacist.ToString() };
            foreach (var role in roles)
            {
                if (!roleManager.Roles.Any(r => r.Name == role))
                {
                    roleManager.CreateAsync(new IdentityRole(role)).Wait();
                }
            }
            CreateTestUsers(serviceProvider, roles);
        }

        private static void CreateTestUsers(IServiceProvider serviceProvider, string[] roles)
        {
            var userManager = serviceProvider.GetService<UserManager<SystemUser>>();
            var passwordHasher = new PasswordHasher<SystemUser>();
            foreach (var role in roles)
            {

                var userTemplate = userManager.FindByNameAsync(role).Result;
                if (userTemplate == null)
                {
                    userTemplate = new SystemUser();
                    var passwordHash = passwordHasher.HashPassword(userTemplate, role+"1!");
                    userTemplate.EmailConfirmed = true;
                    userTemplate.UserName = role;
                    userTemplate.PasswordHash = passwordHash;
                    userTemplate.Email = role+"@local.host";

                    var result = userManager.CreateAsync(userTemplate).Result;
                        result = userManager.AddToRoleAsync(userTemplate, role).Result;
                    if (result.Succeeded)
                    {
                        if (!result.Succeeded)
                        {
                            throw new Exception("Error while adding" + role + " to identity database!");
                        }
                    }
                    else
                    {
                        throw new Exception("Error while creating user: " + role + "!");
                    }
                }
                
            }
        }
    }
}
