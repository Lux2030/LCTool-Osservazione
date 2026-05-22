using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LCTool.Osservazione.Migrations
{

    public partial class AggiungiOwnerOsservazione : Migration
    {
       
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Osservazioni");
        }
    }
}
