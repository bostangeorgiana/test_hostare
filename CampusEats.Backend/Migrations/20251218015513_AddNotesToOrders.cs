using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CampusEats.Migrations
{
    /// <inheritdoc />
    public partial class AddNotesToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemIngredients_MenuItems_MenuItemId",
                table: "MenuItemIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemLabels_MenuItems_MenuItemId",
                table: "MenuItemLabels");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_MenuItems_menu_item_id",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_order_id",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItems",
                table: "MenuItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItemLabels",
                table: "MenuItemLabels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItemIngredients",
                table: "MenuItemIngredients");

            migrationBuilder.DropColumn(
                name: "Allergens",
                table: "MenuItemIngredients");

            migrationBuilder.DropColumn(
                name: "CaloriesPerUnit",
                table: "MenuItemIngredients");

            migrationBuilder.DropColumn(
                name: "IngredientName",
                table: "MenuItemIngredients");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "orders");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                newName: "order_items");

            migrationBuilder.RenameTable(
                name: "MenuItems",
                newName: "menu_items");

            migrationBuilder.RenameTable(
                name: "MenuItemLabels",
                newName: "menu_item_labels");

            migrationBuilder.RenameTable(
                name: "MenuItemIngredients",
                newName: "menu_item_ingredients");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_order_id",
                table: "order_items",
                newName: "IX_order_items_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_menu_item_id",
                table: "order_items",
                newName: "IX_order_items_menu_item_id");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "menu_items",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "menu_items",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "menu_items",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Calories",
                table: "menu_items",
                newName: "calories");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "menu_items",
                newName: "is_available");

            migrationBuilder.RenameColumn(
                name: "CurrentStock",
                table: "menu_items",
                newName: "current_stock");

            migrationBuilder.RenameColumn(
                name: "MenuItemId",
                table: "menu_items",
                newName: "menu_item_id");

            migrationBuilder.RenameColumn(
                name: "LabelId",
                table: "menu_item_labels",
                newName: "label_id");

            migrationBuilder.RenameColumn(
                name: "MenuItemId",
                table: "menu_item_labels",
                newName: "menu_item_id");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "menu_item_ingredients",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "IngredientId",
                table: "menu_item_ingredients",
                newName: "ingredient_id");

            migrationBuilder.RenameColumn(
                name: "MenuItemId",
                table: "menu_item_ingredients",
                newName: "menu_item_id");

            migrationBuilder.AddColumn<string>(
                name: "refresh_token",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "refresh_token_expires_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "final_amount_paid",
                table: "orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "loyalty_points_used",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "notes",
                table: "orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "total_loyalty_points_earned",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "menu_items",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "menu_items",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "picture_link",
                table: "menu_items",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_orders",
                table: "orders",
                column: "order_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_order_items",
                table: "order_items",
                column: "order_item_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_menu_items",
                table: "menu_items",
                column: "menu_item_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_menu_item_labels",
                table: "menu_item_labels",
                columns: new[] { "menu_item_id", "label_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_menu_item_ingredients",
                table: "menu_item_ingredients",
                columns: new[] { "menu_item_id", "ingredient_id" });

            migrationBuilder.CreateTable(
                name: "ingredients",
                columns: table => new
                {
                    ingredient_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    allergens = table.Column<string>(type: "text", nullable: true),
                    calories_per_unit = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredients", x => x.ingredient_id);
                });

            migrationBuilder.CreateTable(
                name: "menu_labels",
                columns: table => new
                {
                    label_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_labels", x => x.label_id);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: false),
                    payment_method = table.Column<string>(type: "text", nullable: true),
                    payment_provider = table.Column<string>(type: "text", nullable: false),
                    provider_payment_id = table.Column<string>(type: "text", nullable: true),
                    payment_intent_secret = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    transaction_id = table.Column<string>(type: "text", nullable: true),
                    confirmed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK_payments_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recommendations",
                columns: table => new
                {
                    base_item_id = table.Column<int>(type: "integer", nullable: false),
                    recommended_item_id = table.Column<int>(type: "integer", nullable: false),
                    score = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recommendations", x => new { x.base_item_id, x.recommended_item_id });
                    table.ForeignKey(
                        name: "FK_recommendations_menu_items_base_item_id",
                        column: x => x.base_item_id,
                        principalTable: "menu_items",
                        principalColumn: "menu_item_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recommendations_menu_items_recommended_item_id",
                        column: x => x.recommended_item_id,
                        principalTable: "menu_items",
                        principalColumn: "menu_item_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    student_id = table.Column<int>(type: "integer", nullable: false),
                    loyalty_points = table.Column<int>(type: "integer", nullable: false),
                    reserved_points = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.student_id);
                    table.ForeignKey(
                        name: "FK_students_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payment_events",
                columns: table => new
                {
                    event_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    payment_id = table.Column<int>(type: "integer", nullable: false),
                    event_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    event_status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    message = table.Column<string>(type: "text", nullable: true),
                    payload = table.Column<string>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_events", x => x.event_id);
                    table.ForeignKey(
                        name: "FK_payment_events_payments_payment_id",
                        column: x => x.payment_id,
                        principalTable: "payments",
                        principalColumn: "payment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "carts",
                columns: table => new
                {
                    cart_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carts", x => x.cart_id);
                    table.ForeignKey(
                        name: "FK_carts_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cart_items",
                columns: table => new
                {
                    cart_item_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cart_id = table.Column<int>(type: "integer", nullable: false),
                    menu_item_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    added_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart_items", x => x.cart_item_id);
                    table.ForeignKey(
                        name: "FK_cart_items_carts_cart_id",
                        column: x => x.cart_id,
                        principalTable: "carts",
                        principalColumn: "cart_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cart_items_menu_items_menu_item_id",
                        column: x => x.menu_item_id,
                        principalTable: "menu_items",
                        principalColumn: "menu_item_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_student_id",
                table: "orders",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_favorites_menu_item_id",
                table: "favorites",
                column: "menu_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_menu_item_labels_label_id",
                table: "menu_item_labels",
                column: "label_id");

            migrationBuilder.CreateIndex(
                name: "IX_menu_item_ingredients_ingredient_id",
                table: "menu_item_ingredients",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_cart_id",
                table: "cart_items",
                column: "cart_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_menu_item_id",
                table: "cart_items",
                column: "menu_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_carts_student_id",
                table: "carts",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_events_payment_id",
                table: "payment_events",
                column: "payment_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_order_id",
                table: "payments",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_recommendations_recommended_item_id",
                table: "recommendations",
                column: "recommended_item_id");

            migrationBuilder.AddForeignKey(
                name: "FK_favorites_menu_items_menu_item_id",
                table: "favorites",
                column: "menu_item_id",
                principalTable: "menu_items",
                principalColumn: "menu_item_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_favorites_students_student_id",
                table: "favorites",
                column: "student_id",
                principalTable: "students",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_menu_item_ingredients_ingredients_ingredient_id",
                table: "menu_item_ingredients",
                column: "ingredient_id",
                principalTable: "ingredients",
                principalColumn: "ingredient_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_menu_item_ingredients_menu_items_menu_item_id",
                table: "menu_item_ingredients",
                column: "menu_item_id",
                principalTable: "menu_items",
                principalColumn: "menu_item_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_menu_item_labels_menu_items_menu_item_id",
                table: "menu_item_labels",
                column: "menu_item_id",
                principalTable: "menu_items",
                principalColumn: "menu_item_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_menu_item_labels_menu_labels_label_id",
                table: "menu_item_labels",
                column: "label_id",
                principalTable: "menu_labels",
                principalColumn: "label_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_menu_items_menu_item_id",
                table: "order_items",
                column: "menu_item_id",
                principalTable: "menu_items",
                principalColumn: "menu_item_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_orders_order_id",
                table: "order_items",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "order_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_students_student_id",
                table: "orders",
                column: "student_id",
                principalTable: "students",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_favorites_menu_items_menu_item_id",
                table: "favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_favorites_students_student_id",
                table: "favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_menu_item_ingredients_ingredients_ingredient_id",
                table: "menu_item_ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_menu_item_ingredients_menu_items_menu_item_id",
                table: "menu_item_ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_menu_item_labels_menu_items_menu_item_id",
                table: "menu_item_labels");

            migrationBuilder.DropForeignKey(
                name: "FK_menu_item_labels_menu_labels_label_id",
                table: "menu_item_labels");

            migrationBuilder.DropForeignKey(
                name: "FK_order_items_menu_items_menu_item_id",
                table: "order_items");

            migrationBuilder.DropForeignKey(
                name: "FK_order_items_orders_order_id",
                table: "order_items");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_students_student_id",
                table: "orders");

            migrationBuilder.DropTable(
                name: "cart_items");

            migrationBuilder.DropTable(
                name: "ingredients");

            migrationBuilder.DropTable(
                name: "menu_labels");

            migrationBuilder.DropTable(
                name: "payment_events");

            migrationBuilder.DropTable(
                name: "recommendations");

            migrationBuilder.DropTable(
                name: "carts");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orders",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_student_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_favorites_menu_item_id",
                table: "favorites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_order_items",
                table: "order_items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_menu_items",
                table: "menu_items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_menu_item_labels",
                table: "menu_item_labels");

            migrationBuilder.DropIndex(
                name: "IX_menu_item_labels_label_id",
                table: "menu_item_labels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_menu_item_ingredients",
                table: "menu_item_ingredients");

            migrationBuilder.DropIndex(
                name: "IX_menu_item_ingredients_ingredient_id",
                table: "menu_item_ingredients");

            migrationBuilder.DropColumn(
                name: "refresh_token",
                table: "users");

            migrationBuilder.DropColumn(
                name: "refresh_token_expires_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "final_amount_paid",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "loyalty_points_used",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "notes",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "total_loyalty_points_earned",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "picture_link",
                table: "menu_items");

            migrationBuilder.RenameTable(
                name: "orders",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "order_items",
                newName: "OrderItems");

            migrationBuilder.RenameTable(
                name: "menu_items",
                newName: "MenuItems");

            migrationBuilder.RenameTable(
                name: "menu_item_labels",
                newName: "MenuItemLabels");

            migrationBuilder.RenameTable(
                name: "menu_item_ingredients",
                newName: "MenuItemIngredients");

            migrationBuilder.RenameIndex(
                name: "IX_order_items_order_id",
                table: "OrderItems",
                newName: "IX_OrderItems_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_order_items_menu_item_id",
                table: "OrderItems",
                newName: "IX_OrderItems_menu_item_id");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "MenuItems",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "MenuItems",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "MenuItems",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "calories",
                table: "MenuItems",
                newName: "Calories");

            migrationBuilder.RenameColumn(
                name: "is_available",
                table: "MenuItems",
                newName: "IsAvailable");

            migrationBuilder.RenameColumn(
                name: "current_stock",
                table: "MenuItems",
                newName: "CurrentStock");

            migrationBuilder.RenameColumn(
                name: "menu_item_id",
                table: "MenuItems",
                newName: "MenuItemId");

            migrationBuilder.RenameColumn(
                name: "label_id",
                table: "MenuItemLabels",
                newName: "LabelId");

            migrationBuilder.RenameColumn(
                name: "menu_item_id",
                table: "MenuItemLabels",
                newName: "MenuItemId");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "MenuItemIngredients",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "ingredient_id",
                table: "MenuItemIngredients",
                newName: "IngredientId");

            migrationBuilder.RenameColumn(
                name: "menu_item_id",
                table: "MenuItemIngredients",
                newName: "MenuItemId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MenuItems",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MenuItems",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Allergens",
                table: "MenuItemIngredients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CaloriesPerUnit",
                table: "MenuItemIngredients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IngredientName",
                table: "MenuItemIngredients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "order_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "order_item_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItems",
                table: "MenuItems",
                column: "MenuItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItemLabels",
                table: "MenuItemLabels",
                columns: new[] { "MenuItemId", "LabelId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItemIngredients",
                table: "MenuItemIngredients",
                columns: new[] { "MenuItemId", "IngredientId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemIngredients_MenuItems_MenuItemId",
                table: "MenuItemIngredients",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "MenuItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemLabels_MenuItems_MenuItemId",
                table: "MenuItemLabels",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "MenuItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_MenuItems_menu_item_id",
                table: "OrderItems",
                column: "menu_item_id",
                principalTable: "MenuItems",
                principalColumn: "MenuItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_order_id",
                table: "OrderItems",
                column: "order_id",
                principalTable: "Orders",
                principalColumn: "order_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
