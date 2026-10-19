using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOps_ERP.Migrations
{
    /// <inheritdoc />
    public partial class Group3_Module34_CostingAndVariance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BatchCostSummaries",
                columns: table => new
                {
                    BatchCostID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionOrderID = table.Column<int>(type: "int", nullable: false),
                    ActualMaterialCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ActualLaborCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ActualOverheadCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalBatchCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, computedColumnSql: "[ActualMaterialCost] + [ActualLaborCost] + [ActualOverheadCost]", stored: true),
                    GoodQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true, computedColumnSql: "CASE WHEN [GoodQty] = 0 THEN NULL ELSE ([ActualMaterialCost] + [ActualLaborCost] + [ActualOverheadCost]) / [GoodQty] END", stored: true),
                    StandardUnitCostSnapshot = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinalizedByUserID = table.Column<int>(type: "int", nullable: true),
                    FinalizedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchCostSummaries", x => x.BatchCostID);
                    table.ForeignKey(
                        name: "FK_BatchCostSummaries_ProductionOrders_ProductionOrderID",
                        column: x => x.ProductionOrderID,
                        principalTable: "ProductionOrders",
                        principalColumn: "ProductionOrderID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchCostSummaries_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchCostSummaries_Users_FinalizedByUserID",
                        column: x => x.FinalizedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchCostSummaries_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CostComponents",
                columns: table => new
                {
                    ComponentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ComponentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ComponentType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AllocationBasis = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostComponents", x => x.ComponentID);
                    table.ForeignKey(
                        name: "FK_CostComponents_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostComponents_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CostVariances",
                columns: table => new
                {
                    VarianceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionOrderID = table.Column<int>(type: "int", nullable: false),
                    MaterialPriceVariance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MaterialUsageVariance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LaborVariance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OverheadVariance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    YieldVariance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalVarianceAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, computedColumnSql: "[MaterialPriceVariance] + [MaterialUsageVariance] + [LaborVariance] + [OverheadVariance] + [YieldVariance]", stored: true),
                    TotalVariancePercent = table.Column<decimal>(type: "decimal(9,4)", precision: 9, scale: 4, nullable: true),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReviewStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReviewedByUserID = table.Column<int>(type: "int", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewComments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostVariances", x => x.VarianceID);
                    table.ForeignKey(
                        name: "FK_CostVariances_ProductionOrders_ProductionOrderID",
                        column: x => x.ProductionOrderID,
                        principalTable: "ProductionOrders",
                        principalColumn: "ProductionOrderID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostVariances_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostVariances_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostVariances_Users_ReviewedByUserID",
                        column: x => x.ReviewedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StandardItemCosts",
                columns: table => new
                {
                    CostID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StandardMaterialCost = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    StandardLaborCost = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    StandardOverheadCost = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalStandardCost = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, computedColumnSql: "[StandardMaterialCost] + [StandardLaborCost] + [StandardOverheadCost]", stored: true),
                    CostUOMID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApprovedByUserID = table.Column<int>(type: "int", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardItemCosts", x => x.CostID);
                    table.ForeignKey(
                        name: "FK_StandardItemCosts_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StandardItemCosts_UnitsOfMeasure_CostUOMID",
                        column: x => x.CostUOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StandardItemCosts_Users_ApprovedByUserID",
                        column: x => x.ApprovedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StandardItemCosts_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StandardItemCosts_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VarianceThresholds",
                columns: table => new
                {
                    ThresholdID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    VarianceType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WarningPercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CriticalPercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VarianceThresholds", x => x.ThresholdID);
                    table.ForeignKey(
                        name: "FK_VarianceThresholds_Plants_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plants",
                        principalColumn: "PlantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VarianceThresholds_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VarianceThresholds_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VarianceThresholds_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BatchCostLines",
                columns: table => new
                {
                    CostLineID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchCostID = table.Column<int>(type: "int", nullable: false),
                    ComponentID = table.Column<int>(type: "int", nullable: false),
                    SourceType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SourceID = table.Column<long>(type: "bigint", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, computedColumnSql: "ROUND([Quantity] * [Rate], 2)", stored: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchCostLines", x => x.CostLineID);
                    table.ForeignKey(
                        name: "FK_BatchCostLines_BatchCostSummaries_BatchCostID",
                        column: x => x.BatchCostID,
                        principalTable: "BatchCostSummaries",
                        principalColumn: "BatchCostID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BatchCostLines_CostComponents_ComponentID",
                        column: x => x.ComponentID,
                        principalTable: "CostComponents",
                        principalColumn: "ComponentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchCostLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OverheadAllocations",
                columns: table => new
                {
                    AllocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantID = table.Column<int>(type: "int", nullable: false),
                    ComponentID = table.Column<int>(type: "int", nullable: false),
                    PeriodMonth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AllocatedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BasisQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RatePerBasis = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, computedColumnSql: "CASE WHEN [BasisQty] = 0 THEN 0 ELSE [AllocatedAmount] / [BasisQty] END", stored: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PostedByUserID = table.Column<int>(type: "int", nullable: true),
                    PostedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverheadAllocations", x => x.AllocationID);
                    table.ForeignKey(
                        name: "FK_OverheadAllocations_CostComponents_ComponentID",
                        column: x => x.ComponentID,
                        principalTable: "CostComponents",
                        principalColumn: "ComponentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OverheadAllocations_Plants_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plants",
                        principalColumn: "PlantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OverheadAllocations_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OverheadAllocations_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OverheadAllocations_Users_PostedByUserID",
                        column: x => x.PostedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchCostLines_BatchCostID_ComponentID",
                table: "BatchCostLines",
                columns: new[] { "BatchCostID", "ComponentID" });

            migrationBuilder.CreateIndex(
                name: "IX_BatchCostLines_ComponentID",
                table: "BatchCostLines",
                column: "ComponentID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchCostLines_ProductID",
                table: "BatchCostLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchCostSummaries_CreatedByUserID",
                table: "BatchCostSummaries",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchCostSummaries_FinalizedByUserID",
                table: "BatchCostSummaries",
                column: "FinalizedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchCostSummaries_ModifiedByUserID",
                table: "BatchCostSummaries",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchCostSummaries_ProductionOrderID",
                table: "BatchCostSummaries",
                column: "ProductionOrderID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BatchCostSummaries_Status",
                table: "BatchCostSummaries",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CostComponents_ComponentCode",
                table: "CostComponents",
                column: "ComponentCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostComponents_CreatedByUserID",
                table: "CostComponents",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CostComponents_ModifiedByUserID",
                table: "CostComponents",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CostVariances_CreatedByUserID",
                table: "CostVariances",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CostVariances_ModifiedByUserID",
                table: "CostVariances",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CostVariances_ProductionOrderID",
                table: "CostVariances",
                column: "ProductionOrderID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostVariances_ReviewedByUserID",
                table: "CostVariances",
                column: "ReviewedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CostVariances_ReviewStatus_Severity",
                table: "CostVariances",
                columns: new[] { "ReviewStatus", "Severity" });

            migrationBuilder.CreateIndex(
                name: "IX_OverheadAllocations_ComponentID",
                table: "OverheadAllocations",
                column: "ComponentID");

            migrationBuilder.CreateIndex(
                name: "IX_OverheadAllocations_CreatedByUserID",
                table: "OverheadAllocations",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_OverheadAllocations_ModifiedByUserID",
                table: "OverheadAllocations",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_OverheadAllocations_PlantID_ComponentID_PeriodMonth",
                table: "OverheadAllocations",
                columns: new[] { "PlantID", "ComponentID", "PeriodMonth" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OverheadAllocations_PostedByUserID",
                table: "OverheadAllocations",
                column: "PostedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_StandardItemCosts_ApprovedByUserID",
                table: "StandardItemCosts",
                column: "ApprovedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_StandardItemCosts_CostUOMID",
                table: "StandardItemCosts",
                column: "CostUOMID");

            migrationBuilder.CreateIndex(
                name: "IX_StandardItemCosts_CreatedByUserID",
                table: "StandardItemCosts",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_StandardItemCosts_ModifiedByUserID",
                table: "StandardItemCosts",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_StandardItemCosts_ProductID_EffectiveFrom",
                table: "StandardItemCosts",
                columns: new[] { "ProductID", "EffectiveFrom" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_StandardItemCosts_OneApprovedPerProduct",
                table: "StandardItemCosts",
                column: "ProductID",
                unique: true,
                filter: "[Status] = 'Approved' AND [EffectiveTo] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VarianceThresholds_CreatedByUserID",
                table: "VarianceThresholds",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_VarianceThresholds_ModifiedByUserID",
                table: "VarianceThresholds",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_VarianceThresholds_PlantID_ProductID_VarianceType",
                table: "VarianceThresholds",
                columns: new[] { "PlantID", "ProductID", "VarianceType" },
                unique: true,
                filter: "[PlantID] IS NOT NULL AND [ProductID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VarianceThresholds_ProductID",
                table: "VarianceThresholds",
                column: "ProductID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchCostLines");

            migrationBuilder.DropTable(
                name: "CostVariances");

            migrationBuilder.DropTable(
                name: "OverheadAllocations");

            migrationBuilder.DropTable(
                name: "StandardItemCosts");

            migrationBuilder.DropTable(
                name: "VarianceThresholds");

            migrationBuilder.DropTable(
                name: "BatchCostSummaries");

            migrationBuilder.DropTable(
                name: "CostComponents");
        }
    }
}
