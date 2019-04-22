using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class updateRegisterHead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelCarts_RegisterHead_HeadTypeId",
                table: "FuelCarts");

            migrationBuilder.AlterColumn<int>(
                name: "HeadTypeId",
                table: "FuelCarts",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FuelCarts_RegisterHead_HeadTypeId",
                table: "FuelCarts",
                column: "HeadTypeId",
                principalTable: "RegisterHead",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelCarts_RegisterHead_HeadTypeId",
                table: "FuelCarts");

            migrationBuilder.AlterColumn<int>(
                name: "HeadTypeId",
                table: "FuelCarts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_FuelCarts_RegisterHead_HeadTypeId",
                table: "FuelCarts",
                column: "HeadTypeId",
                principalTable: "RegisterHead",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
