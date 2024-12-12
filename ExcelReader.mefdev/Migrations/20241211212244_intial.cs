using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExcelReader.mefdev.Migrations
{
    /// <inheritdoc />
    public partial class intial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Scenario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Feb = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Mar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Apr = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    May = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Jun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Jul = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Aug = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Sep = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Oct = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Nov = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dec = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialData");
        }
    }
}
