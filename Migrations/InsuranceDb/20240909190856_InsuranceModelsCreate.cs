using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceManagementApp.Migrations.InsuranceDb
{
    /// <inheritdoc />
    public partial class InsuranceModelsCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PolicyHolders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    TelephoneNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyHolders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Insurances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PolicyHolderId = table.Column<int>(type: "INTEGER", nullable: false),
                    InsuranceType = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<double>(type: "REAL", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insurances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Insurances_PolicyHolders_PolicyHolderId",
                        column: x => x.PolicyHolderId,
                        principalTable: "PolicyHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PolicyHolderId = table.Column<int>(type: "INTEGER", nullable: false),
                    InsuranceId = table.Column<int>(type: "INTEGER", nullable: false),
                    EventDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    EventStatus = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsuranceEvents_Insurances_InsuranceId",
                        column: x => x.InsuranceId,
                        principalTable: "Insurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsuranceEvents_PolicyHolders_PolicyHolderId",
                        column: x => x.PolicyHolderId,
                        principalTable: "PolicyHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceEvents_InsuranceId",
                table: "InsuranceEvents",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceEvents_PolicyHolderId",
                table: "InsuranceEvents",
                column: "PolicyHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Insurances_PolicyHolderId",
                table: "Insurances",
                column: "PolicyHolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsuranceEvents");

            migrationBuilder.DropTable(
                name: "Insurances");

            migrationBuilder.DropTable(
                name: "PolicyHolders");
        }
    }
}
