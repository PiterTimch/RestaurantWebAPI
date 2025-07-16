using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class fixonetoone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblDeliveryInfos_tblOrderEntity_OrderId",
                table: "tblDeliveryInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_tblOrderEntity_AspNetUsers_UserId",
                table: "tblOrderEntity");

            migrationBuilder.DropTable(
                name: "tblCartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblDeliveryInfos",
                table: "tblDeliveryInfos");

            migrationBuilder.DropIndex(
                name: "IX_tblDeliveryInfos_OrderId",
                table: "tblDeliveryInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblCarts",
                table: "tblCarts");

            migrationBuilder.DropIndex(
                name: "IX_tblCarts_UserId",
                table: "tblCarts");

            migrationBuilder.DropColumn(
                name: "DeliveryInfoId",
                table: "tblOrderEntity");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "tblDeliveryInfos");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "tblDeliveryInfos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tblDeliveryInfos");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "tblCarts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tblCarts");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tblCarts",
                newName: "ProductId");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "tblOrderEntity",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OrderId",
                table: "tblDeliveryInfos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "tblCarts",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "tblCarts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblDeliveryInfos",
                table: "tblDeliveryInfos",
                column: "OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblCarts",
                table: "tblCarts",
                columns: new[] { "ProductId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_tblCarts_UserId",
                table: "tblCarts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblCarts_tblProducts_ProductId",
                table: "tblCarts",
                column: "ProductId",
                principalTable: "tblProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblDeliveryInfos_tblOrderEntity_OrderId",
                table: "tblDeliveryInfos",
                column: "OrderId",
                principalTable: "tblOrderEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblOrderEntity_AspNetUsers_UserId",
                table: "tblOrderEntity",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblCarts_tblProducts_ProductId",
                table: "tblCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_tblDeliveryInfos_tblOrderEntity_OrderId",
                table: "tblDeliveryInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_tblOrderEntity_AspNetUsers_UserId",
                table: "tblOrderEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblDeliveryInfos",
                table: "tblDeliveryInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblCarts",
                table: "tblCarts");

            migrationBuilder.DropIndex(
                name: "IX_tblCarts_UserId",
                table: "tblCarts");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "tblCarts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "tblCarts",
                newName: "Id");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "tblOrderEntity",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "DeliveryInfoId",
                table: "tblOrderEntity",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OrderId",
                table: "tblDeliveryInfos",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "tblDeliveryInfos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "tblDeliveryInfos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tblDeliveryInfos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "tblCarts",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "tblCarts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tblCarts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblDeliveryInfos",
                table: "tblDeliveryInfos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblCarts",
                table: "tblCarts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "tblCartItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CartId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblCartItems_tblCarts_CartId",
                        column: x => x.CartId,
                        principalTable: "tblCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblCartItems_tblProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tblProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblDeliveryInfos_OrderId",
                table: "tblDeliveryInfos",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblCarts_UserId",
                table: "tblCarts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblCartItems_CartId",
                table: "tblCartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_tblCartItems_ProductId",
                table: "tblCartItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblDeliveryInfos_tblOrderEntity_OrderId",
                table: "tblDeliveryInfos",
                column: "OrderId",
                principalTable: "tblOrderEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblOrderEntity_AspNetUsers_UserId",
                table: "tblOrderEntity",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
