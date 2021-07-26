using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FastTowAssignment.Migrations
{
    public partial class orders_model_add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(nullable: false),
                    DriverId = table.Column<string>(nullable: true),
                    DepartureCityId = table.Column<long>(nullable: false),
                    DestinationCityId = table.Column<long>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    CurrentStatusId = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Statuses_CurrentStatusId",
                        column: x => x.CurrentStatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Orders_Cities_DepartureCityId",
                        column: x => x.DepartureCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Orders_Cities_DestinationCityId",
                        column: x => x.DestinationCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CurrentStatusId",
                table: "Orders",
                column: "CurrentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DepartureCityId",
                table: "Orders",
                column: "DepartureCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DestinationCityId",
                table: "Orders",
                column: "DestinationCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DriverId",
                table: "Orders",
                column: "DriverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
