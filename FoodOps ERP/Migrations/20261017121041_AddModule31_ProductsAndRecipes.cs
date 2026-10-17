using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOps_ERP.Migrations
{
    /// <inheritdoc />
    public partial class AddModule31_ProductsAndRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackagingConfigs",
                columns: table => new
                {
                    PackagingConfigID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    PackageType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PackageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UnitsPerPack = table.Column<int>(type: "int", nullable: false),
                    NetWeight = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    GrossWeight = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    WeightUOMID = table.Column<int>(type: "int", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackagingConfigs", x => x.PackagingConfigID);
                    table.ForeignKey(
                        name: "FK_PackagingConfigs_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PackagingConfigs_UnitsOfMeasure_WeightUOMID",
                        column: x => x.WeightUOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PackagingConfigs_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PackagingConfigs_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    VersionNo = table.Column<int>(type: "int", nullable: false),
                    RecipeCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StandardBatchSize = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BatchUOMID = table.Column<int>(type: "int", nullable: false),
                    YieldPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedByUserID = table.Column<int>(type: "int", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChangeSummary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeID);
                    table.ForeignKey(
                        name: "FK_Recipes_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recipes_UnitsOfMeasure_BatchUOMID",
                        column: x => x.BatchUOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recipes_Users_ApprovedByUserID",
                        column: x => x.ApprovedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recipes_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recipes_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipeApprovals",
                columns: table => new
                {
                    ApprovalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeID = table.Column<int>(type: "int", nullable: false),
                    StepNo = table.Column<int>(type: "int", nullable: false),
                    RequestedByUserID = table.Column<int>(type: "int", nullable: false),
                    ApproverUserID = table.Column<int>(type: "int", nullable: false),
                    Decision = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DecisionAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeApprovals", x => x.ApprovalID);
                    table.ForeignKey(
                        name: "FK_RecipeApprovals_Recipes_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Recipes",
                        principalColumn: "RecipeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeApprovals_Users_ApproverUserID",
                        column: x => x.ApproverUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeApprovals_Users_RequestedByUserID",
                        column: x => x.RequestedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipeChangeRequests",
                columns: table => new
                {
                    ChangeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    RecipeID = table.Column<int>(type: "int", nullable: false),
                    NewRecipeID = table.Column<int>(type: "int", nullable: true),
                    ChangeType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImpactAssessment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApprovedByUserID = table.Column<int>(type: "int", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImplementedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeChangeRequests", x => x.ChangeID);
                    table.ForeignKey(
                        name: "FK_RecipeChangeRequests_Recipes_NewRecipeID",
                        column: x => x.NewRecipeID,
                        principalTable: "Recipes",
                        principalColumn: "RecipeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeChangeRequests_Recipes_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Recipes",
                        principalColumn: "RecipeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeChangeRequests_Users_ApprovedByUserID",
                        column: x => x.ApprovedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeChangeRequests_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeChangeRequests_Users_ModifiedByUserID",
                        column: x => x.ModifiedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipeItems",
                columns: table => new
                {
                    RecipeItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeID = table.Column<int>(type: "int", nullable: false),
                    LineNo = table.Column<int>(type: "int", nullable: false),
                    IngredientProductID = table.Column<int>(type: "int", nullable: false),
                    RequiredQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UOMID = table.Column<int>(type: "int", nullable: false),
                    ScrapPercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    IsCritical = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeItems", x => x.RecipeItemID);
                    table.ForeignKey(
                        name: "FK_RecipeItems_Products_IngredientProductID",
                        column: x => x.IngredientProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeItems_Recipes_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Recipes",
                        principalColumn: "RecipeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeItems_UnitsOfMeasure_UOMID",
                        column: x => x.UOMID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UOMID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackagingConfigs_Barcode",
                table: "PackagingConfigs",
                column: "Barcode",
                unique: true,
                filter: "[Barcode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PackagingConfigs_CreatedByUserID",
                table: "PackagingConfigs",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_PackagingConfigs_ModifiedByUserID",
                table: "PackagingConfigs",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_PackagingConfigs_ProductID_PackageType_UnitsPerPack",
                table: "PackagingConfigs",
                columns: new[] { "ProductID", "PackageType", "UnitsPerPack" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PackagingConfigs_WeightUOMID",
                table: "PackagingConfigs",
                column: "WeightUOMID");

            migrationBuilder.CreateIndex(
                name: "UX_PackagingConfigs_OneDefaultPerProduct",
                table: "PackagingConfigs",
                column: "ProductID",
                unique: true,
                filter: "[IsDefault] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeApprovals_ApproverUserID_Decision",
                table: "RecipeApprovals",
                columns: new[] { "ApproverUserID", "Decision" });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeApprovals_RecipeID_StepNo_ApproverUserID",
                table: "RecipeApprovals",
                columns: new[] { "RecipeID", "StepNo", "ApproverUserID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeApprovals_RequestedByUserID",
                table: "RecipeApprovals",
                column: "RequestedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeChangeRequests_ApprovedByUserID",
                table: "RecipeChangeRequests",
                column: "ApprovedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeChangeRequests_ChangeNo",
                table: "RecipeChangeRequests",
                column: "ChangeNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeChangeRequests_CreatedByUserID",
                table: "RecipeChangeRequests",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeChangeRequests_ModifiedByUserID",
                table: "RecipeChangeRequests",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeChangeRequests_NewRecipeID",
                table: "RecipeChangeRequests",
                column: "NewRecipeID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeChangeRequests_RecipeID",
                table: "RecipeChangeRequests",
                column: "RecipeID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeChangeRequests_Status",
                table: "RecipeChangeRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItems_IngredientProductID",
                table: "RecipeItems",
                column: "IngredientProductID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItems_RecipeID_IngredientProductID",
                table: "RecipeItems",
                columns: new[] { "RecipeID", "IngredientProductID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItems_RecipeID_LineNo",
                table: "RecipeItems",
                columns: new[] { "RecipeID", "LineNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItems_UOMID",
                table: "RecipeItems",
                column: "UOMID");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ApprovedByUserID",
                table: "Recipes",
                column: "ApprovedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_BatchUOMID",
                table: "Recipes",
                column: "BatchUOMID");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CreatedByUserID",
                table: "Recipes",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ModifiedByUserID",
                table: "Recipes",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ProductID_VersionNo",
                table: "Recipes",
                columns: new[] { "ProductID", "VersionNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_RecipeCode",
                table: "Recipes",
                column: "RecipeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Recipes_OneCurrentPerProduct",
                table: "Recipes",
                column: "ProductID",
                unique: true,
                filter: "[IsCurrent] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackagingConfigs");

            migrationBuilder.DropTable(
                name: "RecipeApprovals");

            migrationBuilder.DropTable(
                name: "RecipeChangeRequests");

            migrationBuilder.DropTable(
                name: "RecipeItems");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
