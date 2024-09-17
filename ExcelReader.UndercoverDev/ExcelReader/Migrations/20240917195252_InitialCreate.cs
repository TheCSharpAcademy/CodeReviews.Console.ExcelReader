using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExcelReader.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataModels",
                columns: table => new
                {
                    Date = table.Column<string>(type: "TEXT", nullable: false),
                    League = table.Column<string>(type: "TEXT", nullable: false),
                    Home = table.Column<string>(type: "TEXT", nullable: false),
                    Away = table.Column<string>(type: "TEXT", nullable: false),
                    HomeProbability = table.Column<string>(type: "TEXT", nullable: false),
                    AwayProbability = table.Column<string>(type: "TEXT", nullable: false),
                    OverTwoGoals = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataModels");
        }
    }
}
