using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YTH_backend.Migrations
{
    /// <inheritdoc />
    public partial class RefreshFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_id",
                table: "refresh_tokens");

            migrationBuilder.CreateIndex(
                name: "IX_user_id",
                table: "refresh_tokens",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_id",
                table: "refresh_tokens");

            migrationBuilder.CreateIndex(
                name: "IX_user_id",
                table: "refresh_tokens",
                column: "user_id",
                unique: true);
        }
    }
}
