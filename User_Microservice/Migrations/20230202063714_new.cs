using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace User_Microservice.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Line1 = table.Column<string>(nullable: true),
                    Line2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Zipcode = table.Column<string>(nullable: true),
                    StateName = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
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

            migrationBuilder.CreateTable(
                name: "Phone",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phone_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSecret",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSecret", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSecret_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "EmailAddress", "FirstName", "IsActive", "LastName", "Role", "UpdateDate", "UpdatedBy" },
                values: new object[] { new Guid("9236ba78-62d6-48e4-b2ac-ac796b01b0ff"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", false, "Admin", "ADMIN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000") });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "City", "Country", "CreatedBy", "CreatedDate", "IsActive", "Line1", "Line2", "StateName", "Type", "UpdateDate", "UpdatedBy", "UserId", "Zipcode" },
                values: new object[] { new Guid("136f7604-ba07-4f43-8dec-78456a78776e"), "Admin", "Admin", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Admin", "Admin", "Admin", "ADMIN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("9236ba78-62d6-48e4-b2ac-ac796b01b0ff"), "531116" });

            migrationBuilder.InsertData(
                table: "Phone",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsActive", "PhoneNumber", "Type", "UpdateDate", "UpdatedBy", "UserId" },
                values: new object[] { new Guid("80bfa39c-e781-4fc0-a60c-5840fc90bbcd"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "8142255769", "ADMIN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("9236ba78-62d6-48e4-b2ac-ac796b01b0ff") });

            migrationBuilder.InsertData(
                table: "UserSecret",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsActive", "Password", "UpdateDate", "UpdatedBy", "UserId" },
                values: new object[] { new Guid("57901aa7-4164-4d5b-8a23-31629a36cfec"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "EgF0XPOUFuQ96ZhmM+Bbw8c2bESZuzw0", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("9236ba78-62d6-48e4-b2ac-ac796b01b0ff") });

            migrationBuilder.CreateIndex(
                name: "IX_Address_UserId",
                table: "Address",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_UserId",
                table: "Payment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Phone_UserId",
                table: "Phone",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSecret_UserId",
                table: "UserSecret",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Phone");

            migrationBuilder.DropTable(
                name: "UserSecret");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
