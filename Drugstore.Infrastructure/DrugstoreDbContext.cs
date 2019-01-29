using Drugstore.Core;
using Drugstore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Drugstore.Infrastructure
{
    public class DrugstoreDbContext : IdentityDbContext<SystemUser>
    {
        public DrugstoreDbContext(DbContextOptions<DrugstoreDbContext> options)
            : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Doctor> Doctors{ get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<ExternalPharmacist> ExternalPharmacists { get; set; }
        public DbSet<InternalPharmacist> InternalPharmacists { get; set; }
        public DbSet<Storekeeper> Storekeepers{ get; set; }
        public DbSet<Department> Departments { get; set; }

        //Internal Drugstore
        public DbSet<MedicineOnStock> Medicines { get; set; }
        public DbSet<MedicalPrescription> MedicalPrescriptions { get; set; }
        public DbSet<AssignedMedicine> AssignedMedicines { get; set; }

        //External Drugstore
        public DbSet<ExternalDrugstoreMedicine> ExternalDrugstoreMedicines { get; set; }
        public DbSet<ExternalDrugstoreSoldMedicine> ExternalDrugstoreSoldMedicines { get; set; }
    }
}
