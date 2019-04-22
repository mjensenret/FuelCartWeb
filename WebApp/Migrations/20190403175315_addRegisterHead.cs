using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class addRegisterHead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartHeadType",
                table: "FuelCarts");

            migrationBuilder.AddColumn<int>(
                name: "CartHeadTypeId",
                table: "FuelCarts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RegisterHead",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterHead", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuelCarts_CartHeadTypeId",
                table: "FuelCarts",
                column: "CartHeadTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelCarts_RegisterHead_CartHeadTypeId",
                table: "FuelCarts",
                column: "CartHeadTypeId",
                principalTable: "RegisterHead",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelCarts_RegisterHead_CartHeadTypeId",
                table: "FuelCarts");

            migrationBuilder.DropTable(
                name: "RegisterHead");

            migrationBuilder.DropIndex(
                name: "IX_FuelCarts_CartHeadTypeId",
                table: "FuelCarts");

            migrationBuilder.DropColumn(
                name: "CartHeadTypeId",
                table: "FuelCarts");

            migrationBuilder.AddColumn<int>(
                name: "CartHeadType",
                table: "FuelCarts",
                nullable: false,
                defaultValue: 0);
        }
    }
}
