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

        public DbSet<Doctor> Doctors{ get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<MedicineOnStock> Medicines { get; set; }

    }
}
