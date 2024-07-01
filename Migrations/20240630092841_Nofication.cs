using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN221_Assignment.Migrations
{
    public partial class Nofication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "typeNofication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typeNofication", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nofication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    typeID = table.Column<int>(type: "int", nullable: false),
                    authorId = table.Column<int>(type: "int", nullable: false),
                    createdBy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nofication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nofication_Accounts_authorId",
                        column: x => x.authorId,
                        principalTable: "Accounts",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nofication_typeNofication_typeID",
                        column: x => x.typeID,
                        principalTable: "typeNofication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nofication_authorId",
                table: "Nofication",
                column: "authorId");

            migrationBuilder.CreateIndex(
                name: "IX_Nofication_typeID",
                table: "Nofication",
                column: "typeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nofication");

            migrationBuilder.DropTable(
                name: "typeNofication");
        }
    }
}
