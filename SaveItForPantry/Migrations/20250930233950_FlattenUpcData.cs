using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaveItForPantry.Migrations
{
    /// <inheritdoc />
    public partial class FlattenUpcData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OffersJson",
                table: "UpcData",
                newName: "OfferTitle");

            migrationBuilder.RenameColumn(
                name: "ImagesJson",
                table: "UpcData",
                newName: "OfferShipping");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "UpcData",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OfferAvailability",
                table: "UpcData",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OfferCondition",
                table: "UpcData",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OfferCurrency",
                table: "UpcData",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OfferDomain",
                table: "UpcData",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OfferLink",
                table: "UpcData",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "OfferListPrice",
                table: "UpcData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfferMerchant",
                table: "UpcData",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "OfferPrice",
                table: "UpcData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OfferUpdatedT",
                table: "UpcData",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "UpcData");

            migrationBuilder.DropColumn(
                name: "OfferAvailability",
                table: "UpcData");

            migrationBuilder.DropColumn(
                name: "OfferCondition",
                table: "UpcData");

            migrationBuilder.DropColumn(
                name: "OfferCurrency",
                table: "UpcData");

            migrationBuilder.DropColumn(
                name: "OfferDomain",
                table: "UpcData");

            migrationBuilder.DropColumn(
                name: "OfferLink",
                table: "UpcData");

            migrationBuilder.DropColumn(
                name: "OfferListPrice",
                table: "UpcData");

            migrationBuilder.DropColumn(
                name: "OfferMerchant",
                table: "UpcData");

            migrationBuilder.DropColumn(
                name: "OfferPrice",
                table: "UpcData");

            migrationBuilder.DropColumn(
                name: "OfferUpdatedT",
                table: "UpcData");

            migrationBuilder.RenameColumn(
                name: "OfferTitle",
                table: "UpcData",
                newName: "OffersJson");

            migrationBuilder.RenameColumn(
                name: "OfferShipping",
                table: "UpcData",
                newName: "ImagesJson");
        }
    }
}
