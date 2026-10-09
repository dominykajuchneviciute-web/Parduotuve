using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddItemTypeDiscriminator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Dimensions",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Electronics_Color",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Electronics_Manufacturer",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Footwear_Color",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Footwear_Manufacturer",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Furniture_Color",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstrumentType",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instrument_Manufacturer",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemType",
                table: "Items",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShoeSize",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Transport_Manufacturer",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Transport_Model",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Transport_Year",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Items",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dimensions",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Electronics_Color",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Electronics_Manufacturer",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Footwear_Color",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Footwear_Manufacturer",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Furniture_Color",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "InstrumentType",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Instrument_Manufacturer",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Material",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ShoeSize",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Transport_Manufacturer",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Transport_Model",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Transport_Year",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Items");
        }
    }
}
