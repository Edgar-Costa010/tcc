using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VidaPlus.Server.Migrations
{
    /// <inheritdoc />
    public partial class SenhaHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenhaHash",
                table: "Pacientes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenhaHash",
                table: "Pacientes");
        }
    }
}
