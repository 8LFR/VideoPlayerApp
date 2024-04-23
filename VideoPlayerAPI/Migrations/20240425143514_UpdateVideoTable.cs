using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoPlayerAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVideoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Videos",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Videos");

            migrationBuilder.RenameColumn(
                name: "FilePathOrUrl",
                table: "Videos",
                newName: "VideoFilename");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Videos",
                newName: "ThumbnailFilename");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UploadDate",
                table: "Videos",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Videos");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Videos",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Videos",
                table: "Videos",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Videos",
                table: "Videos");

            migrationBuilder.RenameColumn(
                name: "VideoFilename",
                table: "Videos",
                newName: "FilePathOrUrl");

            migrationBuilder.RenameColumn(
                name: "ThumbnailFilename",
                table: "Videos",
                newName: "FileName");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadDate",
                table: "Videos",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Videos",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Videos",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "FileSize",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Videos",
                table: "Videos",
                column: "Id");
        }
    }
}
