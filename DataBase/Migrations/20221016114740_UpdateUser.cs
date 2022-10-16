using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBase.Migrations
{
    public partial class UpdateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b21022c-dd62-42c5-b0a3-c07d132c5e43");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6acd7655-0028-48dc-afd3-4f0f67a17cb2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cdf48203-86a7-496c-8a73-1095a3c9345b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1667ffc3-794d-4780-811f-ea73ee9ebe93", "313013aa-d8ce-4d0d-a075-7a0de8917931", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "88c77b0f-c5d0-4246-a481-49a2176aabcf", "b305d70d-a60c-49e6-a6bc-b5aeae162f22", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "caa48194-9f32-4d58-b3af-5113a922663c", "de483599-761f-418d-9586-8146127705f1", "Employee", "EMPLOYEE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1667ffc3-794d-4780-811f-ea73ee9ebe93");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88c77b0f-c5d0-4246-a481-49a2176aabcf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "caa48194-9f32-4d58-b3af-5113a922663c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1b21022c-dd62-42c5-b0a3-c07d132c5e43", "901ce84b-437d-40ad-ba3e-8c20a9f38c61", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6acd7655-0028-48dc-afd3-4f0f67a17cb2", "134a7928-31bb-4bb3-af25-4be729e3b53f", "Employee", "EMPLOYEE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cdf48203-86a7-496c-8a73-1095a3c9345b", "0e1b4211-3a92-445b-803a-02d412c2cdd8", "Customer", "CUSTOMER" });
        }
    }
}
