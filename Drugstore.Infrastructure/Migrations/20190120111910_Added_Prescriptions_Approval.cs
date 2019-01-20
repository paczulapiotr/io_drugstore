using Microsoft.EntityFrameworkCore.Migrations;

namespace Drugstore.Infrastructure.Migrations
{
    public partial class Added_Prescriptions_Approval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "MedicalPrescriptions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "MedicalPrescriptions");
        }
    }
}
