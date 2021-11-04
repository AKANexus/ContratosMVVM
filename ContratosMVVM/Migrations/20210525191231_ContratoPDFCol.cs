using Microsoft.EntityFrameworkCore.Migrations;

namespace ContratosMVVM.Migrations
{
    public partial class ContratoPDFCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contratos_CLIENTE_ClienteId",
                table: "Contratos");

            migrationBuilder.DropTable(
                name: "CLIENTE");

            migrationBuilder.DropIndex(
                name: "IX_Contratos_ClienteId",
                table: "Contratos");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Contratos");

            migrationBuilder.AlterColumn<string>(
                name: "Setor",
                table: "Setors",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Setors",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Vigência",
                table: "Contratos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorUnitário",
                table: "Contratos",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantidade",
                table: "Contratos",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<int>(
                name: "FirebirdIDCliente",
                table: "Contratos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ContratoBaseId",
                table: "Contratos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Contratos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "ContratoPDF",
                table: "Contratos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SetorId",
                table: "ContratoBases",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "ContratoBases",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descrição",
                table: "ContratoBases",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ContratoBases",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContratoPDF",
                table: "Contratos");

            migrationBuilder.AlterColumn<string>(
                name: "Setor",
                table: "Setors",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Setors",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Vigência",
                table: "Contratos",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorUnitário",
                table: "Contratos",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantidade",
                table: "Contratos",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "FirebirdIDCliente",
                table: "Contratos",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "ContratoBaseId",
                table: "Contratos",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Contratos",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Contratos",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SetorId",
                table: "ContratoBases",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "ContratoBases",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descrição",
                table: "ContratoBases",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ContratoBases",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateTable(
                name: "CLIENTE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Bairro = table.Column<string>(type: "longtext", nullable: true),
                    CNPJCPF = table.Column<string>(type: "longtext", nullable: true),
                    Cidade = table.Column<string>(type: "longtext", nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: true),
                    Endereço = table.Column<string>(type: "longtext", nullable: true),
                    Estado = table.Column<string>(type: "longtext", nullable: true),
                    IDFirebird = table.Column<int>(type: "int", nullable: false),
                    RazãoSocial = table.Column<string>(type: "longtext", nullable: true),
                    Telefone = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENTE", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_ClienteId",
                table: "Contratos",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contratos_CLIENTE_ClienteId",
                table: "Contratos",
                column: "ClienteId",
                principalTable: "CLIENTE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
