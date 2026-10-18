using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOps_ERP.Migrations
{
    /// <inheritdoc />
    public partial class Group3_Module32_ProductionAndManufacturing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Demands",
                columns: table => new
                {
                    DemandID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PlantID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UOMID = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Priority = table.Column<byte>(type: "tinyint", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demands", x => x.DemandID);
                    table.ForeignKey(
                        name: "FK_Demands_Plants_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plants",
                        principalColumn: "PlantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Demands_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Demands_UnitsOfMeasure_UOMID",
                        column: x => x.UOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Demands_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Demands_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DowntimeReasons",
                columns: table => new
                {
                    ReasonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReasonCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReasonName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SubCategory = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DowntimeReasons", x => x.ReasonID);
                    table.ForeignKey(
                        name: "FK_DowntimeReasons_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DowntimeReasons_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductionPlans",
                columns: table => new
                {
                    PlanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PlantID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_ProductionPlans", x => x.PlanID);
                    table.ForeignKey(
                        name: "FK_ProductionPlans_Plants_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plants",
                        principalColumn: "PlantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionPlans_Users_ApprovedByUserID",
                        column: x => x.ApprovedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionPlans_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionPlans_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductionStatusHistory",
                columns: table => new
                {
                    HistoryID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EntityID = table.Column<long>(type: "bigint", nullable: false),
                    FromStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ToStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ChangedByUserID = table.Column<int>(type: "int", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionStatusHistory", x => x.HistoryID);
                    table.ForeignKey(
                        name: "FK_ProductionStatusHistory_Users_ChangedByUserID",
                        column: x => x.ChangedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkCenters",
                columns: table => new
                {
                    WorkCenterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantID = table.Column<int>(type: "int", nullable: false),
                    WorkCenterCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WorkCenterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CenterType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CapacityPerHour = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    CapacityUOMID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkCenters", x => x.WorkCenterID);
                    table.ForeignKey(
                        name: "FK_WorkCenters_Plants_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plants",
                        principalColumn: "PlantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkCenters_UnitsOfMeasure_CapacityUOMID",
                        column: x => x.CapacityUOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkCenters_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkCenters_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductionPlanLines",
                columns: table => new
                {
                    PlanLineID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanID = table.Column<int>(type: "int", nullable: false),
                    LineNo = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    RecipeID = table.Column<int>(type: "int", nullable: false),
                    PlannedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UOMID = table.Column<int>(type: "int", nullable: false),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DemandID = table.Column<int>(type: "int", nullable: true),
                    Priority = table.Column<byte>(type: "tinyint", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPlanLines", x => x.PlanLineID);
                    table.ForeignKey(
                        name: "FK_ProductionPlanLines_Demands_DemandID",
                        column: x => x.DemandID,
                        principalTable: "Demands",
                        principalColumn: "DemandID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionPlanLines_ProductionPlans_PlanID",
                        column: x => x.PlanID,
                        principalTable: "ProductionPlans",
                        principalColumn: "PlanID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionPlanLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionPlanLines_Recipes_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Recipes",
                        principalColumn: "RecipeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionPlanLines_UnitsOfMeasure_UOMID",
                        column: x => x.UOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductionRoutings",
                columns: table => new
                {
                    RoutingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    WorkCenterID = table.Column<int>(type: "int", nullable: false),
                    SequenceOrder = table.Column<int>(type: "int", nullable: false),
                    OperationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StandardMinutes = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    SetupMinutes = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionRoutings", x => x.RoutingID);
                    table.ForeignKey(
                        name: "FK_ProductionRoutings_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionRoutings_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionRoutings_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionRoutings_WorkCenters_WorkCenterID",
                        column: x => x.WorkCenterID,
                        principalTable: "WorkCenters",
                        principalColumn: "WorkCenterID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanMaterialRequirements",
                columns: table => new
                {
                    RequirementID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanLineID = table.Column<int>(type: "int", nullable: false),
                    IngredientProductID = table.Column<int>(type: "int", nullable: false),
                    RequiredQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UOMID = table.Column<int>(type: "int", nullable: false),
                    AvailableQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ShortageQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, computedColumnSql: "CASE WHEN [RequiredQty] > [AvailableQty] THEN [RequiredQty] - [AvailableQty] ELSE CONVERT(decimal(18,4), 0) END", stored: true),
                    SuggestedAction = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CalculatedByUserID = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanMaterialRequirements", x => x.RequirementID);
                    table.ForeignKey(
                        name: "FK_PlanMaterialRequirements_ProductionPlanLines_PlanLineID",
                        column: x => x.PlanLineID,
                        principalTable: "ProductionPlanLines",
                        principalColumn: "PlanLineID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanMaterialRequirements_Products_IngredientProductID",
                        column: x => x.IngredientProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanMaterialRequirements_UnitsOfMeasure_UOMID",
                        column: x => x.UOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanMaterialRequirements_Users_CalculatedByUserID",
                        column: x => x.CalculatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductionOrders",
                columns: table => new
                {
                    ProductionOrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PlantID = table.Column<int>(type: "int", nullable: false),
                    PlanLineID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    RecipeID = table.Column<int>(type: "int", nullable: false),
                    TargetQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ProducedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RejectedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UOMID = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<byte>(type: "tinyint", nullable: false),
                    PlannedStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlannedEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOrders", x => x.ProductionOrderID);
                    table.ForeignKey(
                        name: "FK_ProductionOrders_Plants_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plants",
                        principalColumn: "PlantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionOrders_ProductionPlanLines_PlanLineID",
                        column: x => x.PlanLineID,
                        principalTable: "ProductionPlanLines",
                        principalColumn: "PlanLineID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionOrders_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionOrders_Recipes_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Recipes",
                        principalColumn: "RecipeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionOrders_UnitsOfMeasure_UOMID",
                        column: x => x.UOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionOrders_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionOrders_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BatchMaterialIssues",
                columns: table => new
                {
                    IssueID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionOrderID = table.Column<int>(type: "int", nullable: false),
                    BatchID = table.Column<long>(type: "bigint", nullable: false),
                    RecipeItemID = table.Column<int>(type: "int", nullable: true),
                    IssueType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IssuedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UOMID = table.Column<int>(type: "int", nullable: false),
                    InventoryTransactionID = table.Column<long>(type: "bigint", nullable: true),
                    IssuedByUserID = table.Column<int>(type: "int", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchMaterialIssues", x => x.IssueID);
                    table.ForeignKey(
                        name: "FK_BatchMaterialIssues_InventoryBatches_BatchID",
                        column: x => x.BatchID,
                        principalTable: "InventoryBatches",
                        principalColumn: "BatchID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchMaterialIssues_InventoryTransactions_InventoryTransactionID",
                        column: x => x.InventoryTransactionID,
                        principalTable: "InventoryTransactions",
                        principalColumn: "TransactionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchMaterialIssues_ProductionOrders_ProductionOrderID",
                        column: x => x.ProductionOrderID,
                        principalTable: "ProductionOrders",
                        principalColumn: "ProductionOrderID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchMaterialIssues_RecipeItems_RecipeItemID",
                        column: x => x.RecipeItemID,
                        principalTable: "RecipeItems",
                        principalColumn: "RecipeItemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchMaterialIssues_UnitsOfMeasure_UOMID",
                        column: x => x.UOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchMaterialIssues_Users_IssuedByUserID",
                        column: x => x.IssuedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BatchOutputLogs",
                columns: table => new
                {
                    OutputID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionOrderID = table.Column<int>(type: "int", nullable: false),
                    OutputBatchID = table.Column<long>(type: "bigint", nullable: false),
                    ProducedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RejectedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UOMID = table.Column<int>(type: "int", nullable: false),
                    PackagingConfigID = table.Column<int>(type: "int", nullable: true),
                    RecordedByUserID = table.Column<int>(type: "int", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Remarks = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchOutputLogs", x => x.OutputID);
                    table.ForeignKey(
                        name: "FK_BatchOutputLogs_InventoryBatches_OutputBatchID",
                        column: x => x.OutputBatchID,
                        principalTable: "InventoryBatches",
                        principalColumn: "BatchID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchOutputLogs_PackagingConfigs_PackagingConfigID",
                        column: x => x.PackagingConfigID,
                        principalTable: "PackagingConfigs",
                        principalColumn: "PackagingConfigID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchOutputLogs_ProductionOrders_ProductionOrderID",
                        column: x => x.ProductionOrderID,
                        principalTable: "ProductionOrders",
                        principalColumn: "ProductionOrderID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchOutputLogs_UnitsOfMeasure_UOMID",
                        column: x => x.UOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchOutputLogs_Users_RecordedByUserID",
                        column: x => x.RecordedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DowntimeLogs",
                columns: table => new
                {
                    DowntimeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkCenterID = table.Column<int>(type: "int", nullable: false),
                    ProductionOrderID = table.Column<int>(type: "int", nullable: true),
                    ReasonID = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationMinutes = table.Column<int>(type: "int", nullable: true, computedColumnSql: "DATEDIFF(MINUTE, [StartTime], [EndTime])", stored: true),
                    Remarks = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DowntimeLogs", x => x.DowntimeID);
                    table.ForeignKey(
                        name: "FK_DowntimeLogs_DowntimeReasons_ReasonID",
                        column: x => x.ReasonID,
                        principalTable: "DowntimeReasons",
                        principalColumn: "ReasonID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DowntimeLogs_ProductionOrders_ProductionOrderID",
                        column: x => x.ProductionOrderID,
                        principalTable: "ProductionOrders",
                        principalColumn: "ProductionOrderID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DowntimeLogs_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DowntimeLogs_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DowntimeLogs_WorkCenters_WorkCenterID",
                        column: x => x.WorkCenterID,
                        principalTable: "WorkCenters",
                        principalColumn: "WorkCenterID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductionExceptions",
                columns: table => new
                {
                    ExceptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExceptionNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ProductionOrderID = table.Column<int>(type: "int", nullable: true),
                    WorkCenterID = table.Column<int>(type: "int", nullable: true),
                    ExceptionType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AssignedToUserID = table.Column<int>(type: "int", nullable: true),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ResolvedByUserID = table.Column<int>(type: "int", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionExceptions", x => x.ExceptionID);
                    table.ForeignKey(
                        name: "FK_ProductionExceptions_ProductionOrders_ProductionOrderID",
                        column: x => x.ProductionOrderID,
                        principalTable: "ProductionOrders",
                        principalColumn: "ProductionOrderID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionExceptions_Users_AssignedToUserID",
                        column: x => x.AssignedToUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionExceptions_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionExceptions_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionExceptions_Users_ResolvedByUserID",
                        column: x => x.ResolvedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionExceptions_WorkCenters_WorkCenterID",
                        column: x => x.WorkCenterID,
                        principalTable: "WorkCenters",
                        principalColumn: "WorkCenterID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductionOperations",
                columns: table => new
                {
                    OperationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionOrderID = table.Column<int>(type: "int", nullable: false),
                    RoutingID = table.Column<int>(type: "int", nullable: true),
                    WorkCenterID = table.Column<int>(type: "int", nullable: false),
                    SequenceOrder = table.Column<int>(type: "int", nullable: false),
                    OperationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StandardMinutes = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualMinutes = table.Column<int>(type: "int", nullable: true, computedColumnSql: "DATEDIFF(MINUTE, [StartTime], [EndTime])", stored: true),
                    OperatorUserID = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOperations", x => x.OperationID);
                    table.ForeignKey(
                        name: "FK_ProductionOperations_ProductionOrders_ProductionOrderID",
                        column: x => x.ProductionOrderID,
                        principalTable: "ProductionOrders",
                        principalColumn: "ProductionOrderID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionOperations_ProductionRoutings_RoutingID",
                        column: x => x.RoutingID,
                        principalTable: "ProductionRoutings",
                        principalColumn: "RoutingID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionOperations_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionOperations_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionOperations_Users_OperatorUserID",
                        column: x => x.OperatorUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionOperations_WorkCenters_WorkCenterID",
                        column: x => x.WorkCenterID,
                        principalTable: "WorkCenters",
                        principalColumn: "WorkCenterID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductionWastages",
                columns: table => new
                {
                    WastageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionOrderID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    BatchID = table.Column<long>(type: "bigint", nullable: true),
                    WorkCenterID = table.Column<int>(type: "int", nullable: true),
                    WastageType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UOMID = table.Column<int>(type: "int", nullable: false),
                    Stage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    RecordedByUserID = table.Column<int>(type: "int", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionWastages", x => x.WastageID);
                    table.ForeignKey(
                        name: "FK_ProductionWastages_InventoryBatches_BatchID",
                        column: x => x.BatchID,
                        principalTable: "InventoryBatches",
                        principalColumn: "BatchID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionWastages_ProductionOrders_ProductionOrderID",
                        column: x => x.ProductionOrderID,
                        principalTable: "ProductionOrders",
                        principalColumn: "ProductionOrderID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionWastages_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionWastages_UnitsOfMeasure_UOMID",
                        column: x => x.UOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionWastages_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionWastages_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionWastages_Users_RecordedByUserID",
                        column: x => x.RecordedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionWastages_WorkCenters_WorkCenterID",
                        column: x => x.WorkCenterID,
                        principalTable: "WorkCenters",
                        principalColumn: "WorkCenterID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchMaterialIssues_BatchID",
                table: "BatchMaterialIssues",
                column: "BatchID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchMaterialIssues_InventoryTransactionID",
                table: "BatchMaterialIssues",
                column: "InventoryTransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchMaterialIssues_IssuedByUserID",
                table: "BatchMaterialIssues",
                column: "IssuedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchMaterialIssues_ProductionOrderID",
                table: "BatchMaterialIssues",
                column: "ProductionOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchMaterialIssues_RecipeItemID",
                table: "BatchMaterialIssues",
                column: "RecipeItemID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchMaterialIssues_UOMID",
                table: "BatchMaterialIssues",
                column: "UOMID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchOutputLogs_OutputBatchID",
                table: "BatchOutputLogs",
                column: "OutputBatchID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BatchOutputLogs_PackagingConfigID",
                table: "BatchOutputLogs",
                column: "PackagingConfigID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchOutputLogs_ProductionOrderID",
                table: "BatchOutputLogs",
                column: "ProductionOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchOutputLogs_RecordedByUserID",
                table: "BatchOutputLogs",
                column: "RecordedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchOutputLogs_UOMID",
                table: "BatchOutputLogs",
                column: "UOMID");

            migrationBuilder.CreateIndex(
                name: "IX_Demands_CreatedByUserID",
                table: "Demands",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Demands_DemandCode",
                table: "Demands",
                column: "DemandCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Demands_ModifiedByUserID",
                table: "Demands",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Demands_PlantID_Status_RequiredDate",
                table: "Demands",
                columns: new[] { "PlantID", "Status", "RequiredDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Demands_ProductID",
                table: "Demands",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Demands_UOMID",
                table: "Demands",
                column: "UOMID");

            migrationBuilder.CreateIndex(
                name: "IX_DowntimeLogs_CreatedByUserID",
                table: "DowntimeLogs",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_DowntimeLogs_ModifiedByUserID",
                table: "DowntimeLogs",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_DowntimeLogs_ProductionOrderID",
                table: "DowntimeLogs",
                column: "ProductionOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_DowntimeLogs_ReasonID",
                table: "DowntimeLogs",
                column: "ReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_DowntimeLogs_WorkCenterID_StartTime",
                table: "DowntimeLogs",
                columns: new[] { "WorkCenterID", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "UX_DowntimeLogs_OneOpenPerWorkCenter",
                table: "DowntimeLogs",
                column: "WorkCenterID",
                unique: true,
                filter: "[EndTime] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DowntimeReasons_CreatedByUserID",
                table: "DowntimeReasons",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_DowntimeReasons_ModifiedByUserID",
                table: "DowntimeReasons",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_DowntimeReasons_ReasonCode",
                table: "DowntimeReasons",
                column: "ReasonCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanMaterialRequirements_CalculatedByUserID",
                table: "PlanMaterialRequirements",
                column: "CalculatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanMaterialRequirements_IngredientProductID",
                table: "PlanMaterialRequirements",
                column: "IngredientProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanMaterialRequirements_PlanLineID_IngredientProductID",
                table: "PlanMaterialRequirements",
                columns: new[] { "PlanLineID", "IngredientProductID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanMaterialRequirements_ShortageQty_IngredientProductID",
                table: "PlanMaterialRequirements",
                columns: new[] { "ShortageQty", "IngredientProductID" });

            migrationBuilder.CreateIndex(
                name: "IX_PlanMaterialRequirements_UOMID",
                table: "PlanMaterialRequirements",
                column: "UOMID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionExceptions_AssignedToUserID",
                table: "ProductionExceptions",
                column: "AssignedToUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionExceptions_CreatedByUserID",
                table: "ProductionExceptions",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionExceptions_ExceptionNo",
                table: "ProductionExceptions",
                column: "ExceptionNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionExceptions_ModifiedByUserID",
                table: "ProductionExceptions",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionExceptions_ProductionOrderID",
                table: "ProductionExceptions",
                column: "ProductionOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionExceptions_ResolvedByUserID",
                table: "ProductionExceptions",
                column: "ResolvedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionExceptions_Status_Severity",
                table: "ProductionExceptions",
                columns: new[] { "Status", "Severity" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionExceptions_WorkCenterID",
                table: "ProductionExceptions",
                column: "WorkCenterID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOperations_CreatedByUserID",
                table: "ProductionOperations",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOperations_ModifiedByUserID",
                table: "ProductionOperations",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOperations_OperatorUserID",
                table: "ProductionOperations",
                column: "OperatorUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOperations_ProductionOrderID_SequenceOrder",
                table: "ProductionOperations",
                columns: new[] { "ProductionOrderID", "SequenceOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOperations_RoutingID",
                table: "ProductionOperations",
                column: "RoutingID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOperations_WorkCenterID_Status",
                table: "ProductionOperations",
                columns: new[] { "WorkCenterID", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_CreatedByUserID",
                table: "ProductionOrders",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_ModifiedByUserID",
                table: "ProductionOrders",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_OrderNumber",
                table: "ProductionOrders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_PlanLineID",
                table: "ProductionOrders",
                column: "PlanLineID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_PlantID_Status_PlannedStart",
                table: "ProductionOrders",
                columns: new[] { "PlantID", "Status", "PlannedStart" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_ProductID",
                table: "ProductionOrders",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_RecipeID",
                table: "ProductionOrders",
                column: "RecipeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_UOMID",
                table: "ProductionOrders",
                column: "UOMID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanLines_DemandID",
                table: "ProductionPlanLines",
                column: "DemandID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanLines_PlanID_LineNo",
                table: "ProductionPlanLines",
                columns: new[] { "PlanID", "LineNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanLines_ProductID",
                table: "ProductionPlanLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanLines_RecipeID",
                table: "ProductionPlanLines",
                column: "RecipeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanLines_UOMID",
                table: "ProductionPlanLines",
                column: "UOMID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlans_ApprovedByUserID",
                table: "ProductionPlans",
                column: "ApprovedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlans_CreatedByUserID",
                table: "ProductionPlans",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlans_ModifiedByUserID",
                table: "ProductionPlans",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlans_PlanCode",
                table: "ProductionPlans",
                column: "PlanCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlans_PlantID_Status_StartDate",
                table: "ProductionPlans",
                columns: new[] { "PlantID", "Status", "StartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionRoutings_CreatedByUserID",
                table: "ProductionRoutings",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionRoutings_ModifiedByUserID",
                table: "ProductionRoutings",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionRoutings_ProductID_SequenceOrder",
                table: "ProductionRoutings",
                columns: new[] { "ProductID", "SequenceOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionRoutings_WorkCenterID",
                table: "ProductionRoutings",
                column: "WorkCenterID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionStatusHistory_ChangedByUserID",
                table: "ProductionStatusHistory",
                column: "ChangedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionStatusHistory_EntityType_EntityID_ChangedAt",
                table: "ProductionStatusHistory",
                columns: new[] { "EntityType", "EntityID", "ChangedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionWastages_BatchID",
                table: "ProductionWastages",
                column: "BatchID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionWastages_CreatedByUserID",
                table: "ProductionWastages",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionWastages_ModifiedByUserID",
                table: "ProductionWastages",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionWastages_ProductID",
                table: "ProductionWastages",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionWastages_ProductionOrderID",
                table: "ProductionWastages",
                column: "ProductionOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionWastages_RecordedByUserID",
                table: "ProductionWastages",
                column: "RecordedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionWastages_UOMID",
                table: "ProductionWastages",
                column: "UOMID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionWastages_WastageType_RecordedAt",
                table: "ProductionWastages",
                columns: new[] { "WastageType", "RecordedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionWastages_WorkCenterID",
                table: "ProductionWastages",
                column: "WorkCenterID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkCenters_CapacityUOMID",
                table: "WorkCenters",
                column: "CapacityUOMID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkCenters_CreatedByUserID",
                table: "WorkCenters",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkCenters_ModifiedByUserID",
                table: "WorkCenters",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkCenters_PlantID_WorkCenterCode",
                table: "WorkCenters",
                columns: new[] { "PlantID", "WorkCenterCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchMaterialIssues");

            migrationBuilder.DropTable(
                name: "BatchOutputLogs");

            migrationBuilder.DropTable(
                name: "DowntimeLogs");

            migrationBuilder.DropTable(
                name: "PlanMaterialRequirements");

            migrationBuilder.DropTable(
                name: "ProductionExceptions");

            migrationBuilder.DropTable(
                name: "ProductionOperations");

            migrationBuilder.DropTable(
                name: "ProductionStatusHistory");

            migrationBuilder.DropTable(
                name: "ProductionWastages");

            migrationBuilder.DropTable(
                name: "DowntimeReasons");

            migrationBuilder.DropTable(
                name: "ProductionRoutings");

            migrationBuilder.DropTable(
                name: "ProductionOrders");

            migrationBuilder.DropTable(
                name: "WorkCenters");

            migrationBuilder.DropTable(
                name: "ProductionPlanLines");

            migrationBuilder.DropTable(
                name: "Demands");

            migrationBuilder.DropTable(
                name: "ProductionPlans");
        }
    }
}
