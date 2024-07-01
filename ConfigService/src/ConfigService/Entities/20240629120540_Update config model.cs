using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfigService.Entities
{
    /// <inheritdoc />
    public partial class Updateconfigmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Configs",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "AppName",
                table: "Configs",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Configs",
                newName: "createdAt");

            migrationBuilder.AlterColumn<string>(
                name: "AppName",
                table: "Configs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);
        }
    }
}
