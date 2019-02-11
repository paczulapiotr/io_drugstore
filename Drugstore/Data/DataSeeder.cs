using System;
using System.Linq;
using Drugstore.Core;
using Drugstore.Data;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Drugstore.Identity
{
    public static class DataSeeder
    {
        public static void InitializeDepartments(IServiceProvider serviceProvider)
        {
            var drugstore = serviceProvider.GetService<DrugstoreDbContext>();

            if (drugstore.Departments.Count() == 0)
            {
                drugstore.Departments.Add(new Department {Name = "Oddział Anestezjologii i Intensywnej Terapii"});
                drugstore.Departments.Add(new Department {Name = "Oddział Chirurgii Ogólnej"});
                drugstore.Departments.Add(new Department {Name = "Oddział Chirurgii Ogólnej i Onkologicznej"});
                drugstore.Departments.Add(new Department {Name = "Oddział Chirurgii Urazowo – Ortopedycznej"});
                drugstore.Departments.Add(new Department {Name = "Oddział Chorób Płuc i Chemioterapii"});
                drugstore.Departments.Add(new Department {Name = "Oddział Chorób Wewnętrznych i Geriatrii"});
                drugstore.Departments.Add(new Department {Name = "Oddział Kardiologiczny"});
                drugstore.SaveChanges();
            }
        }

        public static void InitializeMedicines(IServiceProvider serviceProvider)
        {
            var medicines = serviceProvider.GetService<DrugstoreDbContext>();

            if (!medicines.Medicines.Any())
            {
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 1,
                    IsRefunded = false,
                    Name = "Neurofen",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 15.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 2,
                    IsRefunded = true,
                    Name = "Xanax",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 50.0f,
                    Quantity = 4
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 3,
                    IsRefunded = false,
                    Name = "Nifuroksazyt",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 9.99f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 4,
                    IsRefunded = false,
                    Name = "EllaOne",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 30.0f,
                    Quantity = 2
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 5,
                    IsRefunded = false,
                    Name = "Ablify",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 40.0f,
                    Quantity = 3
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 6,
                    IsRefunded = false,
                    Name = "Wegiel aktywny",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 5.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 7,
                    IsRefunded = false,
                    Name = "Arnika",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 20.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = 0,
                    IsRefunded = false,
                    Name = "Apap",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });

            }
        }

        public static void InitializeUsers(IServiceProvider serviceProvider)
        {
            var repository = serviceProvider.GetService<IRepository>();
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            var roles = new[]
            {
                UserRoleTypes.Admin.ToString(),
                UserRoleTypes.Patient.ToString(),
                UserRoleTypes.Doctor.ToString(),
                UserRoleTypes.Nurse.ToString(),
                UserRoleTypes.InternalPharmacist.ToString(),
                UserRoleTypes.ExternalPharmacist.ToString(),
                UserRoleTypes.Storekeeper.ToString()
            };
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
                        PhoneNumber = "000-000-000",
                        Role = (UserRoleTypes) Enum.Parse(typeof(UserRoleTypes), role),
                        UserName = role
                    };
                    repository.AddNewUser(userModel);
                }
            }
        }
    }
}