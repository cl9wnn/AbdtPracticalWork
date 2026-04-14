using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PracticalWork.Library.Data.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailToReader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Readers",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(
                @"UPDATE ""Readers"" 
                SET ""Email"" = ""Id""::text || '.mail.ru'");
            
            migrationBuilder.CreateIndex(
                name: "IX_Readers_Email",
                table: "Readers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Readers_Email",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Readers");
        }
    }
}
