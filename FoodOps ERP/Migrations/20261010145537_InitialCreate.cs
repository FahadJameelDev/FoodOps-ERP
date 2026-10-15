using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOps_ERP.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    PlantID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PlantName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.PlantID);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierID);
                });

            migrationBuilder.CreateTable(
                name: "UnitsOfMeasure",
                columns: table => new
                {
                    UOMID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UOMCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UOMName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitsOfMeasure", x => x.UOMID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantID = table.Column<int>(type: "int", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Plants_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plants",
                        principalColumn: "PlantID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    WarehouseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantID = table.Column<int>(type: "int", nullable: false),
                    WarehouseCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WarehouseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.WarehouseID);
                    table.ForeignKey(
                        name: "FK_Warehouses_Plants_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plants",
                        principalColumn: "PlantID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    POID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    PlantID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.POID);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Plants_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plants",
                        principalColumn: "PlantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ProductType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    BaseUOMID = table.Column<int>(type: "int", nullable: false),
                    MinStockLevel = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_UnitsOfMeasure_BaseUOMID",
                        column: x => x.BaseUOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseBins",
                columns: table => new
                {
                    BinID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseID = table.Column<int>(type: "int", nullable: false),
                    BinCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BinType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseBins", x => x.BinID);
                    table.ForeignKey(
                        name: "FK_WarehouseBins_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderItems",
                columns: table => new
                {
                    POItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    UOMID = table.Column<int>(type: "int", nullable: false),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ReceivedQty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderItems", x => x.POItemID);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_PurchaseOrders_POID",
                        column: x => x.POID,
                        principalTable: "PurchaseOrders",
                        principalColumn: "POID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_UnitsOfMeasure_UOMID",
                        column: x => x.UOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QCSpecifications",
                columns: table => new
                {
                    SpecID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    UOMID = table.Column<int>(type: "int", nullable: true),
                    ParameterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MinValue = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    MaxValue = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    TargetValue = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QCSpecifications", x => x.SpecID);
                    table.ForeignKey(
                        name: "FK_QCSpecifications_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QCSpecifications_UnitsOfMeasure_UOMID",
                        column: x => x.UOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryBatches",
                columns: table => new
                {
                    BatchID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    SupplierID = table.Column<int>(type: "int", nullable: true),
                    BinID = table.Column<int>(type: "int", nullable: true),
                    PlantID = table.Column<int>(type: "int", nullable: false),
                    ManufacturingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    QCStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryBatches", x => x.BatchID);
                    table.ForeignKey(
                        name: "FK_InventoryBatches_Plants_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plants",
                        principalColumn: "PlantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryBatches_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryBatches_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryBatches_WarehouseBins_BinID",
                        column: x => x.BinID,
                        principalTable: "WarehouseBins",
                        principalColumn: "BinID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CertificatesOfAnalysis",
                columns: table => new
                {
                    COAID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchID = table.Column<long>(type: "bigint", nullable: false),
                    COANumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssuedToCustomer = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GeneratedByUserID = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificatesOfAnalysis", x => x.COAID);
                    table.ForeignKey(
                        name: "FK_CertificatesOfAnalysis_InventoryBatches_BatchID",
                        column: x => x.BatchID,
                        principalTable: "InventoryBatches",
                        principalColumn: "BatchID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HoldQuarantineLogs",
                columns: table => new
                {
                    HoldID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchID = table.Column<long>(type: "bigint", nullable: false),
                    HoldReason = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    HeldByUserID = table.Column<int>(type: "int", nullable: false),
                    HeldAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReleasedByUserID = table.Column<int>(type: "int", nullable: true),
                    ReleasedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoldQuarantineLogs", x => x.HoldID);
                    table.ForeignKey(
                        name: "FK_HoldQuarantineLogs_InventoryBatches_BatchID",
                        column: x => x.BatchID,
                        principalTable: "InventoryBatches",
                        principalColumn: "BatchID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoldQuarantineLogs_Users_HeldByUserID",
                        column: x => x.HeldByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoldQuarantineLogs_Users_ReleasedByUserID",
                        column: x => x.ReleasedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactions",
                columns: table => new
                {
                    TransactionID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchID = table.Column<long>(type: "bigint", nullable: false),
                    PlantID = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ReferenceType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ReferenceID = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_InventoryBatches_BatchID",
                        column: x => x.BatchID,
                        principalTable: "InventoryBatches",
                        principalColumn: "BatchID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Plants_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plants",
                        principalColumn: "PlantID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QCInspections",
                columns: table => new
                {
                    InspectionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchID = table.Column<long>(type: "bigint", nullable: false),
                    ProductionOrderID = table.Column<int>(type: "int", nullable: true),
                    InspectorUserID = table.Column<int>(type: "int", nullable: false),
                    InspectionType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    OverallResult = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QCInspections", x => x.InspectionID);
                    table.ForeignKey(
                        name: "FK_QCInspections_InventoryBatches_BatchID",
                        column: x => x.BatchID,
                        principalTable: "InventoryBatches",
                        principalColumn: "BatchID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QCInspections_Users_InspectorUserID",
                        column: x => x.InspectorUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockAdjustments",
                columns: table => new
                {
                    AdjustmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchID = table.Column<long>(type: "bigint", nullable: false),
                    AdjustedQty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AdjustedByUserID = table.Column<int>(type: "int", nullable: false),
                    AdjustedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAdjustments", x => x.AdjustmentID);
                    table.ForeignKey(
                        name: "FK_StockAdjustments_InventoryBatches_BatchID",
                        column: x => x.BatchID,
                        principalTable: "InventoryBatches",
                        principalColumn: "BatchID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockAdjustments_Users_AdjustedByUserID",
                        column: x => x.AdjustedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NonConformanceReports",
                columns: table => new
                {
                    NCRID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionID = table.Column<int>(type: "int", nullable: false),
                    DefectDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RootCause = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CorrectiveAction = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonConformanceReports", x => x.NCRID);
                    table.ForeignKey(
                        name: "FK_NonConformanceReports_QCInspections_InspectionID",
                        column: x => x.InspectionID,
                        principalTable: "QCInspections",
                        principalColumn: "InspectionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QCInspectionResults",
                columns: table => new
                {
                    ResultID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionID = table.Column<int>(type: "int", nullable: false),
                    SpecID = table.Column<int>(type: "int", nullable: false),
                    MeasuredValue = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IsPassed = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QCInspectionResults", x => x.ResultID);
                    table.ForeignKey(
                        name: "FK_QCInspectionResults_QCInspections_InspectionID",
                        column: x => x.InspectionID,
                        principalTable: "QCInspections",
                        principalColumn: "InspectionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QCInspectionResults_QCSpecifications_SpecID",
                        column: x => x.SpecID,
                        principalTable: "QCSpecifications",
                        principalColumn: "SpecID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryName",
                table: "Categories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CertificatesOfAnalysis_BatchID",
                table: "CertificatesOfAnalysis",
                column: "BatchID");

            migrationBuilder.CreateIndex(
                name: "IX_CertificatesOfAnalysis_COANumber",
                table: "CertificatesOfAnalysis",
                column: "COANumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoldQuarantineLogs_BatchID",
                table: "HoldQuarantineLogs",
                column: "BatchID");

            migrationBuilder.CreateIndex(
                name: "IX_HoldQuarantineLogs_HeldByUserID",
                table: "HoldQuarantineLogs",
                column: "HeldByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_HoldQuarantineLogs_ReleasedByUserID",
                table: "HoldQuarantineLogs",
                column: "ReleasedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBatches_BatchNumber",
                table: "InventoryBatches",
                column: "BatchNumber");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBatches_BinID",
                table: "InventoryBatches",
                column: "BinID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBatches_PlantID",
                table: "InventoryBatches",
                column: "PlantID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBatches_ProductID",
                table: "InventoryBatches",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBatches_SupplierID",
                table: "InventoryBatches",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_BatchID",
                table: "InventoryTransactions",
                column: "BatchID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_PlantID",
                table: "InventoryTransactions",
                column: "PlantID");

            migrationBuilder.CreateIndex(
                name: "IX_NonConformanceReports_InspectionID",
                table: "NonConformanceReports",
                column: "InspectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_PlantCode",
                table: "Plants",
                column: "PlantCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_BaseUOMID",
                table: "Products",
                column: "BaseUOMID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCode",
                table: "Products",
                column: "ProductCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_POID",
                table: "PurchaseOrderItems",
                column: "POID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_ProductID",
                table: "PurchaseOrderItems",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_UOMID",
                table: "PurchaseOrderItems",
                column: "UOMID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PlantID",
                table: "PurchaseOrders",
                column: "PlantID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_POCode",
                table: "PurchaseOrders",
                column: "POCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierID",
                table: "PurchaseOrders",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_QCInspectionResults_InspectionID",
                table: "QCInspectionResults",
                column: "InspectionID");

            migrationBuilder.CreateIndex(
                name: "IX_QCInspectionResults_SpecID",
                table: "QCInspectionResults",
                column: "SpecID");

            migrationBuilder.CreateIndex(
                name: "IX_QCInspections_BatchID",
                table: "QCInspections",
                column: "BatchID");

            migrationBuilder.CreateIndex(
                name: "IX_QCInspections_InspectorUserID",
                table: "QCInspections",
                column: "InspectorUserID");

            migrationBuilder.CreateIndex(
                name: "IX_QCSpecifications_ProductID_ParameterName",
                table: "QCSpecifications",
                columns: new[] { "ProductID", "ParameterName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QCSpecifications_UOMID",
                table: "QCSpecifications",
                column: "UOMID");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_AdjustedByUserID",
                table: "StockAdjustments",
                column: "AdjustedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_BatchID",
                table: "StockAdjustments",
                column: "BatchID");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SupplierCode",
                table: "Suppliers",
                column: "SupplierCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnitsOfMeasure_UOMCode",
                table: "UnitsOfMeasure",
                column: "UOMCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PlantID",
                table: "Users",
                column: "PlantID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseBins_WarehouseID_BinCode",
                table: "WarehouseBins",
                columns: new[] { "WarehouseID", "BinCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_PlantID_WarehouseCode",
                table: "Warehouses",
                columns: new[] { "PlantID", "WarehouseCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificatesOfAnalysis");

            migrationBuilder.DropTable(
                name: "HoldQuarantineLogs");

            migrationBuilder.DropTable(
                name: "InventoryTransactions");

            migrationBuilder.DropTable(
                name: "NonConformanceReports");

            migrationBuilder.DropTable(
                name: "PurchaseOrderItems");

            migrationBuilder.DropTable(
                name: "QCInspectionResults");

            migrationBuilder.DropTable(
                name: "StockAdjustments");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "QCInspections");

            migrationBuilder.DropTable(
                name: "QCSpecifications");

            migrationBuilder.DropTable(
                name: "InventoryBatches");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "WarehouseBins");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "UnitsOfMeasure");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Plants");
        }
    }
}
