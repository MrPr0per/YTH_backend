using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YTH_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "posts",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "events",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "courses",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "image_url",
                table: "events");

            migrationBuilder.DropColumn(
                name: "image_url",
                table: "courses");
        }
    }
}
