using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddParentProductRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ParentProductId",
                table: "tblProducts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblProducts_ParentProductId",
                table: "tblProducts",
                column: "ParentProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_tblProducts_ParentProductId",
                table: "tblProducts",
                column: "ParentProductId",
                principalTable: "tblProducts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_tblProducts_ParentProductId",
                table: "tblProducts");

            migrationBuilder.DropIndex(
                name: "IX_tblProducts_ParentProductId",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "ParentProductId",
                table: "tblProducts");
        }
    }
}
