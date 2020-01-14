using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LoginIdentityCore.Migrations
{
    public partial class InsertRolesData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("1865a4ce-5c68-4128-bbc4-0d4b1225a704"), null, "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1865a4ce-5c68-4128-bbc4-0d4b1225a704"));
        }
    }
}
