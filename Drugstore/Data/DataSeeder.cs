using Drugstore.Core;
using Drugstore.Data;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Drugstore.Identity
{
    public static class DataSeeder
    {
        public static void InitializeDepartments(IServiceProvider serviceProvider)
        {
            var drugstore = serviceProvider.GetService<DrugstoreDbContext>();

            if (!drugstore.Departments.Any())
            {
                drugstore.Departments.Add(new Department { Name = "Oddział Anestezjologii i Intensywnej Terapii" });
                drugstore.Departments.Add(new Department { Name = "Oddział Chirurgii Ogólnej" });
                drugstore.Departments.Add(new Department { Name = "Oddział Chirurgii Ogólnej i Onkologicznej" });
                drugstore.Departments.Add(new Department { Name = "Oddział Chirurgii Urazowo – Ortopedycznej" });
                drugstore.Departments.Add(new Department { Name = "Oddział Chorób Płuc i Chemioterapii" });
                drugstore.Departments.Add(new Department { Name = "Oddział Chorób Wewnętrznych i Geriatrii" });
                drugstore.Departments.Add(new Department { Name = "Oddział Kardiologiczny" });
                drugstore.SaveChanges();
            }
        }

        public static void InitializePresciptions(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<DrugstoreDbContext>();
            if (!context.MedicalPrescriptions.Any())
            {
                var medicine1 = context.Medicines.First(m => m.Name == "Tramadol");
                var medicine2 = context.Medicines.First(m => m.Name == "Nifuroksazyt");
                var medicine3 = context.Medicines.First(m => m.Name == "Esberitox N");
                var medicine4 = context.Medicines.First(m => m.Name == "Clonazepamum");


                ICollection<AssignedMedicine> medList1 = new List<AssignedMedicine>{
                    new AssignedMedicine
                {
                    AssignedQuantity = 2,
                    StockMedicine = context.Medicines.First(m=>m.ID == medicine1.ID),
                    PricePerOne = (1 - medicine1.Refundation) * medicine1.PricePerOne,
                },
                    new AssignedMedicine
                {
                    AssignedQuantity = 1,
                    StockMedicine = context.Medicines.First(m=>m.ID == medicine2.ID),
                    PricePerOne = (1 - medicine2.Refundation) * medicine2.PricePerOne,
                },
                    new AssignedMedicine
                {
                    AssignedQuantity = 2,
                    StockMedicine = context.Medicines.First(m=>m.ID == medicine3.ID),
                    PricePerOne = (1 - medicine3.Refundation) * medicine3.PricePerOne,
                },
                    new AssignedMedicine
                {
                    AssignedQuantity = 3,
                    StockMedicine = context.Medicines.First(m=>m.ID == medicine4.ID),
                    PricePerOne = (1 - medicine4.Refundation) * medicine4.PricePerOne,
                }
                  };


                medicine1 = context.Medicines.First(m => m.Name == "Ablify");
                medicine2 = context.Medicines.First(m => m.Name == "Groprinosin");
                medicine3 = context.Medicines.First(m => m.Name == "Morfina");
                ICollection<AssignedMedicine> medList2 = new List<AssignedMedicine>{
                    new AssignedMedicine
                    {

                        AssignedQuantity = 1,
                        StockMedicine = context.Medicines.First(m=>m.ID == medicine1.ID),
                        PricePerOne = (1 - medicine1.Refundation) * medicine1.PricePerOne,
                    },
                    new AssignedMedicine
                    {
                        AssignedQuantity = 2,
                        StockMedicine = context.Medicines.First(m=>m.ID == medicine2.ID),
                        PricePerOne = (1 - medicine2.Refundation) * medicine2.PricePerOne,
                    },
                  new AssignedMedicine
                   {
                        AssignedQuantity = 3,
                        StockMedicine = context.Medicines.First(m=>m.ID == medicine3.ID),
                        PricePerOne = (1 - medicine3.Refundation) * medicine3.PricePerOne,
                   },
            };
                medicine1 = context.Medicines.First(m => m.Name == "Pyralgina");
                medicine2 = context.Medicines.First(m => m.Name == "Groprinosin");
                medicine3 = context.Medicines.First(m => m.Name == "Augmentin");
                ICollection<AssignedMedicine> medList3 = new List<AssignedMedicine>{
                    new AssignedMedicine
                {
                    AssignedQuantity = 2,
                    StockMedicine = context.Medicines.First(m=>m.ID == medicine1.ID),
                        PricePerOne = (1 - medicine1.Refundation) * medicine1.PricePerOne,
                },
                    new AssignedMedicine
                {
                    AssignedQuantity = 3,
                    StockMedicine = context.Medicines.First(m=>m.ID == medicine2.ID),
                    PricePerOne = (1 - medicine2.Refundation) * medicine2.PricePerOne,
                },
                    new AssignedMedicine
                {
                    AssignedQuantity = 2,
                    StockMedicine = context.Medicines.First(m=>m.ID == medicine3.ID),
                    PricePerOne = (1 - medicine3.Refundation) * medicine3.PricePerOne,
                },
            };
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Wieloryb"),
                    Patient = context.Patients.First(p => p.SecondName == "Kasztan"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.NotVerified,
                    Medicines = medList1
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Wieloryb"),
                    Patient = context.Patients.First(p => p.SecondName == "Baron"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.Rejected,
                    Medicines = medList2
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Wieloryb"),
                    Patient = context.Patients.First(p => p.SecondName == "Tulipan"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.Rejected,
                    Medicines = medList1
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Dzyndzel"),
                    Patient = context.Patients.First(p => p.SecondName == "Kalosz"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.NotVerified,
                    Medicines = medList2
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Bulwa"),
                    Patient = context.Patients.First(p => p.SecondName == "Kruk"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.Rejected,
                    Medicines = medList3
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Bulwa"),
                    Patient = context.Patients.First(p => p.SecondName == "Olechno"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.NotVerified,
                    Medicines = medList1
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Bulwa"),
                    Patient = context.Patients.First(p => p.SecondName == "Olechno"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.Accepted,
                    Medicines = medList2
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Jur"),
                    Patient = context.Patients.First(p => p.SecondName == "Lizak"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.Accepted,
                    Medicines = medList1
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Jur"),
                    Patient = context.Patients.First(p => p.SecondName == "Czapla"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.NotVerified,
                    Medicines = medList1
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Jur"),
                    Patient = context.Patients.First(p => p.SecondName == "Brylant"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.Rejected,
                    Medicines = medList3
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Placek"),
                    Patient = context.Patients.First(p => p.SecondName == "Kulig"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.NotVerified,
                    Medicines = medList2
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Placek"),
                    Patient = context.Patients.First(p => p.SecondName == "Morszczyn"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.NotVerified,
                    Medicines = medList1
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Placek"),
                    Patient = context.Patients.First(p => p.SecondName == "Morszczyn"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.Rejected,
                    Medicines = medList2
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Placek"),
                    Patient = context.Patients.First(p => p.SecondName == "Graham"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.Rejected,
                    Medicines = medList2
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Rzodkiew"),
                    Patient = context.Patients.First(p => p.SecondName == "Rogacz"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.Accepted,
                    Medicines = medList2
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Rzodkiew"),
                    Patient = context.Patients.First(p => p.SecondName == "Kolano"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.NotVerified,
                    Medicines = medList2
                });
                context.MedicalPrescriptions.Add(new MedicalPrescription
                {
                    Doctor = context.Doctors.First(d => d.SecondName == "Rzodkiew"),
                    Patient = context.Patients.First(p => p.SecondName == "Dorsz"),
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.Accepted,
                    Medicines = medList2
                });
                context.SaveChanges();
            }

        }



        public static void InitializePeople(IServiceProvider serviceProvider, AdminUseCase adminUseCase)
        {
            var context = serviceProvider.GetService<DrugstoreDbContext>();

            if (context.Patients.Count() == 1)
            {
                //----------------------------------------------------
                UserViewModel newUser = new UserViewModel
                {
                    FirstName = "Henryk",
                    SecondName = "Kasztan",
                    UserName = "HenrykKasztan",
                    Password = "Kasztan1!",
                    ConfirmPassword = "Kasztan1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Anest")).ID,
                    Email = "Kasztan@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Kasztan"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Gabriel",
                    SecondName = "Baron",
                    UserName = "GabrielBaron",
                    Password = "Baron1!",
                    ConfirmPassword = "Baron1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Anest")).ID,
                    Email = "Baron@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Baron"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Roksana",
                    SecondName = "Kruk",
                    UserName = "RoksanaKruk",
                    Password = "Kruk1!!",
                    ConfirmPassword = "Kruk1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Onko")).ID,
                    Email = "Kruk@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Kruk"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Sylwia",
                    SecondName = "Dziobak",
                    UserName = "SylwiaDziobak",
                    Password = "Dziobak1!",
                    ConfirmPassword = "Dziobak1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Onko")).ID,
                    Email = "Dziobak@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Dziobak"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Jadwiga",
                    SecondName = "Lizak",
                    UserName = "JadwigaLizak",
                    Password = "Lizak1!",
                    ConfirmPassword = "Lizak1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Kardio")).ID,
                    Email = "Lizak@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Lizak"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Adam",
                    SecondName = "Olechno",
                    UserName = "AdamOlechno",
                    Password = "Olechno1!",
                    ConfirmPassword = "Olechno1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Onko")).ID,
                    Email = "Olechno@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Olechno"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Wojciech",
                    SecondName = "Czapla",
                    UserName = "WojciechCzapla",
                    Password = "Czapla1!",
                    ConfirmPassword = "Czapla1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Kardio")).ID,
                    Email = "Czapla@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Czapla"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Cyprian",
                    SecondName = "Kalosz",
                    UserName = "CyprianKalosz",
                    Password = "Kalosz1!",
                    ConfirmPassword = "Kalosz1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Chemio")).ID,
                    Email = "Kalosz@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Kalosz"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Bartosz",
                    SecondName = "Kaktus",
                    UserName = "BartoszKaktus",
                    Password = "Kaktus1!",
                    ConfirmPassword = "Kaktus1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Anes")).ID,
                    Email = "Kaktus@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Kaktus"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Ewa",
                    SecondName = "Dziupla",
                    UserName = "EwaDziupla",
                    Password = "Dziupla1!",
                    ConfirmPassword = "Dziupla1!!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Uraz")).ID,
                    Email = "Dziupla@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Dziupla"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Jan",
                    SecondName = "Polansky",
                    UserName = "JanPolansky",
                    Password = "Polansky1!",
                    ConfirmPassword = "Polansky1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Uraz")).ID,
                    Email = "Polansky@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Polansky"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Anna",
                    SecondName = "Torba",
                    UserName = "AnnaTorba",
                    Password = "Torba1!",
                    ConfirmPassword = "Torba1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Uraz")).ID,
                    Email = "Torba@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Torba"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Andrzej",
                    SecondName = "Kabanos",
                    UserName = "AndrzejKabanos",
                    Password = "Kabanos1!",
                    ConfirmPassword = "Kabanos1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Uraz")).ID,
                    Email = "Kabanos@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Kabanos"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Tomasz",
                    SecondName = "Siorbacz",
                    UserName = "TomaszSiorbacz",
                    Password = "Siorbacz1!",
                    ConfirmPassword = "Siorbacz1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Uraz")).ID,
                    Email = "Siorbacz@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Nurse,
                    SystemUserId = "Siorbacz"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Kamil",
                    SecondName = "Rogacz",
                    UserName = "KamilRogacz",
                    Password = "Rogacz1!",
                    ConfirmPassword = "Rogacz1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Geri")).ID,
                    Email = "Rogacz@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Rogacz"
                };
                adminUseCase.AddNewUser(newUser);

                newUser = new UserViewModel
                {
                    FirstName = "Paulina",
                    SecondName = "Dorsz",
                    UserName = "PaulinaDorsz",
                    Password = "Dorsz1!",
                    ConfirmPassword = "Dorsz1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Geri")).ID,
                    Email = "Dorsz@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Dorsz"
                };
                adminUseCase.AddNewUser(newUser);
                //----------------------------------------------------
                newUser = new UserViewModel
                {
                    FirstName = "Katarzyna",
                    SecondName = "Fortepian",
                    UserName = "KatarzynaFortepian",
                    Password = "Fortepian1!",
                    ConfirmPassword = "Fortepian1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Geri")).ID,
                    Email = "Fortepian@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Nurse,
                    SystemUserId = "Fortepian"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Ewelina",
                    SecondName = "Kolano",
                    UserName = "EwelinaKolano",
                    Password = "Kolano1!",
                    ConfirmPassword = "Kolano1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Geri")).ID,
                    Email = "Kolano@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Kolano"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Matylda",
                    SecondName = "Kulig",
                    UserName = "MatyldaKulig",
                    Password = "Kulig1!",
                    ConfirmPassword = "Kulig1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Ortop")).ID,
                    Email = "Kulig@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Kulig"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Monika",
                    SecondName = "Graham",
                    UserName = "MonikaGraham",
                    Password = "Graham1!",
                    ConfirmPassword = "Graham1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Ortop")).ID,
                    Email = "Graham@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Graham"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Maria",
                    SecondName = "Morszczyn",
                    UserName = "MariaMorszczyn",
                    Password = "Morszczyn1!",
                    ConfirmPassword = "Morszczyn1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Ortop")).ID,
                    Email = "Morszczyn@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Morszczyn"
                };
                newUser = new UserViewModel
                {
                    FirstName = "Sebastian",
                    SecondName = "Trampek",
                    UserName = "SebastianTrampek",
                    Password = "Trampek1!",
                    ConfirmPassword = "Trampek1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Ortop")).ID,
                    Email = "Trampek@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Nurse,
                    SystemUserId = "Trampek"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Barbara",
                    SecondName = "Bursztyn",
                    UserName = "BarbaraBursztyn",
                    Password = "Bursztyn1!",
                    ConfirmPassword = "Bursztyn1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Ortop")).ID,
                    Email = "Bursztyn@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Nurse,
                    SystemUserId = "Bursztyn"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Celina",
                    SecondName = "Czoper",
                    UserName = "CelinaCzoper",
                    Password = "Czoper1!",
                    ConfirmPassword = "Czoper1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Ortop")).ID,
                    Email = "Czoper@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Czoper"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Gabriela",
                    SecondName = "Grzebyk",
                    UserName = "GabierlaGrzebyk",
                    Password = "Grzebyk1!",
                    ConfirmPassword = "Grzebyk1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Chiru")).ID,
                    Email = "Grzebyk@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Nurse,
                    SystemUserId = "Grzebyk"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Maja",
                    SecondName = "Macocha",
                    UserName = "MajaMacocha",
                    Password = "Macocha1!",
                    ConfirmPassword = "Macocha1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Chiru")).ID,
                    Email = "Macocha@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Nurse,
                    SystemUserId = "Macocha"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Barabasz",
                    SecondName = "Tulipan",
                    UserName = "BarabaszTulipan",
                    Password = "Tulipan1!!",
                    ConfirmPassword = "Tulipan1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Anes")).ID,
                    Email = "Tulipan@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Tulipan"
                };
                adminUseCase.AddNewUser(newUser);
                //------------------------------------------------
                newUser = new UserViewModel
                {
                    FirstName = "Maciej",
                    SecondName = "Szczotka",
                    UserName = "MaciejSzczotka",
                    Password = "Szczotka1!",
                    ConfirmPassword = "Szczotka1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Chiru")).ID,
                    Email = "Szczotka@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Doctor,
                    SystemUserId = "Szczotka"
                };
                newUser = new UserViewModel
                {
                    FirstName = "Stefan",
                    SecondName = "Wieloryb",
                    UserName = "StefanWieloryb",
                    Password = "Wieloryb1!",
                    ConfirmPassword = "Wieloryb1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Anest")).ID,
                    Email = "Wieloryb@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Doctor,
                    SystemUserId = "Wieloryb"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Grzegorz",
                    SecondName = "Bulwa",
                    UserName = "GrzegorzBulwa",
                    Password = "Bulwa1!",
                    ConfirmPassword = "Bulwa1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Onko")).ID,
                    Email = "Bulwa@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Doctor,
                    SystemUserId = "Bulwa"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Mateusz",
                    SecondName = "Petarda",
                    UserName = "MateuszPetarda",
                    Password = "Petarda1!",
                    ConfirmPassword = "Petarda1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Onko")).ID,
                    Email = "Petarda@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Doctor,
                    SystemUserId = "Petarda"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Robert",
                    SecondName = "Rzodkiew",
                    UserName = "RobertRzodkiew",
                    Password = "Rzodkiew1!",
                    ConfirmPassword = "Rzodkiew1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Geri")).ID,
                    Email = "Rzodkiew@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Doctor,
                    SystemUserId = "Rzodkiew"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Paulo",
                    SecondName = "Jur",
                    UserName = "PauloJur",
                    Password = "Jur1!",
                    ConfirmPassword = "Jur1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Kardi")).ID,
                    Email = "Jur@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Doctor,
                    SystemUserId = "Jur"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Aneta",
                    SecondName = "Sznur",
                    UserName = "AnetaSznur",
                    Password = "Sznur1!",
                    ConfirmPassword = "Sznur1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Kardi")).ID,
                    Email = "Sznur@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Doctor,
                    SystemUserId = "Sznur"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Klaudia",
                    SecondName = "Brylant",
                    UserName = "KlaudiaBrylant",
                    Password = "Brylant1!",
                    ConfirmPassword = "Brylant1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Kardi")).ID,
                    Email = "Brylant@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Patient,
                    SystemUserId = "Brylant"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Piotr",
                    SecondName = "Placek",
                    UserName = "PiotrPlacek",
                    Password = "Placek1!",
                    ConfirmPassword = "Placek!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Orto")).ID,
                    Email = "Placek@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Doctor,
                    SystemUserId = "Placek"
                };
                adminUseCase.AddNewUser(newUser);
                newUser = new UserViewModel
                {
                    FirstName = "Marek",
                    SecondName = "Dzyndzel",
                    UserName = "MarekDzyndzel",
                    Password = "Dzyndzel1!",
                    ConfirmPassword = "Dzyndzel1!",
                    DepartmentID = context.Departments.First(d => d.Name.Contains("Chem")).ID,
                    Email = "Dzyndzel@local.host",
                    PhoneNumber = "",
                    Role = UserRoleTypes.Doctor,
                    SystemUserId = "Dzyndzel"
                };
                adminUseCase.AddNewUser(newUser);
            }
        }


        public static void InitializeExternalDrugstoreMedicines(IServiceProvider serviceProvider)
        {
            var medicines = serviceProvider.GetService<DrugstoreDbContext>();

            if (!medicines.ExternalDrugstoreMedicines.Any())
            {
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Apap",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Apap"),
                    PricePerOne = 10.0f,
                    Quantity = 5
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Neurofen",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Neurofen"),
                    PricePerOne = 15.0f,
                    Quantity = 5
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Xanax",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Xanax"),
                    PricePerOne = 50.0f,
                    Quantity = 1
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Wegiel aktywny",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Wegiel aktywny"),
                    PricePerOne = 5.5f,
                    Quantity = 3
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Scorbolamid",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Scorbolamid"),
                    PricePerOne = 9.0f,
                    Quantity = 3
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Chlorchinaldin",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Apap"),
                    PricePerOne = 10.0f,
                    Quantity = 2
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Theraflu",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Theraflu"),
                    PricePerOne = 13.0f,
                    Quantity = 6
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Pyralgina",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Pyralgina"),
                    PricePerOne = 12.0f,
                    Quantity = 6
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Esberitox N",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Apap"),
                    PricePerOne = 35.0f,
                    Quantity = 3
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Aspiryna",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Aspiryna"),
                    PricePerOne = 12.0f,
                    Quantity = 6
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Controloc Control",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Controloc Control"),
                    PricePerOne = 12.0f,
                    Quantity = 3
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Epiduo",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Epiduo"),
                    PricePerOne = 40.0f,
                    Quantity = 1
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Clonazepamum",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Clonazepamum"),
                    PricePerOne = 25.0f,
                    Quantity = 2
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Stilnox",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Stilnox"),
                    PricePerOne = 30.0f,
                    Quantity = 3
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "NeoAzarina",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "NeoAzarina"),
                    PricePerOne = 70.0f,
                    Quantity = 1
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Espumisan",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Espumisan"),
                    PricePerOne = 22.0f,
                    Quantity = 10
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Tramadol",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Tramadol"),
                    PricePerOne = 35.0f,
                    Quantity = 8
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "EllaOne",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "EllaOne"),
                    PricePerOne = 15.0f,
                    Quantity = 150
                });
                medicines.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                {
                    Name = "Nifuroksazyt",
                    StockMedicine = medicines.Medicines.First(m => m.Name == "Nifuroksazyt"),
                    PricePerOne = 43.0f,
                    Quantity = 25
                });
                medicines.SaveChanges();
            }

        }


        public static void InitializeExternalDrugstoreSoldMedicines(IServiceProvider serviceProvider)
        {
            var medicines = serviceProvider.GetService<DrugstoreDbContext>();

            if (!medicines.ExternalDrugstoreSoldMedicines.Any())
            {
                var medName = "Neurofen";
                medicines.ExternalDrugstoreSoldMedicines.Add(new ExternalDrugstoreSoldMedicine
                {
                    StockMedicine = medicines.ExternalDrugstoreMedicines.First(m => m.Name == medName).StockMedicine,
                    SoldQuantity = 3,
                    Date = new DateTime(2019, 2, 12, 11, 24, 33),
                    PricePerOne = medicines.ExternalDrugstoreMedicines.First(m => m.Name == medName).PricePerOne
                });
                medName = "Xanax";
                medicines.ExternalDrugstoreSoldMedicines.Add(new ExternalDrugstoreSoldMedicine
                {
                    StockMedicine = medicines.ExternalDrugstoreMedicines.First(m => m.Name == medName).StockMedicine,
                    SoldQuantity = 2,
                    Date = new DateTime(2019, 2, 14, 16, 6, 24),
                    PricePerOne = medicines.ExternalDrugstoreMedicines.First(m => m.Name == medName).PricePerOne
                });
                medName = "Apap";
                medicines.ExternalDrugstoreSoldMedicines.Add(new ExternalDrugstoreSoldMedicine
                {
                    StockMedicine = medicines.ExternalDrugstoreMedicines.First(m => m.Name == medName).StockMedicine,
                    SoldQuantity = 1,
                    Date = new DateTime(2019, 1, 14, 15, 46, 14),
                    PricePerOne = medicines.ExternalDrugstoreMedicines.First(m => m.Name == medName).PricePerOne
                });
                medName = "Wegiel aktywny";
                medicines.ExternalDrugstoreSoldMedicines.Add(new ExternalDrugstoreSoldMedicine
                {
                    StockMedicine = medicines.ExternalDrugstoreMedicines.First(m => m.Name == medName).StockMedicine,
                    SoldQuantity = 2,
                    Date = new DateTime(2019, 1, 14, 18, 10, 51),
                    PricePerOne = medicines.ExternalDrugstoreMedicines.First(m => m.Name == medName).PricePerOne
                });
                medName = "Scorbolamid";
                medicines.ExternalDrugstoreSoldMedicines.Add(new ExternalDrugstoreSoldMedicine
                {
                    StockMedicine = medicines.ExternalDrugstoreMedicines.First(m => m.Name == medName).StockMedicine,
                    SoldQuantity = 1,
                    Date = new DateTime(2019, 1, 14, 13, 30, 13),
                    PricePerOne = medicines.ExternalDrugstoreMedicines.First(m => m.Name == medName).PricePerOne
                });
                medName = "Chlorchinaldin";
                medicines.ExternalDrugstoreSoldMedicines.Add(new ExternalDrugstoreSoldMedicine
                {
                    StockMedicine = medicines.ExternalDrugstoreMedicines.First(m => m.Name == medName).StockMedicine,
                    SoldQuantity = 2,
                    Date = new DateTime(2019, 1, 14, 19, 54, 6),
                    PricePerOne = medicines.ExternalDrugstoreMedicines.First(m => m.Name == medName).PricePerOne
                });
                medName = "Theraflu";
                medicines.ExternalDrugstoreSoldMedicines.Add(new ExternalDrugstoreSoldMedicine
                {
                    StockMedicine = medicines.ExternalDrugstoreMedicines.First(m => m.Name == medName).StockMedicine,
                    SoldQuantity = 2,
                    Date = new DateTime(2019, 1, 14, 17, 4, 6),
                    PricePerOne = medicines.ExternalDrugstoreMedicines.First(m => m.Name == medName).PricePerOne
                });
                medicines.SaveChanges();
            }

        }
        public static void InitializeMedicine(IServiceProvider serviceProvider)
        {
            var medicines = serviceProvider.GetService<DrugstoreDbContext>();

            if (!medicines.Medicines.Any())
            {
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.1,
                    Name = "Apap",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.1,
                    Name = "Neurofen",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 15.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.5,
                    Name = "Xanax",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 50.0f,
                    Quantity = 4
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.4,
                    Name = "Nifuroksazyt",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 9.99f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.2,
                    Name = "EllaOne",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 30.0f,
                    Quantity = 2
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.65,
                    Name = "Ablify",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 40.0f,
                    Quantity = 3
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.45,
                    Name = "Wegiel aktywny",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 5.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.10,
                    Name = "Arnika",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 20.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.10,
                    Name = "Rutinoscorbin",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 8.0f,
                    Quantity = 15
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.10,
                    Name = "Scorbolamid",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.10,
                    Name = "Groprinosin",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.10,
                    Name = "Chlorchinaldin",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.20,
                    Name = "Morfina",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 100.0f,
                    Quantity = 1
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.20,
                    Name = "Theraflu",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.20,
                    Name = "Pyralgina",
                    MedicineCategory = 0,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.20,
                    Name = "Augmentin",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 13.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.30,
                    Name = "Esberitox N",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 30.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.30,
                    Name = "Aspiryna",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 10.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.30,
                    Name = "Gripex",
                    MedicineCategory = 0,
                    PricePerOne = 15.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.30,
                    Name = "Polopiryna",
                    MedicineCategory = 0,
                    PricePerOne = 15.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.1,
                    Name = "Controloc Control",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 11.0f,
                    Quantity = 10
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.2,
                    Name = "Normaclin",
                    MedicineCategory = 0,
                    PricePerOne = 20.0f,
                    Quantity = 3
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.25,
                    Name = "Epiduo",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 30.0f,
                    Quantity = 4
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.25,
                    Name = "Tramadol",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 50.0f,
                    Quantity = 2
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.25,
                    Name = "Metadon",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 60.0f,
                    Quantity = 6
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.25,
                    Name = "Fentanyl",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 49.99f,
                    Quantity = 3
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.25,
                    Name = "Clonazepamum",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 25.0f,
                    Quantity = 4
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.25,
                    Name = "Thicodin",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 20.0f,
                    Quantity = 7
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.25,
                    Name = "Tramal Retard 100",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 20.0f,
                    Quantity = 4
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.25,
                    Name = "Stilnox",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 30.0f,
                    Quantity = 4
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.25,
                    Name = "DHC Continus",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 60.0f,
                    Quantity = 2
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.25,
                    Name = "Tramal",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 54.0f,
                    Quantity = 2
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.25,
                    Name = "NeoAzarina",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 70.0f,
                    Quantity = 2
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.25,
                    Name = "Stodal",
                    MedicineCategory = MedicineCategory.Special,
                    PricePerOne = 20.0f,
                    Quantity = 2
                });
                medicines.Medicines.Add(new MedicineOnStock
                {
                    Refundation = 0.2,
                    Name = "Espumisan",
                    MedicineCategory = MedicineCategory.Normal,
                    PricePerOne = 20.0f,
                    Quantity = 2
                });
                medicines.SaveChanges();
            }

            medicines.SaveChanges();
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
            InitializePeople(serviceProvider, adminUseCase);
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
                        Role = (UserRoleTypes)Enum.Parse(typeof(UserRoleTypes), role),
                        UserName = role
                    };
                    adminUseCase.AddNewUser(userModel);
                }
            }
        }
    }
}