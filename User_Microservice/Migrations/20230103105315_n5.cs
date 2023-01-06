using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace User_Microservice.Migrations
{
    public partial class n5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payment_UserId",
                table: "Payment");

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: new Guid("1a2a103d-17e3-46e4-b38a-0bcbd9064bba"));

            migrationBuilder.DeleteData(
                table: "Phone",
                keyColumn: "Id",
                keyValue: new Guid("f794c6cb-eb2e-4bfa-862b-0b0291a0b905"));

            migrationBuilder.DeleteData(
                table: "UserSecret",
                keyColumn: "Id",
                keyValue: new Guid("47b15044-a2a2-4981-a472-734b45a91300"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("fdce4d6a-226b-474c-976f-87f63084d791"));

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

            migrationBuilder.CreateIndex(
                name: "IX_Payment_UserId",
                table: "Payment",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payment_UserId",
                table: "Payment");

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
                values: new object[] { new Guid("fdce4d6a-226b-474c-976f-87f63084d791"), "surya@gamil.com", "Surya", "Raju", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "City", "Country", "Line1", "Line2", "StateName", "Type", "UserId", "Zipcode" },
                values: new object[] { new Guid("1a2a103d-17e3-46e4-b38a-0bcbd9064bba"), "vizag", "India", "s-street", "ss-street", "Andhra", "ADMIN", new Guid("fdce4d6a-226b-474c-976f-87f63084d791"), "531116" });

            migrationBuilder.InsertData(
                table: "Phone",
                columns: new[] { "Id", "PhoneNumber", "Type", "UserId" },
                values: new object[] { new Guid("f794c6cb-eb2e-4bfa-862b-0b0291a0b905"), "8142255769", "ADMIN", new Guid("fdce4d6a-226b-474c-976f-87f63084d791") });

            migrationBuilder.InsertData(
                table: "UserSecret",
                columns: new[] { "Id", "Password", "UserId" },
                values: new object[] { new Guid("47b15044-a2a2-4981-a472-734b45a91300"), "7CtkAg/X1ImgPy1BBb61+XUzs6b3iWzI", new Guid("fdce4d6a-226b-474c-976f-87f63084d791") });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_UserId",
                table: "Payment",
                column: "UserId",
                unique: true);
        }
    }
}
