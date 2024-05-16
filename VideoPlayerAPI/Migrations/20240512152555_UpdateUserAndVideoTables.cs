using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoPlayerAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAndVideoTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AvatarFilename",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "AvatarFilename",
                table: "Users");
        }
    }
}
