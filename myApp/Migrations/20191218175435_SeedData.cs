using Microsoft.EntityFrameworkCore.Migrations;

namespace myApp.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Department", "Email", "Name", "Salary" },
                values: new object[] { 1, 0, "akash@gmail.com", "Akash", 100000m });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Department", "Email", "Name", "Salary" },
                values: new object[] { 2, 0, "suraj@gmail.com", "Suraj", 200000m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
