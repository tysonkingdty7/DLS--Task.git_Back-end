using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DLS_Infrastructure__Layer.Migrations
{
    /// <inheritdoc />
    public partial class DLs3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_CartItems_CartItemId",
                schema: "Product",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_CartItemId",
                schema: "Product",
                table: "Product");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "34a5230d-7359-4351-aad7-0c2ca6438a95");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8158609d-530a-4f38-b9e8-9c6461b27f67");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eec061c3-fe9b-4a46-ac3c-d7b0e278efa7");

            migrationBuilder.DropColumn(
                name: "CartItemId",
                schema: "Product",
                table: "Product");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1703cab1-f80d-4023-9d26-262571d6948d", null, "Admin", "ADMIN" },
                    { "70652373-08d5-4319-99c8-a13740f73c8a", null, "User", "USER" },
                    { "ca3a8bc2-b603-41a5-81b1-27178c9f3d41", null, "Manager", "MANAGER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductID",
                table: "CartItems",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Product_ProductID",
                table: "CartItems",
                column: "ProductID",
                principalSchema: "Product",
                principalTable: "Product",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Product_ProductID",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductID",
                table: "CartItems");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1703cab1-f80d-4023-9d26-262571d6948d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "70652373-08d5-4319-99c8-a13740f73c8a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca3a8bc2-b603-41a5-81b1-27178c9f3d41");

            migrationBuilder.AddColumn<int>(
                name: "CartItemId",
                schema: "Product",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "34a5230d-7359-4351-aad7-0c2ca6438a95", null, "User", "USER" },
                    { "8158609d-530a-4f38-b9e8-9c6461b27f67", null, "Admin", "ADMIN" },
                    { "eec061c3-fe9b-4a46-ac3c-d7b0e278efa7", null, "Manager", "MANAGER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_CartItemId",
                schema: "Product",
                table: "Product",
                column: "CartItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_CartItems_CartItemId",
                schema: "Product",
                table: "Product",
                column: "CartItemId",
                principalTable: "CartItems",
                principalColumn: "Id");
        }
    }
}
