using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Drugstore.Core;
using Drugstore.Identity;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Drugstore.Models.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Drugstore.UseCases.Nurse
{
    public class NurseUseCase
    {
        private readonly DrugstoreDbContext context;
        private readonly int pageSize = 5;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<SystemUser> userManager;

        public NurseUseCase(DrugstoreDbContext context, RoleManager<IdentityRole> roleManager,
            UserManager<SystemUser> userManager)
        {
            this.context = context;
            this.roleManager = roleManager;
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

        public Stream PreparePdf(TreatmentHistoryViewModel prescriptions)
        {
            var document = new PdfDocument();
            document.Info.Title = "Historia leczenia";
            document.Info.Author = "Szybka Pigula";
            var page = document.AddPage();

            if (prescriptions.Prescriptions != null && prescriptions.Prescriptions.Any())
            {
                var patient = prescriptions.Prescriptions.First().PatientName;
                document.Info.Title += " " + patient;
                var font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

                var graphics = XGraphics.FromPdfPage(page);
                var i = 25f;

                foreach (var prescription in prescriptions.Prescriptions)
                {
                    graphics.DrawString(prescription.ToString(),
                        font,
                        XBrushes.Red,
                        new XPoint(0f, i),
                        XStringFormats.BaseLineLeft);
                    i += 25;
                }
            }

            var saveStream = new MemoryStream();
            document.Save(saveStream);
            return saveStream;
        }
    }
}