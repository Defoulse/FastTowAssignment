using Microsoft.EntityFrameworkCore.Migrations;

namespace FastTowAssignment.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "d747aa55-cb04-4085-9a4b-d68651d5bd96", "10afb392-f847-44ee-925c-31808d698ba4" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "d747aa55-cb04-4085-9a4b-d68651d5bd96", "10afb392-f847-44ee-925c-31808d698ba4" });
        }
    }
}
