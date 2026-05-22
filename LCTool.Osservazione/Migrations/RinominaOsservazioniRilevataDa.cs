using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LCTool.Osservazione.Migrations
{
    
    public partial class RinominaOsservazioniRilevataDa : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "OsservazioniRilevataDa",
                newName: "OsservazioneRilevataDa");
        }
    }
}
