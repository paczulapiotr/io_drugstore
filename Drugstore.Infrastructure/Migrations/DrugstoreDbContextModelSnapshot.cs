﻿// <auto-generated />
using System;
using Drugstore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Drugstore.Infrastructure.Migrations
{
    [DbContext(typeof(DrugstoreDbContext))]
    partial class DrugstoreDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Drugstore.Core.Admin", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DepartmentID");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SystemUserId");

                    b.HasKey("ID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("SystemUserId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("Drugstore.Core.AssignedMedicine", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AssignedQuantity");

                    b.Property<int?>("MedicalPrescriptionID");

                    b.Property<double>("PricePerOne");

                    b.Property<int?>("StockMedicineID");

                    b.HasKey("ID");

                    b.HasIndex("MedicalPrescriptionID");

                    b.HasIndex("StockMedicineID");

                    b.ToTable("AssignedMedicines");
                });

            modelBuilder.Entity("Drugstore.Core.Department", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Drugstore.Core.Doctor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DepartmentID");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SystemUserId");

                    b.HasKey("ID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("SystemUserId");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("Drugstore.Core.ExternalDrugstoreMedicine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<double>("PricePerOne");

                    b.Property<long>("Quantity");

                    b.Property<int>("StockMedicineID");

                    b.HasKey("Id");

                    b.HasIndex("StockMedicineID");

                    b.ToTable("ExternalDrugstoreMedicines");
                });

            modelBuilder.Entity("Drugstore.Core.ExternalDrugstoreSoldMedicine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<double>("PricePerOne");

                    b.Property<int>("SoldQuantity");

                    b.Property<int>("StockMedicineID");

                    b.HasKey("Id");

                    b.HasIndex("StockMedicineID");

                    b.ToTable("ExternalDrugstoreSoldMedicines");
                });

            modelBuilder.Entity("Drugstore.Core.ExternalPharmacist", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DepartmentID");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SystemUserId");

                    b.HasKey("ID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("SystemUserId");

                    b.ToTable("ExternalPharmacists");
                });

            modelBuilder.Entity("Drugstore.Core.InternalPharmacist", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DepartmentID");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SystemUserId");

                    b.HasKey("ID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("SystemUserId");

                    b.ToTable("InternalPharmacists");
                });

            modelBuilder.Entity("Drugstore.Core.MedicalPrescription", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationTime");

                    b.Property<int>("DoctorID");

                    b.Property<int>("PatientID");

                    b.Property<int>("VerificationState");

                    b.HasKey("ID");

                    b.HasIndex("DoctorID");

                    b.HasIndex("PatientID");

                    b.ToTable("MedicalPrescriptions");
                });

            modelBuilder.Entity("Drugstore.Core.MedicineOnStock", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MedicineCategory");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<double>("PricePerOne");

                    b.Property<long>("Quantity");

                    b.Property<double>("Refundation");

                    b.HasKey("ID");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("Drugstore.Core.Nurse", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DepartmentID");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SystemUserId");

                    b.HasKey("ID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("SystemUserId");

                    b.ToTable("Nurses");
                });

            modelBuilder.Entity("Drugstore.Core.Patient", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DepartmentID");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SystemUserId");

                    b.HasKey("ID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("SystemUserId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("Drugstore.Core.Storekeeper", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DepartmentID");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SystemUserId");

                    b.HasKey("ID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("SystemUserId");

                    b.ToTable("Storekeepers");
                });

            modelBuilder.Entity("Drugstore.Identity.SystemUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Drugstore.Core.Admin", b =>
                {
                    b.HasOne("Drugstore.Core.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentID");

                    b.HasOne("Drugstore.Identity.SystemUser", "SystemUser")
                        .WithMany()
                        .HasForeignKey("SystemUserId");
                });

            modelBuilder.Entity("Drugstore.Core.AssignedMedicine", b =>
                {
                    b.HasOne("Drugstore.Core.MedicalPrescription")
                        .WithMany("Medicines")
                        .HasForeignKey("MedicalPrescriptionID");

                    b.HasOne("Drugstore.Core.MedicineOnStock", "StockMedicine")
                        .WithMany()
                        .HasForeignKey("StockMedicineID");
                });

            modelBuilder.Entity("Drugstore.Core.Doctor", b =>
                {
                    b.HasOne("Drugstore.Core.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentID");

                    b.HasOne("Drugstore.Identity.SystemUser", "SystemUser")
                        .WithMany()
                        .HasForeignKey("SystemUserId");
                });

            modelBuilder.Entity("Drugstore.Core.ExternalDrugstoreMedicine", b =>
                {
                    b.HasOne("Drugstore.Core.MedicineOnStock", "StockMedicine")
                        .WithMany()
                        .HasForeignKey("StockMedicineID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Drugstore.Core.ExternalDrugstoreSoldMedicine", b =>
                {
                    b.HasOne("Drugstore.Core.MedicineOnStock", "StockMedicine")
                        .WithMany()
                        .HasForeignKey("StockMedicineID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Drugstore.Core.ExternalPharmacist", b =>
                {
                    b.HasOne("Drugstore.Core.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentID");

                    b.HasOne("Drugstore.Identity.SystemUser", "SystemUser")
                        .WithMany()
                        .HasForeignKey("SystemUserId");
                });

            modelBuilder.Entity("Drugstore.Core.InternalPharmacist", b =>
                {
                    b.HasOne("Drugstore.Core.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentID");

                    b.HasOne("Drugstore.Identity.SystemUser", "SystemUser")
                        .WithMany()
                        .HasForeignKey("SystemUserId");
                });

            modelBuilder.Entity("Drugstore.Core.MedicalPrescription", b =>
                {
                    b.HasOne("Drugstore.Core.Doctor", "Doctor")
                        .WithMany("IssuedPresciptions")
                        .HasForeignKey("DoctorID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Drugstore.Core.Patient", "Patient")
                        .WithMany("TreatmentHistory")
                        .HasForeignKey("PatientID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Drugstore.Core.Nurse", b =>
                {
                    b.HasOne("Drugstore.Core.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentID");

                    b.HasOne("Drugstore.Identity.SystemUser", "SystemUser")
                        .WithMany()
                        .HasForeignKey("SystemUserId");
                });

            modelBuilder.Entity("Drugstore.Core.Patient", b =>
                {
                    b.HasOne("Drugstore.Core.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentID");

                    b.HasOne("Drugstore.Identity.SystemUser", "SystemUser")
                        .WithMany()
                        .HasForeignKey("SystemUserId");
                });

            modelBuilder.Entity("Drugstore.Core.Storekeeper", b =>
                {
                    b.HasOne("Drugstore.Core.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentID");

                    b.HasOne("Drugstore.Identity.SystemUser", "SystemUser")
                        .WithMany()
                        .HasForeignKey("SystemUserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Drugstore.Identity.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Drugstore.Identity.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Drugstore.Identity.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Drugstore.Identity.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
