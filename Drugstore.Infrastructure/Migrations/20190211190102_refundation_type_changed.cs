using Microsoft.EntityFrameworkCore.Migrations;

namespace Drugstore.Infrastructure.Migrations
{
    public partial class refundation_type_changed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRefunded",
                table: "Medicines");

            migrationBuilder.AlterColumn<double>(
                name: "PricePerOne",
                table: "Medicines",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Medicines",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Refundation",
                table: "Medicines",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "PricePerOne",
                table: "ExternalDrugstoreSoldMedicines",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "PricePerOne",
                table: "ExternalDrugstoreMedicines",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "PricePerOne",
                table: "AssignedMedicines",
                nullable: false,
                oldClrType: typeof(float));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Refundation",
                table: "Medicines");

            migrationBuilder.AlterColumn<float>(
                name: "PricePerOne",
                table: "Medicines",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Medicines",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddColumn<bool>(
                name: "IsRefunded",
                table: "Medicines",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<float>(
                name: "PricePerOne",
                table: "ExternalDrugstoreSoldMedicines",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "PricePerOne",
                table: "ExternalDrugstoreMedicines",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "PricePerOne",
                table: "AssignedMedicines",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
