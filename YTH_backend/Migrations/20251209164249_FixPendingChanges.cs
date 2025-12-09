using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YTH_backend.Migrations
{
    /// <inheritdoc />
    public partial class FixPendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_expert_applications_status_fields_consistency",
                table: "expert_applications");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "posts",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldDefaultValueSql: "'Posted'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_expert_applications_status_fields_consistency",
                table: "expert_applications",
                sql: "(\r\n                   (status IN ('Created','Sent') AND accepted_by IS NULL AND is_approved IS NULL AND resolution_message IS NULL)\r\n                   OR\r\n                   (status = 'AcceptedForReview' AND accepted_by IS NOT NULL AND is_approved IS NULL AND resolution_message IS NULL)\r\n                   OR\r\n                   (status = 'Reviewed' AND accepted_by IS NOT NULL AND is_approved IS NOT NULL AND resolution_message IS NOT NULL)\r\n                 )");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_expert_applications_status_fields_consistency",
                table: "expert_applications");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "posts",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValueSql: "'Posted'",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddCheckConstraint(
                name: "CK_expert_applications_status_fields_consistency",
                table: "expert_applications",
                sql: "(\r\n                   (status IN ('NotSent','Sent') AND accepted_by IS NULL AND is_approved IS NULL AND resolution_message IS NULL)\r\n                   OR\r\n                   (status = 'AcceptedForReview' AND accepted_by IS NOT NULL AND is_approved IS NULL AND resolution_message IS NULL)\r\n                   OR\r\n                   (status = 'Reviewed' AND accepted_by IS NOT NULL AND is_approved IS NOT NULL AND resolution_message IS NOT NULL)\r\n                 )");
        }
    }
}
