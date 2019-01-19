using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Drugstore.Infrastructure.Migrations
{
    public partial class Added_New_User_Roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    SecondName = table.Column<string>(maxLength: 50, nullable: true),
                    DepartmentID = table.Column<int>(nullable: true),
                    SystemUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Admins_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admins_AspNetUsers_SystemUserId",
                        column: x => x.SystemUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExternalPharmacists",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    SecondName = table.Column<string>(maxLength: 50, nullable: true),
                    DepartmentID = table.Column<int>(nullable: true),
                    SystemUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalPharmacists", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExternalPharmacists_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalPharmacists_AspNetUsers_SystemUserId",
                        column: x => x.SystemUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InternalPharmacists",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    SecondName = table.Column<string>(maxLength: 50, nullable: true),
                    DepartmentID = table.Column<int>(nullable: true),
                    SystemUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalPharmacists", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InternalPharmacists_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternalPharmacists_AspNetUsers_SystemUserId",
                        column: x => x.SystemUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Storekeepers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    SecondName = table.Column<string>(maxLength: 50, nullable: true),
                    DepartmentID = table.Column<int>(nullable: true),
                    SystemUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storekeepers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Storekeepers_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Storekeepers_AspNetUsers_SystemUserId",
                        column: x => x.SystemUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_DepartmentID",
                table: "Admins",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_SystemUserId",
                table: "Admins",
                column: "SystemUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalPharmacists_DepartmentID",
                table: "ExternalPharmacists",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalPharmacists_SystemUserId",
                table: "ExternalPharmacists",
                column: "SystemUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalPharmacists_DepartmentID",
                table: "InternalPharmacists",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_InternalPharmacists_SystemUserId",
                table: "InternalPharmacists",
                column: "SystemUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Storekeepers_DepartmentID",
                table: "Storekeepers",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Storekeepers_SystemUserId",
                table: "Storekeepers",
                column: "SystemUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "ExternalPharmacists");

            migrationBuilder.DropTable(
                name: "InternalPharmacists");

            migrationBuilder.DropTable(
                name: "Storekeepers");
        }
    }
}
