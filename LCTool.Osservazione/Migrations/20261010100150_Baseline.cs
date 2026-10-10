using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LCTool.Osservazione.Migrations
{
    /// <inheritdoc />
    public partial class Baseline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Osservazioni",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stato = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsoRiferimento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemRiferimento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RilevataDa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataApertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataPrevistaChiusura = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataEffettivaChiusura = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Descrizione = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Causa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AzioniCorrettive = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocSgqModificati = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataRevisione = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoteRevisione = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatoDa = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ModificatoDa = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RevisionatoDa = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Osservazioni", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OsservazioniRilevataDa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Valore = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlagAttivo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OsservazioniRilevataDa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OsservazioniStati",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlagAttivo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OsservazioniStati", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Osservazioni",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OsservazioniRilevataDa");

            migrationBuilder.DropTable(
                name: "OsservazioniStati");
        }
    }
}
