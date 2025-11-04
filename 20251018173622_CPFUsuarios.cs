using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VidaPlus.Server.Migrations
{
    /// <inheritdoc />
    public partial class CPFUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Senha",
                table: "Usuarios",
                newName: "CPF");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CPF",
                table: "Usuarios",
                newName: "Senha");
        }
    }
}
