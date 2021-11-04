using Microsoft.EntityFrameworkCore.Migrations;

namespace ContratosMVVM.Migrations
{
    public partial class skdjf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contratos_ContratoBases_ContratoBaseId",
                table: "Contratos");

            migrationBuilder.AddForeignKey(
                name: "FK_Contratos_ContratoBases_ContratoBaseId",
                table: "Contratos",
                column: "ContratoBaseId",
                principalTable: "ContratoBases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contratos_ContratoBases_ContratoBaseId",
                table: "Contratos");

            migrationBuilder.AddForeignKey(
                name: "FK_Contratos_ContratoBases_ContratoBaseId",
                table: "Contratos",
                column: "ContratoBaseId",
                principalTable: "ContratoBases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
