using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Authentications",
                newName: "PasswordKey");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Authentications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Authentications");

            migrationBuilder.RenameColumn(
                name: "PasswordKey",
                table: "Authentications",
                newName: "Password");
        }
    }
}
