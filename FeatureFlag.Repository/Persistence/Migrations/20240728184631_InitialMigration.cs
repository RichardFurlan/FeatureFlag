using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Repository.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consumidores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Identificacao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Inativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumidores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recursos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Identificacao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Inativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recursos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecursosConsumidores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoRecurso = table.Column<int>(type: "integer", nullable: false),
                    CodigoConsumidor = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ConsumidoresId = table.Column<int>(type: "integer", nullable: true),
                    RecursosId = table.Column<int>(type: "integer", nullable: true),
                    Inativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecursosConsumidores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecursosConsumidores_Consumidores_ConsumidoresId",
                        column: x => x.ConsumidoresId,
                        principalTable: "Consumidores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecursosConsumidores_Recursos_RecursosId",
                        column: x => x.RecursosId,
                        principalTable: "Recursos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecursosConsumidores_ConsumidoresId",
                table: "RecursosConsumidores",
                column: "ConsumidoresId");

            migrationBuilder.CreateIndex(
                name: "IX_RecursosConsumidores_RecursosId",
                table: "RecursosConsumidores",
                column: "RecursosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecursosConsumidores");

            migrationBuilder.DropTable(
                name: "Consumidores");

            migrationBuilder.DropTable(
                name: "Recursos");
        }
    }
}
