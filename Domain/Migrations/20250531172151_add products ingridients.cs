using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class addproductsingridients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblProductIngridients",
                columns: table => new
                {
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    IngredientId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProductIngridients", x => new { x.ProductId, x.IngredientId });
                    table.ForeignKey(
                        name: "FK_tblProductIngridients_tblIngredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "tblIngredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblProductIngridients_tblProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tblProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblProductIngridients_IngredientId",
                table: "tblProductIngridients",
                column: "IngredientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblProductIngridients");
        }
    }
}
