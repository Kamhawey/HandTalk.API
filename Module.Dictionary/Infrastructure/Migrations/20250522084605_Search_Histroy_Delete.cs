using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Module.Dictionary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Search_Histroy_Delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.CreateTable(
                name: "UserSearchesHistory",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    MatchedDictionaryEntryId = table.Column<long>(type: "bigint", nullable: false),
                    SearchText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSearchesHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSearchesHistory_DictionaryEntries_MatchedDictionaryEntryId",
                        column: x => x.MatchedDictionaryEntryId,
                        principalSchema: "Dictionary",
                        principalTable: "DictionaryEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });






            migrationBuilder.CreateIndex(
                name: "IX_UserSearchesHistory_MatchedDictionaryEntryId",
                schema: "Dictionary",
                table: "UserSearchesHistory",
                column: "MatchedDictionaryEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSearchesHistory_UserId_SearchDate",
                schema: "Dictionary",
                table: "UserSearchesHistory",
                columns: new[] { "UserId", "SearchDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
      

            migrationBuilder.DropTable(
                name: "UserSearchesHistory",
                schema: "Dictionary");

        }
    }
}
