using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class addproducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Slug = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Image = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    ProductSizeId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblProducts_tblProductSizes_ProductSizeId",
                        column: x => x.ProductSizeId,
                        principalTable: "tblProductSizes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tblProducts_tbl_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "tbl_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblProducts_CategoryId",
                table: "tblProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_tblProducts_ProductSizeId",
                table: "tblProducts",
                column: "ProductSizeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblProducts");
        }
    }
}
