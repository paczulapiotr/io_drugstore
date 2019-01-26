using Microsoft.EntityFrameworkCore.Migrations;

namespace Drugstore.Infrastructure.Migrations
{
    public partial class Added_VerificationState_For_Prescriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "MedicalPrescriptions");

            migrationBuilder.AddColumn<int>(
                name: "VerificationState",
                table: "MedicalPrescriptions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationState",
                table: "MedicalPrescriptions");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "MedicalPrescriptions",
                nullable: false,
                defaultValue: false);
        }
    }
}
