using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShoppingCartMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitPriceInCartDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "ShoppingCart",
                newName: "UserId");

            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "CartDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "CartDetail");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ShoppingCart",
                newName: "UserID");
        }
    }
}
