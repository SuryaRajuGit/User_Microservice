using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace User_Microservice.Migrations
{
    public partial class n3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: new Guid("8fe5f668-ea29-4a2e-bd9e-063e7a6f0b0f"));

            migrationBuilder.DeleteData(
                table: "Phone",
                keyColumn: "Id",
                keyValue: new Guid("3f643b63-f3ed-4276-9acb-911d2c76ad77"));

            migrationBuilder.DeleteData(
                table: "UserSecret",
                keyColumn: "Id",
                keyValue: new Guid("8e35aff6-371c-41c6-86e1-358c7fa404ab"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("334f3d2d-4465-4e2f-9e0d-fb4fa0e11d78"));

            migrationBuilder.DropColumn(
                name: "CardHolderName",
                table: "Payment");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Payment",
                nullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Payment");

            migrationBuilder.AddColumn<string>(
                name: "CardHolderName",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "EmailAddress", "FirstName", "LastName", "Role" },
                values: new object[] { new Guid("334f3d2d-4465-4e2f-9e0d-fb4fa0e11d78"), "surya@gamil.com", "Surya", "Raju", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "City", "Country", "Line1", "Line2", "StateName", "Type", "UserId", "Zipcode" },
                values: new object[] { new Guid("8fe5f668-ea29-4a2e-bd9e-063e7a6f0b0f"), "vizag", "India", "s-street", "ss-street", "Andhra", "ADMIN", new Guid("334f3d2d-4465-4e2f-9e0d-fb4fa0e11d78"), "531116" });

            migrationBuilder.InsertData(
                table: "Phone",
                columns: new[] { "Id", "PhoneNumber", "Type", "UserId" },
                values: new object[] { new Guid("3f643b63-f3ed-4276-9acb-911d2c76ad77"), "8142255769", "ADMIN", new Guid("334f3d2d-4465-4e2f-9e0d-fb4fa0e11d78") });

            migrationBuilder.InsertData(
                table: "UserSecret",
                columns: new[] { "Id", "Password", "UserId" },
                values: new object[] { new Guid("8e35aff6-371c-41c6-86e1-358c7fa404ab"), "7CtkAg/X1ImgPy1BBb61+XUzs6b3iWzI", new Guid("334f3d2d-4465-4e2f-9e0d-fb4fa0e11d78") });
        }
    }
}
