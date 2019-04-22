using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class addTransactionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionId = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    SequenceId = table.Column<int>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    EndDateTime = table.Column<DateTime>(nullable: false),
                    IVVolume = table.Column<double>(nullable: false),
                    GVVolume = table.Column<double>(nullable: false),
                    GSTVolume = table.Column<double>(nullable: false),
                    GSVVolume = table.Column<double>(nullable: false),
                    IVTotalizer = table.Column<double>(nullable: false),
                    GVTotalizer = table.Column<double>(nullable: false),
                    GSTTotalizer = table.Column<double>(nullable: false),
                    GSVTotalizer = table.Column<double>(nullable: false),
                    AvgMeterFactor = table.Column<double>(nullable: false),
                    AvgTemperature = table.Column<double>(nullable: false),
                    AvgPressure = table.Column<double>(nullable: false),
                    AvgDensity = table.Column<double>(nullable: false),
                    CTL = table.Column<double>(nullable: false),
                    CPL = table.Column<double>(nullable: false),
                    CartId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_FuelCarts_CartId",
                        column: x => x.CartId,
                        principalTable: "FuelCarts",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CartId",
                table: "Transactions",
                column: "CartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
