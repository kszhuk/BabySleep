using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BabySleep.EfData.Migrations.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Children",
                columns: table => new
                {
                    ChildGUID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    Picture = table.Column<byte[]>(type: "BLOB", nullable: true),
                    BirthWeek = table.Column<short>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Children", x => x.ChildGUID);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    RowID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: true),
                    Type = table.Column<short>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.RowID);
                });

            migrationBuilder.CreateTable(
                name: "Sleep",
                columns: table => new
                {
                    SleepGuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartTime = table.Column<DateTime>(type: "date", nullable: false),
                    EndTime = table.Column<DateTime>(type: "date", nullable: false),
                    SleepType = table.Column<short>(type: "INTEGER", nullable: false),
                    SleepPlace = table.Column<short>(type: "INTEGER", nullable: false),
                    FeedingCount = table.Column<short>(type: "INTEGER", nullable: false),
                    Quality = table.Column<short>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    FallAsleepTime = table.Column<short>(type: "INTEGER", nullable: false),
                    AwakeningCount = table.Column<short>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sleep", x => x.SleepGuid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Children");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Sleep");
        }
    }
}
