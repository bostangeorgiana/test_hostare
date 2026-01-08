using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusEats.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderItemMenuItemRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_menu_item_id",
                table: "OrderItems",
                column: "menu_item_id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_MenuItems_menu_item_id",
                table: "OrderItems",
                column: "menu_item_id",
                principalTable: "MenuItems",
                principalColumn: "MenuItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_MenuItems_menu_item_id",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_menu_item_id",
                table: "OrderItems");
        }
    }
}
