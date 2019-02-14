using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Drugstore.Core;
using Drugstore.Identity;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Drawing;

namespace Drugstore.UseCases.Nurse
{
    public class NurseUseCase
    {
        private readonly DrugstoreDbContext context;
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

        public MemoryStream PreparePdf(IEnumerable<MedicalPrescription> prescriptions)
        {
            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            var graph = XGraphics.FromPdfPage(page);
            var font = new XFont("Verdana", 20, XFontStyle.Regular);

            graph.DrawString("This is my first PDF document", font, XBrushes.Black,
                new XRect(0, 0, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

            MemoryStream stream = null;
        }
    }
}