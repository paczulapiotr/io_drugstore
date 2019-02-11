using Drugstore.Data;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Drugstore.Identity
{
    public static class DataSeeder
    {

        public static void InitializeDepartments(IServiceProvider serviceProvider)
        {
            var drugstore = serviceProvider.GetService<DrugstoreDbContext>();

            if (drugstore.Departments.Count() == 0)
            {
                drugstore.Departments.Add(new Core.Department() { Name = "Oddział Anestezjologii i Intensywnej Terapii" });
                drugstore.Departments.Add(new Core.Department() { Name = "Oddział Chirurgii Ogólnej" });
                drugstore.Departments.Add(new Core.Department() { Name = "Oddział Chirurgii Ogólnej i Onkologicznej" });
                drugstore.Departments.Add(new Core.Department() { Name = "Oddział Chirurgii Urazowo – Ortopedycznej" });
                drugstore.Departments.Add(new Core.Department() { Name = "Oddział Chorób Płuc i Chemioterapii" });
                drugstore.Departments.Add(new Core.Department() { Name = "Oddział Chorób Wewnętrznych i Geriatrii" });
                drugstore.Departments.Add(new Core.Department() { Name = "Oddział Kardiologiczny" });
                drugstore.SaveChanges();
            }

        }
        public static void InitializeUsers(IServiceProvider serviceProvider)
        {
            var repository = serviceProvider.GetService<IRepository>();
            var passwordHasher = new PasswordHasher<SystemUser>();
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            string[] roles = new string[] {
                UserRoleTypes.Admin.ToString(),
                UserRoleTypes.Patient.ToString(),
                UserRoleTypes.Doctor.ToString(),
                UserRoleTypes.Nurse.ToString(),
                UserRoleTypes.InternalPharmacist.ToString(),
                UserRoleTypes.ExternalPharmacist.ToString(),
                UserRoleTypes.Storekeeper.ToString()};
            foreach (var role in roles)
            {
                if (!roleManager.Roles.Any(r => r.Name == role))
                {
                    roleManager.CreateAsync(new IdentityRole(role)).Wait();
                }
            }
            CreateTestUsers(serviceProvider, repository, roles);
        }
        private static void CreateTestUsers(IServiceProvider serviceProvider, IRepository repository, string[] roles)
        {
            var userManager = serviceProvider.GetService<UserManager<SystemUser>>();
            var passwordHasher = new PasswordHasher<SystemUser>();
            var firstDepartment = serviceProvider
                .GetService<DrugstoreDbContext>()
                .Departments
                .First();

            foreach (var role in roles)
            {

                var userTemplate = userManager.FindByNameAsync(role).Result;
                if (userTemplate == null)
                {
                    var userModel = new UserViewModel
                    {
                        FirstName = role,
                        SecondName = role,
                        Password = role + "1!",
                        ConfirmPassword = role + "1!",
                        DepartmentID = firstDepartment.ID,
                        Email = role + "@local.host",
                        PhoneNumber = "123456789",
                        Role = (UserRoleTypes)Enum.Parse(typeof(UserRoleTypes),role),
                        UserName = role
                    };
                    repository.AddNewUser(userModel);
                }

            }
        }
    }
}
