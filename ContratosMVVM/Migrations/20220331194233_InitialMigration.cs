using Microsoft.EntityFrameworkCore.Migrations;

namespace ContratosMVVM.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Observacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Texto = table.Column<string>(type: "TEXT", nullable: true),
                    FirebirdId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Setors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Setor = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContratoBases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SetorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Descrição = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratoBases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContratoBases_Setors_SetorId",
                        column: x => x.SetorId,
                        principalTable: "Setors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contratos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirebirdIDCliente = table.Column<int>(type: "INTEGER", nullable: false),
                    ValorUnitário = table.Column<decimal>(type: "TEXT", nullable: false),
                    Quantidade = table.Column<decimal>(type: "TEXT", nullable: false),
                    ContratoBaseId = table.Column<int>(type: "INTEGER", nullable: false),
                    Vigência = table.Column<int>(type: "INTEGER", nullable: false),
                    ContratoPDF = table.Column<string>(type: "TEXT", nullable: true),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contratos_ContratoBases_ContratoBaseId",
                        column: x => x.ContratoBaseId,
                        principalTable: "ContratoBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContratoBases_SetorId",
                table: "ContratoBases",
                column: "SetorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_ContratoBaseId",
                table: "Contratos",
                column: "ContratoBaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contratos");

            migrationBuilder.DropTable(
                name: "Observacoes");

            migrationBuilder.DropTable(
                name: "ContratoBases");

            migrationBuilder.DropTable(
                name: "Setors");
        }
    }
}
