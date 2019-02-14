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

            if (!drugstore.Departments.Any())
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
            var id = 0;

            if (!medicines.Medicines.Any())
            {
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.1,
                    Name = "Apap",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.1,
                    Name = "Neurofen",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 15.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.5,
                    Name = "Xanax",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 50.0f,
                    Quantity = 4
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.4,
                    Name = "Nifuroksazyt",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 9.99f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.2,
                    Name = "EllaOne",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 30.0f,
                    Quantity = 2
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.65,
                    Name = "Ablify",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 40.0f,
                    Quantity = 3
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.45,
                    Name = "Wegiel aktywny",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 5.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.10,
                    Name = "Arnika",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 20.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.10,
                    Name = "Rutinoscorbin",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 8.0f,
                    Quantity = 15
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.10,
                    Name = "Scorbolamid",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.10,
                    Name = "Groprinosin",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.10,
                    Name = "Chlorchinaldin",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.20,
                    Name = "Morfina",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 100.0f,
                    Quantity = 1
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.20,
                    Name = "Theraflu",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.20,
                    Name = "Pyralgina",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.20,
                    Name = "Augmentin",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 13.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.30,
                    Name = "Esberitox N",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 30.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.30,
                    Name = "Aspiryna",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.30,
                    Name = "Gripex",
                    MedicineCategory = 0,
                    PricePerOne = 15.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.30,
                    Name = "Polopiryna",
                    MedicineCategory = 0,
                    PricePerOne = 15.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.1,
                    Name = "Controloc Control",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 11.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.2,
                    Name = "Normaclin",
                    MedicineCategory = 0,
                    PricePerOne = 20.0f,
                    Quantity = 3
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.25,
                    Name = "Epiduo",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 30.0f,
                    Quantity = 4
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.25,
                    Name = "Tramadol",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 50.0f,
                    Quantity = 2
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.25,
                    Name = "Metadon",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 60.0f,
                    Quantity = 6
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.25,
                    Name = "Fentanyl",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 49.99f,
                    Quantity = 3
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.25,
                    Name = "Clonazepamum",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 25.0f,
                    Quantity = 4
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.25,
                    Name = "Thicodin",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 20.0f,
                    Quantity = 7
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.25,
                    Name = "Tramal Retard 100",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 20.0f,
                    Quantity = 4
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.25,
                    Name = "Stilnox",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 30.0f,
                    Quantity = 4
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.25,
                    Name = "DHC Continus",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 60.0f,
                    Quantity = 2
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.25,
                    Name = "Tramal",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 54.0f,
                    Quantity = 2
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.25,
                    Name = "NeoAzarina",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 70.0f,
                    Quantity = 2
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.25,
                    Name = "Stodal",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 20.0f,
                    Quantity = 2
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    ID = id++,
                    Refundation = 0.2,
                    Name = "Espumisan",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 20.0f,
                    Quantity = 2
                });
            }
        }

        public static void InitializeUsers(IServiceProvider serviceProvider)
        {
            var adminUseCase = serviceProvider.GetService<AdminUseCase>();
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

            CreateTestUsers(serviceProvider, adminUseCase, roles);
        }

        private static void CreateTestUsers(IServiceProvider serviceProvider, AdminUseCase adminUseCase, string[] roles)
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
                        PhoneNumber = "123456789",
                        Role = (UserRoleTypes)Enum.Parse(typeof(UserRoleTypes),role),
                        UserName = role
                    };
                    adminUseCase.AddNewUser(userModel);
                }
            }
        }
    }
}