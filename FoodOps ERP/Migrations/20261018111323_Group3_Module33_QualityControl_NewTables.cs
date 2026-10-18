using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOps_ERP.Migrations
{
    /// <inheritdoc />
    public partial class Group3_Module33_QualityControl_NewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QCParameters",
                columns: table => new
                {
                    ParameterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParameterCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ParameterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParameterType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DefaultUOMID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QCParameters", x => x.ParameterID);
                    table.ForeignKey(
                        name: "FK_QCParameters_UnitsOfMeasure_DefaultUOMID",
                        column: x => x.DefaultUOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QCParameters_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QCParameters_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QualityDecisions",
                columns: table => new
                {
                    DecisionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchID = table.Column<long>(type: "bigint", nullable: false),
                    InspectionID = table.Column<int>(type: "int", nullable: true),
                    NCRID = table.Column<int>(type: "int", nullable: true),
                    Decision = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DecidedByUserID = table.Column<int>(type: "int", nullable: false),
                    DecidedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    ApprovedByUserID = table.Column<int>(type: "int", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityDecisions", x => x.DecisionID);
                    table.ForeignKey(
                        name: "FK_QualityDecisions_InventoryBatches_BatchID",
                        column: x => x.BatchID,
                        principalTable: "InventoryBatches",
                        principalColumn: "BatchID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QualityDecisions_NonConformanceReports_NCRID",
                        column: x => x.NCRID,
                        principalTable: "NonConformanceReports",
                        principalColumn: "NCRID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QualityDecisions_QCInspections_InspectionID",
                        column: x => x.InspectionID,
                        principalTable: "QCInspections",
                        principalColumn: "InspectionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QualityDecisions_Users_ApprovedByUserID",
                        column: x => x.ApprovedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QualityDecisions_Users_DecidedByUserID",
                        column: x => x.DecidedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QCParameters_CreatedByUserID",
                table: "QCParameters",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_QCParameters_DefaultUOMID",
                table: "QCParameters",
                column: "DefaultUOMID");

            migrationBuilder.CreateIndex(
                name: "IX_QCParameters_ModifiedByUserID",
                table: "QCParameters",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_QCParameters_ParameterCode",
                table: "QCParameters",
                column: "ParameterCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QCParameters_ParameterName",
                table: "QCParameters",
                column: "ParameterName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QualityDecisions_ApprovedByUserID",
                table: "QualityDecisions",
                column: "ApprovedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_QualityDecisions_BatchID_DecidedAt",
                table: "QualityDecisions",
                columns: new[] { "BatchID", "DecidedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_QualityDecisions_DecidedByUserID",
                table: "QualityDecisions",
                column: "DecidedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_QualityDecisions_InspectionID",
                table: "QualityDecisions",
                column: "InspectionID");

            migrationBuilder.CreateIndex(
                name: "IX_QualityDecisions_NCRID",
                table: "QualityDecisions",
                column: "NCRID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QCParameters");

            migrationBuilder.DropTable(
                name: "QualityDecisions");
        }
    }
}
