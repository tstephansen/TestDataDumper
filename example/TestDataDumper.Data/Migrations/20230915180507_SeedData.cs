using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestDataDumper.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "City", "Country", "State", "Street", "Street2", "Zip" },
                values: new object[,]
                {
                    { new Guid("59f1f53b-5af5-434e-9215-581cbe98b106"), "Roswell", "USA", "GA", "456 Second Ave.", null, "30076" },
                    { new Guid("887257ed-a55c-4837-bf23-ac2c74f6c65a"), "Atlanta", "USA", "GA", "789 Third Street NE", "Apt. 210", "30319" },
                    { new Guid("e4fd2b47-8d09-4c17-ad4c-26ef61b2de17"), "Johns Creek", "USA", "GA", "123 Main Street", "Suite 100", "30022" }
                });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0ede9947-28af-489f-b15d-658a4df73c8f"), "Skipper" },
                    { new Guid("da94a5b8-9a64-4e26-87fb-6ed48a6698a7"), "Munson" }
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "AddressId", "FirstName", "LastName" },
                values: new object[,]
                {
                    { new Guid("412dfae8-e2e0-47f3-9762-c32004c97115"), new Guid("e4fd2b47-8d09-4c17-ad4c-26ef61b2de17"), "Jane", "Smith" },
                    { new Guid("4eb9c29b-d843-46d8-826e-a5821f8ac4f6"), new Guid("887257ed-a55c-4837-bf23-ac2c74f6c65a"), "James", "Maxwell" },
                    { new Guid("74c150a1-29a8-44d7-a12b-21d41b5367ec"), new Guid("59f1f53b-5af5-434e-9215-581cbe98b106"), "Rebecca", "Lane" },
                    { new Guid("fa79a427-a880-4823-b6ab-be06184c2010"), new Guid("e4fd2b47-8d09-4c17-ad4c-26ef61b2de17"), "John", "Smith" }
                });

            migrationBuilder.InsertData(
                table: "PetOwners",
                columns: new[] { "Id", "OwnerId", "PetId" },
                values: new object[,]
                {
                    { new Guid("0b1d9377-47cf-49b8-8b72-a9b7db3877c4"), new Guid("4eb9c29b-d843-46d8-826e-a5821f8ac4f6"), new Guid("0ede9947-28af-489f-b15d-658a4df73c8f") },
                    { new Guid("3d968eb0-bcbe-4cbd-8ff0-9def9f3c6ff4"), new Guid("412dfae8-e2e0-47f3-9762-c32004c97115"), new Guid("da94a5b8-9a64-4e26-87fb-6ed48a6698a7") },
                    { new Guid("3e932ab2-36d3-42df-8bbb-48363142aa59"), new Guid("fa79a427-a880-4823-b6ab-be06184c2010"), new Guid("da94a5b8-9a64-4e26-87fb-6ed48a6698a7") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: new Guid("74c150a1-29a8-44d7-a12b-21d41b5367ec"));

            migrationBuilder.DeleteData(
                table: "PetOwners",
                keyColumn: "Id",
                keyValue: new Guid("0b1d9377-47cf-49b8-8b72-a9b7db3877c4"));

            migrationBuilder.DeleteData(
                table: "PetOwners",
                keyColumn: "Id",
                keyValue: new Guid("3d968eb0-bcbe-4cbd-8ff0-9def9f3c6ff4"));

            migrationBuilder.DeleteData(
                table: "PetOwners",
                keyColumn: "Id",
                keyValue: new Guid("3e932ab2-36d3-42df-8bbb-48363142aa59"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("59f1f53b-5af5-434e-9215-581cbe98b106"));

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: new Guid("412dfae8-e2e0-47f3-9762-c32004c97115"));

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: new Guid("4eb9c29b-d843-46d8-826e-a5821f8ac4f6"));

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: new Guid("fa79a427-a880-4823-b6ab-be06184c2010"));

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: new Guid("0ede9947-28af-489f-b15d-658a4df73c8f"));

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: new Guid("da94a5b8-9a64-4e26-87fb-6ed48a6698a7"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("887257ed-a55c-4837-bf23-ac2c74f6c65a"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("e4fd2b47-8d09-4c17-ad4c-26ef61b2de17"));
        }
    }
}
