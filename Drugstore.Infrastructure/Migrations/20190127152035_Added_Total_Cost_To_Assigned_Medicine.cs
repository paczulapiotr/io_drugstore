using Microsoft.EntityFrameworkCore.Migrations;

namespace Drugstore.Infrastructure.Migrations
{
    public partial class Added_Total_Cost_To_Assigned_Medicine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Cost",
                table: "AssignedMedicines",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "AssignedMedicines");
        }
    }
}
