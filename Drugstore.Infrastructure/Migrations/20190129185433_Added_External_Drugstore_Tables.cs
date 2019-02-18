using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Drugstore.Infrastructure.Migrations
{
    public partial class Added_External_Drugstore_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "AssignedMedicines",
                newName: "PricePerOne");

            migrationBuilder.CreateTable(
                name: "ExternalDrugstoreMedicines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    StockMedicineID = table.Column<int>(nullable: false),
                    Quantity = table.Column<long>(nullable: false),
                    PricePerOne = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalDrugstoreMedicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalDrugstoreMedicines_Medicines_StockMedicineID",
                        column: x => x.StockMedicineID,
                        principalTable: "Medicines",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalDrugstoreSoldMedicines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StockMedicineID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    SoldQuantity = table.Column<int>(nullable: false),
                    PricePerOne = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalDrugstoreSoldMedicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalDrugstoreSoldMedicines_Medicines_StockMedicineID",
                        column: x => x.StockMedicineID,
                        principalTable: "Medicines",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalDrugstoreMedicines_StockMedicineID",
                table: "ExternalDrugstoreMedicines",
                column: "StockMedicineID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalDrugstoreSoldMedicines_StockMedicineID",
                table: "ExternalDrugstoreSoldMedicines",
                column: "StockMedicineID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalDrugstoreMedicines");

            migrationBuilder.DropTable(
                name: "ExternalDrugstoreSoldMedicines");

            migrationBuilder.RenameColumn(
                name: "PricePerOne",
                table: "AssignedMedicines",
                newName: "Cost");
        }
    }
}
