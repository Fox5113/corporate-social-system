using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsFeed.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "NewsComment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 0, 4, 54, 281, DateTimeKind.Local).AddTicks(1458),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 0, 0, 52, 851, DateTimeKind.Local).AddTicks(3318));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "NewsComment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 0, 4, 54, 281, DateTimeKind.Local).AddTicks(1240),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 0, 0, 52, 851, DateTimeKind.Local).AddTicks(3005));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "NewsComment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("5c325ac2-529e-4b32-92a2-5f8c5c821a80"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "News",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 0, 4, 54, 281, DateTimeKind.Local).AddTicks(995),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 0, 0, 52, 851, DateTimeKind.Local).AddTicks(2715));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "News",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 0, 4, 54, 281, DateTimeKind.Local).AddTicks(773),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 0, 0, 52, 851, DateTimeKind.Local).AddTicks(2397));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "News",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("1dcd2afb-5365-4363-8662-a73a0c56756e"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "HashtagNews",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("bce14068-add4-4074-b5f8-36d3c14ab96d"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Hashtag",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("e5c0b747-de1d-46b2-852c-99d1f27eb4a6"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Employee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("0cf08358-53b9-413d-8e9f-affdff351dca"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "NewsComment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 0, 0, 52, 851, DateTimeKind.Local).AddTicks(3318),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 0, 4, 54, 281, DateTimeKind.Local).AddTicks(1458));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "NewsComment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 0, 0, 52, 851, DateTimeKind.Local).AddTicks(3005),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 0, 4, 54, 281, DateTimeKind.Local).AddTicks(1240));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "NewsComment",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("5c325ac2-529e-4b32-92a2-5f8c5c821a80"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "News",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 0, 0, 52, 851, DateTimeKind.Local).AddTicks(2715),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 0, 4, 54, 281, DateTimeKind.Local).AddTicks(995));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "News",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 0, 0, 52, 851, DateTimeKind.Local).AddTicks(2397),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 0, 4, 54, 281, DateTimeKind.Local).AddTicks(773));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "News",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("1dcd2afb-5365-4363-8662-a73a0c56756e"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "HashtagNews",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("bce14068-add4-4074-b5f8-36d3c14ab96d"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Hashtag",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("e5c0b747-de1d-46b2-852c-99d1f27eb4a6"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Employee",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("0cf08358-53b9-413d-8e9f-affdff351dca"));
        }
    }
}
