using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace User_Microservice.Migrations
{
    public partial class n4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: new Guid("6ba5e478-267b-4291-b947-d8e1fddee4af"));

            migrationBuilder.DeleteData(
                table: "Phone",
                keyColumn: "Id",
                keyValue: new Guid("22916f99-599c-4418-aae6-ad9edf978147"));

            migrationBuilder.DeleteData(
                table: "UserSecret",
                keyColumn: "Id",
                keyValue: new Guid("85938cc9-c249-456f-9bfd-31351c74dd65"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("19d869b4-d3c2-4158-9b21-ff3acb5777d6"));

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "EmailAddress", "FirstName", "LastName", "Role" },
                values: new object[] { new Guid("f09cdf79-e08f-4ffd-8998-875f9a07ac63"), "admin@gmail.com", "Admin", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "City", "Country", "Line1", "Line2", "StateName", "Type", "UserId", "Zipcode" },
                values: new object[] { new Guid("50bcfd1c-72d9-4050-8b1a-ce8d3180088a"), "Admin", "Admin", "Admin", "Admin", "Admin", "ADMIN", new Guid("f09cdf79-e08f-4ffd-8998-875f9a07ac63"), "531116" });

            migrationBuilder.InsertData(
                table: "Phone",
                columns: new[] { "Id", "PhoneNumber", "Type", "UserId" },
                values: new object[] { new Guid("803d3a94-7116-454a-a860-f8fd4fee3e0c"), "8142255769", "ADMIN", new Guid("f09cdf79-e08f-4ffd-8998-875f9a07ac63") });

            migrationBuilder.InsertData(
                table: "UserSecret",
                columns: new[] { "Id", "Password", "UserId" },
                values: new object[] { new Guid("ab7d7102-74aa-48fe-a963-76212878b19c"), "EgF0XPOUFuQ96ZhmM+Bbw8c2bESZuzw0", new Guid("f09cdf79-e08f-4ffd-8998-875f9a07ac63") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: new Guid("50bcfd1c-72d9-4050-8b1a-ce8d3180088a"));

            migrationBuilder.DeleteData(
                table: "Phone",
                keyColumn: "Id",
                keyValue: new Guid("803d3a94-7116-454a-a860-f8fd4fee3e0c"));

            migrationBuilder.DeleteData(
                table: "UserSecret",
                keyColumn: "Id",
                keyValue: new Guid("ab7d7102-74aa-48fe-a963-76212878b19c"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("f09cdf79-e08f-4ffd-8998-875f9a07ac63"));

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "EmailAddress", "FirstName", "LastName", "Role" },
                values: new object[] { new Guid("19d869b4-d3c2-4158-9b21-ff3acb5777d6"), "surya@gamil.com", "Surya", "Raju", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "City", "Country", "Line1", "Line2", "StateName", "Type", "UserId", "Zipcode" },
                values: new object[] { new Guid("6ba5e478-267b-4291-b947-d8e1fddee4af"), "vizag", "India", "s-street", "ss-street", "Andhra", "ADMIN", new Guid("19d869b4-d3c2-4158-9b21-ff3acb5777d6"), "531116" });

            migrationBuilder.InsertData(
                table: "Phone",
                columns: new[] { "Id", "PhoneNumber", "Type", "UserId" },
                values: new object[] { new Guid("22916f99-599c-4418-aae6-ad9edf978147"), "8142255769", "ADMIN", new Guid("19d869b4-d3c2-4158-9b21-ff3acb5777d6") });

            migrationBuilder.InsertData(
                table: "UserSecret",
                columns: new[] { "Id", "Password", "UserId" },
                values: new object[] { new Guid("85938cc9-c249-456f-9bfd-31351c74dd65"), "7CtkAg/X1ImgPy1BBb61+XUzs6b3iWzI", new Guid("19d869b4-d3c2-4158-9b21-ff3acb5777d6") });
        }
    }
}
