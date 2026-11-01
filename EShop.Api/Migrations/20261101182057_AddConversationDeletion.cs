using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddConversationDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "User1DeletedAt",
                table: "Conversations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "User2DeletedAt",
                table: "Conversations",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User1DeletedAt",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "User2DeletedAt",
                table: "Conversations");
        }
    }
}
