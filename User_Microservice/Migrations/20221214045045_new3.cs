using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace User_Microservice.Migrations
{
    public partial class new3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: new Guid("bb927701-2bc6-466d-965c-8b6ac636d791"));

            migrationBuilder.DeleteData(
                table: "Phone",
                keyColumn: "Id",
                keyValue: new Guid("6c81ce9a-89cf-41a0-b493-0394dd86cbe5"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("42da0bab-4d03-4fb9-821b-21396870d24f"));

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "EmailAddress", "FirstName", "LastName", "Role" },
                values: new object[] { new Guid("ab907394-c30a-4d2f-959a-3515140698a1"), "surya@gamil.com", "Surya", "Raju", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "City", "Country", "Line1", "Line2", "StateName", "Type", "UserId", "Zipcode" },
                values: new object[] { new Guid("3b5b6a4d-2681-4433-9b80-2d831ec8b718"), "vizag", "India", "s-street", "ss-street", "Andhra", "ADMIN", new Guid("ab907394-c30a-4d2f-959a-3515140698a1"), "531116" });

            migrationBuilder.InsertData(
                table: "Phone",
                columns: new[] { "Id", "PhoneNumber", "Type", "UserId" },
                values: new object[] { new Guid("496a0ed4-8f9b-4f10-8871-1e1977d87fd0"), "8142255769", "ADMIN", new Guid("ab907394-c30a-4d2f-959a-3515140698a1") });

            migrationBuilder.InsertData(
                table: "UserSecret",
                columns: new[] { "Id", "Password", "UserId" },
                values: new object[] { new Guid("57e2e317-1502-439c-89fb-f8305dbfbf13"), "Surya@123", new Guid("ab907394-c30a-4d2f-959a-3515140698a1") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: new Guid("3b5b6a4d-2681-4433-9b80-2d831ec8b718"));

            migrationBuilder.DeleteData(
                table: "Phone",
                keyColumn: "Id",
                keyValue: new Guid("496a0ed4-8f9b-4f10-8871-1e1977d87fd0"));

            migrationBuilder.DeleteData(
                table: "UserSecret",
                keyColumn: "Id",
                keyValue: new Guid("57e2e317-1502-439c-89fb-f8305dbfbf13"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("ab907394-c30a-4d2f-959a-3515140698a1"));

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "EmailAddress", "FirstName", "LastName", "Role" },
                values: new object[] { new Guid("42da0bab-4d03-4fb9-821b-21396870d24f"), "surya@gamil.com", "Surya", "Raju", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "City", "Country", "Line1", "Line2", "StateName", "Type", "UserId", "Zipcode" },
                values: new object[] { new Guid("bb927701-2bc6-466d-965c-8b6ac636d791"), "vizag", "India", "s-street", "ss-street", "Andhra", "ADMIN", new Guid("42da0bab-4d03-4fb9-821b-21396870d24f"), "531116" });

            migrationBuilder.InsertData(
                table: "Phone",
                columns: new[] { "Id", "PhoneNumber", "Type", "UserId" },
                values: new object[] { new Guid("6c81ce9a-89cf-41a0-b493-0394dd86cbe5"), "8142255769", "ADMIN", new Guid("42da0bab-4d03-4fb9-821b-21396870d24f") });
        }
    }
}
