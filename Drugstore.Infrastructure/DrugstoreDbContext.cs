using System;
using Drugstore.Core;
using Microsoft.EntityFrameworkCore;
namespace Drugstore.Infrastructure
{
    public class DrugstoreDbContext: DbContext
    {
        public DrugstoreDbContext(DbContextOptions<DrugstoreDbContext> options)
            :base(options)
        {

        }
        public DbSet<Medicine> Medicines { get; set; }
    }
}
