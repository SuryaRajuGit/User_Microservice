using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace User_Microservice.Migrations
{
    public partial class new2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: new Guid("de81a044-44a6-4f0f-8c10-e54ebd2f8328"));

            migrationBuilder.DeleteData(
                table: "Phone",
                keyColumn: "Id",
                keyValue: new Guid("aef625bb-0694-4670-a990-f91bccfcba76"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("afa4ec93-60f6-4bd6-a884-c55172ac87f8"));

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { new Guid("afa4ec93-60f6-4bd6-a884-c55172ac87f8"), "surya@gamil.com", "Surya", "Raju", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "City", "Country", "Line1", "Line2", "StateName", "Type", "UserId", "Zipcode" },
                values: new object[] { new Guid("de81a044-44a6-4f0f-8c10-e54ebd2f8328"), "ss-street", "Andhra", "ADMIN", "s-street", "531116", "India", new Guid("afa4ec93-60f6-4bd6-a884-c55172ac87f8"), "vizag" });

            migrationBuilder.InsertData(
                table: "Phone",
                columns: new[] { "Id", "PhoneNumber", "Type", "UserId" },
                values: new object[] { new Guid("aef625bb-0694-4670-a990-f91bccfcba76"), "surya@gamil.com", "8142255769", new Guid("afa4ec93-60f6-4bd6-a884-c55172ac87f8") });
        }
    }
}
