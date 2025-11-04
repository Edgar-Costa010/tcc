using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VidaPlus.Server.Migrations
{
    /// <inheritdoc />
    public partial class TableProfissionais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Senha",
                table: "Profissionais",
                newName: "SenhaHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenhaHash",
                table: "Profissionais",
                newName: "Senha");
        }
    }
}
