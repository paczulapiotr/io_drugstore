using Drugstore.Core;
using Drugstore.Identity;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Drugstore.Data
{
    public class DrugstoreRepository : IRepository
    {
        private readonly UserManager<SystemUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly DrugstoreDbContext context;

        public DrugstoreRepository(
            UserManager<SystemUser> userManager,
            RoleManager<IdentityRole> roleManager,
            DrugstoreDbContext drugstoreDbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            context = drugstoreDbContext;
        }


        public void DeleteUser(string userId)
        {
            var user = userManager.FindByIdAsync(userId).Result;
            var role = userManager.GetRolesAsync(user).Result.First();
            if (Enum.TryParse(typeof(UserRoleTypes), role, true, out object result))
            {
                var userRole = (UserRoleTypes)result;
                switch (userRole)
                {
                    case UserRoleTypes.Admin:
                        DeletePerson(context.Admins, userId);
                        break;
                    case UserRoleTypes.Patient:
                        DeletePerson(context.Patients, userId);
                        break;
                    case UserRoleTypes.Doctor:
                        DeletePerson(context.Doctors, userId);
                        break;
                    case UserRoleTypes.Nurse:
                        DeletePerson(context.Nurses, userId);
                        break;
                    case UserRoleTypes.InternalPharmacist:
                        DeletePerson(context.InternalPharmacists, userId);
                        break;
                    case UserRoleTypes.ExternalPharmacist:
                        DeletePerson(context.ExternalPharmacists, userId);
                        break;
                    case UserRoleTypes.Storekeeper:
                        DeletePerson(context.Storekeepers, userId);
                        break;
                    default:
                        break;
                }

                userManager.DeleteAsync(user).Wait();
            }

        }
        private void DeletePerson<Type>(DbSet<Type> people, string userId) where Type : Person
        {
            var person = people.FirstOrDefault(p => p.SystemUser.Id == userId);
            var entry = context.Entry<Type>(person);
            entry.State = EntityState.Deleted;
            context.SaveChanges();
        }

        public void EditUser(UserModel updatedUser)
        {
            string userId = updatedUser.SystemUserId;
            var user = userManager.FindByIdAsync(userId).Result;
            switch (updatedUser.Role)
            {
                case UserRoleTypes.Admin:
                    EditPerson(context.Admins, updatedUser, userId);
                    break;
                case UserRoleTypes.Patient:
                    EditPerson(context.Patients, updatedUser, userId);
                    break;
                case UserRoleTypes.Doctor:
                    EditPerson(context.Doctors, updatedUser, userId);
                    break;
                case UserRoleTypes.Nurse:
                    EditPerson(context.Nurses, updatedUser, userId);
                    break;
                case UserRoleTypes.InternalPharmacist:
                    EditPerson(context.InternalPharmacists, updatedUser, userId);
                    break;
                case UserRoleTypes.ExternalPharmacist:
                    EditPerson(context.ExternalPharmacists, updatedUser, userId);
                    break;
                case UserRoleTypes.Storekeeper:
                    EditPerson(context.Storekeepers, updatedUser, userId);
                    break;
                default:
                    break;
            }

            user.UserName = updatedUser.UserName;
            user.PhoneNumber = updatedUser.PhoneNumber;
            if (!String.IsNullOrEmpty(updatedUser.Password))
            {
                user.PasswordHash = userManager.PasswordHasher
                .HashPassword(user, updatedUser.Password);
            }

            userManager.UpdateAsync(user).Wait();
        }
        private void EditPerson<Type>(DbSet<Type> people, UserModel updatedUser, string userId) where Type : Person
        {
            var person = people.First(p => p.SystemUser.Id == userId);
            person.FirstName = updatedUser.FirstName;
            person.SecondName = updatedUser.SecondName;
            var department = context.Departments.First(d => d.ID == updatedUser.DepartmentID);
            person.Department = department;
            context.SaveChanges();
        }

        public void AddNewUser(UserModel newUser)
        {
            Person person = null;
            SystemUser systemUser = new SystemUser
            {
                UserName = newUser.UserName,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber
            };
            string passHash = userManager.PasswordHasher.HashPassword(systemUser, newUser.Password);
            systemUser.PasswordHash = passHash;
            userManager.CreateAsync(systemUser).Wait();

            string role = newUser.Role.ToString();
            userManager.AddToRoleAsync(systemUser, role).Wait();

            switch (newUser.Role)
            {
                case UserRoleTypes.Admin:
                    person = new Admin();
                    SetPersonProperties(person, newUser, systemUser);
                    context.Admins.Add(person as Admin);
                    break;
                case UserRoleTypes.Patient:
                    person = new Patient();
                    SetPersonProperties(person, newUser, systemUser);
                    context.Patients.Add(person as Patient);
                    break;
                case UserRoleTypes.Doctor:
                    person = new Doctor();
                    SetPersonProperties(person, newUser, systemUser);
                    context.Doctors.Add(person as Doctor);
                    break;
                case UserRoleTypes.Nurse:
                    person = new Nurse();
                    SetPersonProperties(person, newUser, systemUser);
                    context.Nurses.Add(person as Nurse);
                    break;
                case UserRoleTypes.InternalPharmacist:
                    person = new InternalPharmacist();
                    SetPersonProperties(person, newUser, systemUser);
                    context.InternalPharmacists.Add(person as InternalPharmacist);
                    break;
                case UserRoleTypes.ExternalPharmacist:
                    person = new ExternalPharmacist();
                    SetPersonProperties(person, newUser, systemUser);
                    context.ExternalPharmacists.Add(person as ExternalPharmacist);
                    break;
                case UserRoleTypes.Storekeeper:
                    person = new Storekeeper();
                    SetPersonProperties(person, newUser, systemUser);
                    context.Storekeepers.Add(person as Storekeeper);
                    break;
                default:
                    throw new Exception("Unknown user type when creating new user!");
            }
            context.SaveChanges();

        }

        private void SetPersonProperties(Person person, UserModel newUser, SystemUser systemUser)
        {
            if (person != null)
            {
                person.Department = context.Departments.First(d => d.ID == newUser.DepartmentID);
                person.FirstName = newUser.FirstName;
                person.SecondName = newUser.SecondName;
                person.SystemUser = systemUser;
            }
        }

        public UserModel GetUser(string userId)
        {

            var systemUser = userManager.FindByIdAsync(userId).Result;
            string role = userManager.GetRolesAsync(systemUser).Result.First();

            var roleType = (UserRoleTypes)Enum.Parse(typeof(UserRoleTypes), role, true);
            Person person = null;
            switch (roleType)
            {
                case UserRoleTypes.Admin:
                    person = context.Admins
                        .Include(a => a.Department)
                        .Include(a => a.SystemUser)
                        .First(a => a.SystemUser.Id == userId);
                    break;
                case UserRoleTypes.Patient:
                    person = context.Patients
                        .Include(a => a.Department)
                        .Include(a => a.SystemUser)
                        .First(a => a.SystemUser.Id == userId);
                    break;
                case UserRoleTypes.Doctor:
                    person = context.Doctors
                        .Include(a => a.Department)
                        .Include(a => a.SystemUser)
                        .First(a => a.SystemUser.Id == userId);
                    break;
                case UserRoleTypes.Nurse:
                    person = context.Nurses
                        .Include(a => a.Department)
                        .Include(a => a.SystemUser)
                        .First(a => a.SystemUser.Id == userId);
                    break;
                case UserRoleTypes.InternalPharmacist:
                    person = context.InternalPharmacists
                        .Include(a => a.Department)
                        .Include(a => a.SystemUser)
                        .First(a => a.SystemUser.Id == userId);
                    break;
                case UserRoleTypes.ExternalPharmacist:
                    person = context.ExternalPharmacists
                        .Include(a => a.Department)
                        .Include(a => a.SystemUser)
                        .First(a => a.SystemUser.Id == userId);
                    break;
                case UserRoleTypes.Storekeeper:
                    person = context.Storekeepers
                        .Include(a => a.Department)
                        .Include(a => a.SystemUser)
                        .First(a => a.SystemUser.Id == userId);
                    break;
                default:
                    break;
            }

            return new UserModel
            {
                FirstName = person.FirstName,
                SecondName = person.SecondName,
                Password = "",
                ConfirmPassword = "",
                DepartmentID = person.Department.ID,
                Email = systemUser.Email,
                PhoneNumber = systemUser.PhoneNumber,
                Role = roleType,
                UserName = systemUser.UserName,
                SystemUserId = systemUser.Id
            };

        }
    }
}
