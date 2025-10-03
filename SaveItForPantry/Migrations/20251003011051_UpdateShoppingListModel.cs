using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaveItForPantry.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShoppingListModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantityToBuy",
                table: "ShoppingListItems",
                newName: "Quantity");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "ShoppingListItems",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "ShoppingListItems");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "ShoppingListItems",
                newName: "QuantityToBuy");
        }
    }
}
