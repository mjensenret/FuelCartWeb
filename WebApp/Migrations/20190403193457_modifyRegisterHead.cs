using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class modifyRegisterHead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelCarts_RegisterHead_CartHeadTypeId",
                table: "FuelCarts");

            migrationBuilder.RenameColumn(
                name: "CartHeadTypeId",
                table: "FuelCarts",
                newName: "HeadTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_FuelCarts_CartHeadTypeId",
                table: "FuelCarts",
                newName: "IX_FuelCarts_HeadTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelCarts_RegisterHead_HeadTypeId",
                table: "FuelCarts",
                column: "HeadTypeId",
                principalTable: "RegisterHead",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelCarts_RegisterHead_HeadTypeId",
                table: "FuelCarts");

            migrationBuilder.RenameColumn(
                name: "HeadTypeId",
                table: "FuelCarts",
                newName: "CartHeadTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_FuelCarts_HeadTypeId",
                table: "FuelCarts",
                newName: "IX_FuelCarts_CartHeadTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelCarts_RegisterHead_CartHeadTypeId",
                table: "FuelCarts",
                column: "CartHeadTypeId",
                principalTable: "RegisterHead",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
