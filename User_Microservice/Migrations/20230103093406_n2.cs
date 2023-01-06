using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace User_Microservice.Migrations
{
    public partial class n2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Card");

            migrationBuilder.DropIndex(
                name: "IX_UserSecret_UserId",
                table: "UserSecret");

            migrationBuilder.DropIndex(
                name: "IX_Phone_UserId",
                table: "Phone");

            migrationBuilder.DropIndex(
                name: "IX_Address_UserId",
                table: "Address");

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

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    CardHolderName = table.Column<string>(nullable: true),
                    CardNo = table.Column<string>(nullable: true),
                    ExpiryDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_UserSecret_UserId",
                table: "UserSecret",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Phone_UserId",
                table: "Phone",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Address_UserId",
                table: "Address",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_UserId",
                table: "Payment",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_UserSecret_UserId",
                table: "UserSecret");

            migrationBuilder.DropIndex(
                name: "IX_Phone_UserId",
                table: "Phone");

            migrationBuilder.DropIndex(
                name: "IX_Address_UserId",
                table: "Address");

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

            migrationBuilder.CreateTable(
                name: "Card",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardHolderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Card_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_UserSecret_UserId",
                table: "UserSecret",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Phone_UserId",
                table: "Phone",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_UserId",
                table: "Address",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Card_UserId",
                table: "Card",
                column: "UserId");
        }
    }
}
