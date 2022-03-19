using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Book_Store.Migrations
{
    public partial class InsertDefaultData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContinuationBook_BookId",
                table: "ContinuationBook");

            migrationBuilder.AlterColumn<int>(
                name: "PredecessorBookId",
                table: "ContinuationBook",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Order__A7AA244A5BBE3634",
                table: "ContinuationBook",
                columns: new[] { "BookId", "PredecessorBookId" });

            migrationBuilder.InsertData(
                table: "Author",
                columns: new[] { "Id", "FirstName", "LastName", "Patronymic" },
                values: new object[,]
                {
                    { 1, "Joanne", "Rowling", null },
                    { 2, "Stephen", "King", null },
                    { 3, "Fyodor", "Dostoevsky", "Mikhailovich" }
                });

            migrationBuilder.InsertData(
                table: "Discount",
                columns: new[] { "Id", "EndDate", "Name", "Percent", "StartDate" },
                values: new object[] { 1, new DateTime(2022, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harry Potter Discount", 70.0, new DateTime(2022, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 16, "Humor" },
                    { 15, "Travel" },
                    { 14, "History" },
                    { 13, "Motivational" },
                    { 12, "Self-help" },
                    { 11, "Development" },
                    { 10, "Art" },
                    { 9, "Childrens" },
                    { 8, "Science Fiction" },
                    { 7, "Historical fiction" },
                    { 6, "Thriller" },
                    { 5, "Horror" },
                    { 4, "Dystopian" },
                    { 3, "Romance" },
                    { 2, "Adventure" },
                    { 1, "Fantasy" }
                });

            migrationBuilder.InsertData(
                table: "Publisher",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Bloomsbury Publishing" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "admin" },
                    { 2, "user" },
                    { 3, "guest" }
                });

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "Login", "Password", "RoleId" },
                values: new object[] { 1, "admin", "admin", 1 });

            migrationBuilder.InsertData(
                table: "Book",
                columns: new[] { "Id", "AuthorId", "CostPrice", "GenreId", "Name", "Pages", "Price", "PublisherId", "YearPublishing" },
                values: new object[] { 2, 1, 20m, 1, "Harry Potter and the Chamber of Secrets", 251, 61.50m, 1, new DateTime(1998, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Book",
                columns: new[] { "Id", "AuthorId", "CostPrice", "GenreId", "Name", "Pages", "Price", "PublisherId", "YearPublishing" },
                values: new object[] { 1, 1, 30m, 1, "Harry Potter and the Philosopher's Stone", 400, 45.50m, 1, new DateTime(1997, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "BookDiscount",
                columns: new[] { "BookId", "DiscountId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "BookDiscount",
                columns: new[] { "BookId", "DiscountId" },
                values: new object[] { 2, 1 });

            migrationBuilder.InsertData(
                table: "ContinuationBook",
                columns: new[] { "BookId", "PredecessorBookId" },
                values: new object[] { 2, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__Order__A7AA244A5BBE3634",
                table: "ContinuationBook");

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Author",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Author",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BookDiscount",
                keyColumns: new[] { "BookId", "DiscountId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "BookDiscount",
                keyColumns: new[] { "BookId", "DiscountId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "ContinuationBook",
                keyColumns: new[] { "BookId", "PredecessorBookId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Author",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Publisher",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "PredecessorBookId",
                table: "ContinuationBook",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ContinuationBook_BookId",
                table: "ContinuationBook",
                column: "BookId");
        }
    }
}
