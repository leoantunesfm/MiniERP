using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniERP.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNovosPerfisDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Perfis",
                columns: new[] { "Id", "Descricao", "Nome" },
                values: new object[,]
                {
                    { new Guid("f0000000-0000-0000-0000-000000000002"), "Gestão do negócio e equipe", "Gerente" },
                    { new Guid("f0000000-0000-0000-0000-000000000003"), "Operação diária de vendas e clientes", "Operador" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Perfis",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Perfis",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000003"));
        }
    }
}
