﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class addDeliveryInfos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblDeliveryInfos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    PostDepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentTypeId = table.Column<long>(type: "bigint", nullable: false),
                    PhonNumber = table.Column<string>(type: "text", nullable: false),
                    RecipientName = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDeliveryInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblDeliveryInfos_tblCities_CityId",
                        column: x => x.CityId,
                        principalTable: "tblCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblDeliveryInfos_tblPaymentTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "tblPaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblDeliveryInfos_tblPostDepartments_PostDepartmentId",
                        column: x => x.PostDepartmentId,
                        principalTable: "tblPostDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblDeliveryInfos_CityId",
                table: "tblDeliveryInfos",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_tblDeliveryInfos_PaymentTypeId",
                table: "tblDeliveryInfos",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblDeliveryInfos_PostDepartmentId",
                table: "tblDeliveryInfos",
                column: "PostDepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblDeliveryInfos");
        }
    }
}
