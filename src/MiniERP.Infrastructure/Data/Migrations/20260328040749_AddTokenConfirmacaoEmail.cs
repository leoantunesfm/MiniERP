using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniERP.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTokenConfirmacaoEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TokenConfirmacaoEmail",
                table: "Empresas",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenConfirmacaoEmail",
                table: "Empresas");
        }
    }
}
