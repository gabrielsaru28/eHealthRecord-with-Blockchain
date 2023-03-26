using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorFullStackCrud.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Allergies",
                columns: table => new
                {
                    AllergyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllergyName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergies", x => x.AllergyId);
                });

            migrationBuilder.CreateTable(
                name: "HealthRecords",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Medications = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllergyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthRecords", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_HealthRecords_Allergies_AllergyId",
                        column: x => x.AllergyId,
                        principalTable: "Allergies",
                        principalColumn: "AllergyId");
                });

            migrationBuilder.InsertData(
                table: "Allergies",
                columns: new[] { "AllergyId", "AllergyName" },
                values: new object[,]
                {
                    { 1, "Alergie la praf" },
                    { 2, "Alergie la lactoza" },
                    { 3, "Alergie la capsuni" }
                });

            migrationBuilder.InsertData(
                table: "HealthRecords",
                columns: new[] { "PatientId", "AllergyId", "MedicalHistory", "Medications", "PatientName" },
                values: new object[,]
                {
                    { 1, 1, "Healthy", "Pastile test", "Ion" },
                    { 2, 2, "Healthy", "Pastile lactoza", "John" },
                    { 3, 3, "Very Healthy", "Pastile1212 lactoza", "Mark" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_AllergyId",
                table: "HealthRecords",
                column: "AllergyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthRecords");

            migrationBuilder.DropTable(
                name: "Allergies");
        }
    }
}
