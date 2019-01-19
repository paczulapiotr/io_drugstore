using Drugstore.Core;
using Drugstore.Identity;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Drugstore.Data
{
    public class DrugstoreRepository : IRepository
    {
        private readonly UserManager<SystemUser> userManager;
        private readonly DrugstoreDbContext drugstoreDbContext;

        public DrugstoreRepository(UserManager<SystemUser> userManager, DrugstoreDbContext drugstoreDbContext)
        {
            this.userManager = userManager;
            this.drugstoreDbContext = drugstoreDbContext;
        }

        public void AddNewUser(NewUserModel newUser)
        {
            Person person = null;
            SystemUser systemUser = new SystemUser
            {
                UserName = newUser.Username,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber
            };
            string passHash = userManager.PasswordHasher.HashPassword(systemUser, newUser.Password);
            userManager.CreateAsync(systemUser).Wait();
            systemUser.PasswordHash = passHash;

            switch (newUser.Role)
            {
                case UserRoleTypes.Admin:
                    break;
                case UserRoleTypes.Patient:
                    person = new Patient();
                    SetPersonProperties(person, newUser, systemUser);
                    drugstoreDbContext.Patients.Add(person as Patient);
                    break;
                case UserRoleTypes.Doctor:
                    person = new Doctor();
                    SetPersonProperties(person, newUser, systemUser);
                    drugstoreDbContext.Doctors.Add(person as Doctor);
                    break;
                case UserRoleTypes.Nurse:
                    person = new Nurse();
                    SetPersonProperties(person, newUser, systemUser);
                    drugstoreDbContext.Nurses.Add(person as Nurse);
                    break;
                case UserRoleTypes.InternalPharmacist:
                    break;
                case UserRoleTypes.ExternalPharmacist:
                    break;
                default:
                    break;
            }



        }

        private void SetPersonProperties(Person person, NewUserModel newUser, SystemUser systemUser)
        {
            if (person != null)
            {
                person.Department = drugstoreDbContext.Departments.First(d => d.ID == newUser.DepartmentID);
                person.FirstName = newUser.FirstName;
                person.SecondName = newUser.SecondName;
                person.SystemUser = systemUser;
            }
        }
    }
}
