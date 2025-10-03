using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaveItForPantry.Migrations
{
    /// <inheritdoc />
    public partial class AddInCartToShoppingListItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InCart",
                table: "ShoppingListItems",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InCart",
                table: "ShoppingListItems");
        }
    }
}
