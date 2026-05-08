using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporSalonu.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sifre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UyelikPaketleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaketAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KacAylikPaket = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ucret = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UyelikPaketleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uyeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UyelikPaketiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uyeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Uyeler_UyelikPaketleri_UyelikPaketiId",
                        column: x => x.UyelikPaketiId,
                        principalTable: "UyelikPaketleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AntrenmanProgramlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UyeId = table.Column<int>(type: "int", nullable: false),
                    Gun = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bolge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HareketAdi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Set = table.Column<int>(type: "int", nullable: false),
                    Tekrar = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntrenmanProgramlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntrenmanProgramlari_Uyeler_UyeId",
                        column: x => x.UyeId,
                        principalTable: "Uyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GirisKayitlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UyeId = table.Column<int>(type: "int", nullable: false),
                    GirisZamani = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GirisKayitlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GirisKayitlari_Uyeler_UyeId",
                        column: x => x.UyeId,
                        principalTable: "Uyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Olcumler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UyeId = table.Column<int>(type: "int", nullable: false),
                    Kilo = table.Column<double>(type: "float", nullable: false),
                    Boy = table.Column<double>(type: "float", nullable: false),
                    YagOrani = table.Column<double>(type: "float", nullable: false),
                    OlcumTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Olcumler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Olcumler_Uyeler_UyeId",
                        column: x => x.UyeId,
                        principalTable: "Uyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AntrenmanProgramlari_UyeId",
                table: "AntrenmanProgramlari",
                column: "UyeId");

            migrationBuilder.CreateIndex(
                name: "IX_GirisKayitlari_UyeId",
                table: "GirisKayitlari",
                column: "UyeId");

            migrationBuilder.CreateIndex(
                name: "IX_Olcumler_UyeId",
                table: "Olcumler",
                column: "UyeId");

            migrationBuilder.CreateIndex(
                name: "IX_Uyeler_UyelikPaketiId",
                table: "Uyeler",
                column: "UyelikPaketiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AntrenmanProgramlari");

            migrationBuilder.DropTable(
                name: "GirisKayitlari");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Olcumler");

            migrationBuilder.DropTable(
                name: "Uyeler");

            migrationBuilder.DropTable(
                name: "UyelikPaketleri");
        }
    }
}
