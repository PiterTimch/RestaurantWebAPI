using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class adddeliverdepend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhonNumber",
                table: "tblDeliveryInfos",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<long>(
                name: "DeliveryInfoId",
                table: "tblOrderEntity",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OrderId",
                table: "tblDeliveryInfos",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblDeliveryInfos_OrderId",
                table: "tblDeliveryInfos",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tblDeliveryInfos_tblOrderEntity_OrderId",
                table: "tblDeliveryInfos",
                column: "OrderId",
                principalTable: "tblOrderEntity",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblDeliveryInfos_tblOrderEntity_OrderId",
                table: "tblDeliveryInfos");

            migrationBuilder.DropIndex(
                name: "IX_tblDeliveryInfos_OrderId",
                table: "tblDeliveryInfos");

            migrationBuilder.DropColumn(
                name: "DeliveryInfoId",
                table: "tblOrderEntity");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "tblDeliveryInfos");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "tblDeliveryInfos",
                newName: "PhonNumber");
        }
    }
}
