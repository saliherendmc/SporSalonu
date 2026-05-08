using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporSalonu.Migrations
{
    /// <inheritdoc />
    public partial class OlcumTabloGuncelleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "YagOrani",
                table: "Olcumler",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Boy",
                table: "Olcumler",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<double>(
                name: "BelCevresi",
                table: "Olcumler",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GogusCevresi",
                table: "Olcumler",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "KolCevresi",
                table: "Olcumler",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BelCevresi",
                table: "Olcumler");

            migrationBuilder.DropColumn(
                name: "GogusCevresi",
                table: "Olcumler");

            migrationBuilder.DropColumn(
                name: "KolCevresi",
                table: "Olcumler");

            migrationBuilder.AlterColumn<double>(
                name: "YagOrani",
                table: "Olcumler",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Boy",
                table: "Olcumler",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
