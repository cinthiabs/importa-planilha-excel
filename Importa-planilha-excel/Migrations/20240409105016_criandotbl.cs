using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Importa_planilha_excel.Migrations
{
    /// <inheritdoc />
    public partial class criandotbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Produtos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Produtos");
        }
    }
}
