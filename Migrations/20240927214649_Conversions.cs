using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConversionApplication.Migrations
{
    /// <inheritdoc />
    public partial class Conversions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Energy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Joules = table.Column<double>(type: "REAL", nullable: false),
                    Calories = table.Column<double>(type: "REAL", nullable: false),
                    KilowattHours = table.Column<double>(type: "REAL", nullable: false),
                    BTU = table.Column<double>(type: "REAL", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Energy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Length",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Millimeters = table.Column<double>(type: "REAL", nullable: false),
                    Meters = table.Column<double>(type: "REAL", nullable: false),
                    Inches = table.Column<double>(type: "REAL", nullable: false),
                    Feet = table.Column<double>(type: "REAL", nullable: false),
                    Yards = table.Column<double>(type: "REAL", nullable: false),
                    Miles = table.Column<double>(type: "REAL", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Length", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Temperature",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Celsius = table.Column<double>(type: "REAL", nullable: false),
                    Fahrenheit = table.Column<double>(type: "REAL", nullable: false),
                    Kelvin = table.Column<double>(type: "REAL", nullable: false),
                    Reomur = table.Column<double>(type: "REAL", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temperature", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weight",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Grams = table.Column<double>(type: "REAL", nullable: false),
                    Ounces = table.Column<double>(type: "REAL", nullable: false),
                    Karats = table.Column<double>(type: "REAL", nullable: false),
                    Pounds = table.Column<double>(type: "REAL", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weight", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Energy");

            migrationBuilder.DropTable(
                name: "Length");

            migrationBuilder.DropTable(
                name: "Temperature");

            migrationBuilder.DropTable(
                name: "Weight");
        }
    }
}
