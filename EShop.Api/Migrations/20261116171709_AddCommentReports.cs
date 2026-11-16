using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemReports_ItemId_ReporterId_Status",
                table: "ItemReports");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ItemReports",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "ItemReports",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemReports_CommentId_ReporterId_Status",
                table: "ItemReports",
                columns: new[] { "CommentId", "ReporterId", "Status" },
                unique: true,
                filter: "\"CommentId\" IS NOT NULL AND \"Status\" = 'Active'");

            migrationBuilder.CreateIndex(
                name: "IX_ItemReports_ItemId_ReporterId_Status",
                table: "ItemReports",
                columns: new[] { "ItemId", "ReporterId", "Status" },
                unique: true,
                filter: "\"ItemId\" IS NOT NULL AND \"Status\" = 'Active'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemReports_CommentId_ReporterId_Status",
                table: "ItemReports");

            migrationBuilder.DropIndex(
                name: "IX_ItemReports_ItemId_ReporterId_Status",
                table: "ItemReports");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "ItemReports");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ItemReports",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemReports_ItemId_ReporterId_Status",
                table: "ItemReports",
                columns: new[] { "ItemId", "ReporterId", "Status" },
                unique: true,
                filter: "\"Status\" = 'Active'");
        }
    }
}
