using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporSalonu.Migrations
{
    /// <inheritdoc />
    public partial class UyelikBitisEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UyelikBitisTarihi",
                table: "Uyeler",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UyelikBitisTarihi",
                table: "Uyeler");
        }
    }
}
