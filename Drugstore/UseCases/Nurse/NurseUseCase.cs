using System;
using System.Collections.Generic;
using System.Linq;
using Drugstore.Core;
using Drugstore.Identity;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Drugstore.Models.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Drugstore.UseCases.Nurse
{
    public class NurseUseCase
    {
        private readonly DrugstoreDbContext context;
        private readonly int pageSize = 5;
        private readonly UserManager<SystemUser> userManager;

        public NurseUseCase(DrugstoreDbContext context,
            UserManager<SystemUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public void AddPatient(UserViewModel newUser)
        {
            var systemUser = new SystemUser
            {
                UserName = newUser.UserName,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber
            };
            var passHash = userManager.PasswordHasher.HashPassword(systemUser, newUser.Password);
            systemUser.PasswordHash = passHash;
            userManager.CreateAsync(systemUser).Wait();

            var role = newUser.Role.ToString();
            userManager.AddToRoleAsync(systemUser, role).Wait();

            var person = new Core.Patient();
            SetPersonProperties(person, newUser, systemUser);
            context.Patients.Add(person);
            context.SaveChanges();
        }

        private void SetPersonProperties(Person person, UserViewModel newUser, SystemUser systemUser)
        {
            if (person != null)
            {
                person.Department = context.Nurses.Include(n => n.Department).FirstOrDefault().Department;
                person.FirstName = newUser.FirstName;
                person.SecondName = newUser.SecondName;
                person.SystemUser = systemUser;
            }
        }

        public PatientViewModel[] GetPatients(string search, Department department)
        {
            var searchPattern = search ?? "";

            var filteredPatients = context.Patients
                .Include(p => p.Department)
                .OrderByDescending(p => p.FullName)
                .Where(p => p.FullName
                                .Contains(searchPattern ?? "", StringComparison.OrdinalIgnoreCase) &&
                            p.Department.ID == department.ID)
                .Take(pageSize);

            var result = filteredPatients.Select(p => AutoMapper.Mapper.Map<PatientViewModel>(p)).ToArray();

            return result;
        }

        public TreatmentHistoryViewModel GeTreatmentHistory(int patientId, int page = 1)
        {
            var patient = context.Patients
                .Include(p => p.TreatmentHistory).ThenInclude(t => t.Doctor)
                .Single(p => p.ID == patientId);

            var prescriptions = patient.TreatmentHistory
                .OrderByDescending(p => p.CreationTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var totalPages = (int) Math.Ceiling((float) patient.TreatmentHistory.Count() / pageSize);

            var requestTemplate = "/Nurse/TreatmentHistory?patientId=" + patientId + "&page={0}";

            return new TreatmentHistoryViewModel
            {
                Pagination = new PaginationViewModel(requestTemplate, totalPages, page),
                Prescriptions = prescriptions.Select(
                        p => AutoMapper.Mapper.Map<PrescriptionViewModel>(p))
                    .ToList()
            };
        }

        public IEnumerable<PrescriptionViewModel> GetAllPrescriptions(int patientId)
        {
            var patient = context.Patients
                .Include(p => p.TreatmentHistory).ThenInclude(t => t.Doctor)
                .Single(p => p.ID == patientId);

            var prescriptions = patient.TreatmentHistory
                .OrderByDescending(p => p.CreationTime);

            var prescriptionsViewModels = prescriptions.Select(
                    p => AutoMapper.Mapper.Map<PrescriptionViewModel>(p))
                .ToList();

            foreach (var prescriptionsViewModel in prescriptionsViewModels)
            {
                var prescriptionId = prescriptionsViewModel.Id;
                var medicalPrescription = context.MedicalPrescriptions
                    .Include(p => p.Patient)
                    .Include(p => p.Doctor)
                    .Include(p => p.Medicines).ThenInclude(m => m.StockMedicine)
                    .SingleOrDefault(p => p.ID == prescriptionId);
                yield return AutoMapper.Mapper.Map<PrescriptionViewModel>(medicalPrescription);
            }
        }

        public PrescriptionViewModel GetPrescriptionDetails(int prescriptionId)
        {
            var prescription = context.MedicalPrescriptions
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .Include(p => p.Medicines).ThenInclude(m => m.StockMedicine)
                .SingleOrDefault(p => p.ID == prescriptionId);

            if (prescription == null)
            {
                return new PrescriptionViewModel();
            }

            var result = AutoMapper.Mapper.Map<PrescriptionViewModel>(prescription);

            return result;
        }
    }
}