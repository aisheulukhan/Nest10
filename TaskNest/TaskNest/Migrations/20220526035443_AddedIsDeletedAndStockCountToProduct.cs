using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskNest.Migrations
{
    public partial class AddedIsDeletedAndStockCountToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Raiting",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StockCount",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StockCount",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "Raiting",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
