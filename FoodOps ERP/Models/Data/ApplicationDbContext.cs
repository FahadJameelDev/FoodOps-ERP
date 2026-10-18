using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.Entities.Manufacturing;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ─────────────────────────────────────────────
        // Master Entities
        // ─────────────────────────────────────────────
        public DbSet<Plant> Plants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        // ─────────────────────────────────────────────
        // Procurement & Inventory Entities (Group 2)
        // ─────────────────────────────────────────────
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseBin> WarehouseBins { get; set; }
        public DbSet<InventoryBatch> InventoryBatches { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<StockAdjustment> StockAdjustments { get; set; }

        // Incoming QC
        public DbSet<QCSpecification> QCSpecifications { get; set; }
        public DbSet<QCInspection> QCInspections { get; set; }
        public DbSet<QCInspectionResult> QCInspectionResults { get; set; }
        public DbSet<HoldQuarantineLog> HoldQuarantineLogs { get; set; }
        public DbSet<NonConformanceReport> NonConformanceReports { get; set; }
        public DbSet<CertificateOfAnalysis> CertificatesOfAnalysis { get; set; }

        // ─────────────────────────────────────────────
        // Module 3.1 : Products & Recipes (Group 3)
        // ─────────────────────────────────────────────
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeItem> RecipeItems { get; set; }
        public DbSet<RecipeApproval> RecipeApprovals { get; set; }
        public DbSet<RecipeChangeRequest> RecipeChangeRequests { get; set; }
        public DbSet<PackagingConfig> PackagingConfigs { get; set; }

        // ─────────────────────────────────────────────
        // Module 3.2 : Production & Manufacturing (15 Tables)
        // ─────────────────────────────────────────────
        public DbSet<WorkCenter> WorkCenters { get; set; }
        public DbSet<ProductionRouting> ProductionRoutings { get; set; }
        public DbSet<Demand> Demands { get; set; }
        public DbSet<ProductionPlan> ProductionPlans { get; set; }
        public DbSet<ProductionPlanLine> ProductionPlanLines { get; set; }
        public DbSet<PlanMaterialRequirement> PlanMaterialRequirements { get; set; }
        public DbSet<ProductionStatusHistory> ProductionStatusHistory { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<ProductionOperation> ProductionOperations { get; set; }
        public DbSet<BatchMaterialIssue> BatchMaterialIssues { get; set; }
        public DbSet<BatchOutputLog> BatchOutputLogs { get; set; }
        public DbSet<ProductionWastage> ProductionWastages { get; set; }
        public DbSet<DowntimeReason> DowntimeReasons { get; set; }
        public DbSet<DowntimeLog> DowntimeLogs { get; set; }
        public DbSet<ProductionException> ProductionExceptions { get; set; }
        // ─────────────────────────────────────────────
        // Module 3.3 : Quality Control (2 New Tables)
        // ─────────────────────────────────────────────
        public DbSet<QCParameter> QCParameters { get; set; }
        public DbSet<QualityDecision> QualityDecisions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ─────────────────────────────────────────────
            // Master Entities Configuration
            // ─────────────────────────────────────────────

            // Plant
            modelBuilder.Entity<Plant>(entity =>
            {
                entity.HasKey(e => e.PlantID);
                entity.HasIndex(e => e.PlantCode).IsUnique();
            });

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasOne(e => e.Plant)
                      .WithMany()
                      .HasForeignKey(e => e.PlantID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // UnitOfMeasure
            modelBuilder.Entity<UnitOfMeasure>(entity =>
            {
                entity.HasKey(e => e.UOMID);
                entity.HasIndex(e => e.UOMCode).IsUnique();
            });

            // Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryID);
                entity.HasIndex(e => e.CategoryName).IsUnique();
            });

            // Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductID);
                entity.HasIndex(e => e.ProductCode).IsUnique();

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(e => e.CategoryID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.BaseUOM)
                      .WithMany()
                      .HasForeignKey(e => e.BaseUOMID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ─────────────────────────────────────────────
            // Procurement & Inventory Configuration
            // ─────────────────────────────────────────────

            // Supplier
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.SupplierID);
                entity.HasIndex(e => e.SupplierCode).IsUnique();
            });

            // PurchaseOrder
            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.HasKey(e => e.POID);
                entity.HasIndex(e => e.POCode).IsUnique();

                entity.HasOne(e => e.Supplier)
                      .WithMany(s => s.PurchaseOrders)
                      .HasForeignKey(e => e.SupplierID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Plant)
                      .WithMany()
                      .HasForeignKey(e => e.PlantID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // PurchaseOrderItem
            modelBuilder.Entity<PurchaseOrderItem>(entity =>
            {
                entity.HasKey(e => e.POItemID);

                entity.HasOne(e => e.PurchaseOrder)
                      .WithMany(po => po.PurchaseOrderItems)
                      .HasForeignKey(e => e.POID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Product)
                      .WithMany()
                      .HasForeignKey(e => e.ProductID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UOM)
                      .WithMany()
                      .HasForeignKey(e => e.UOMID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Warehouse
            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(e => e.WarehouseID);
                entity.HasIndex(e => new { e.PlantID, e.WarehouseCode }).IsUnique();

                entity.HasOne(e => e.Plant)
                      .WithMany()
                      .HasForeignKey(e => e.PlantID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // WarehouseBin
            modelBuilder.Entity<WarehouseBin>(entity =>
            {
                entity.HasKey(e => e.BinID);
                entity.HasIndex(e => new { e.WarehouseID, e.BinCode }).IsUnique();

                entity.HasOne(e => e.Warehouse)
                      .WithMany(w => w.WarehouseBins)
                      .HasForeignKey(e => e.WarehouseID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // InventoryBatch
            modelBuilder.Entity<InventoryBatch>(entity =>
            {
                entity.HasKey(e => e.BatchID);
                entity.HasIndex(e => e.BatchNumber);

                entity.HasOne(e => e.Product)
                      .WithMany()
                      .HasForeignKey(e => e.ProductID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Plant)
                      .WithMany()
                      .HasForeignKey(e => e.PlantID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Supplier)
                      .WithMany(s => s.InventoryBatches)
                      .HasForeignKey(e => e.SupplierID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.WarehouseBin)
                      .WithMany(b => b.InventoryBatches)
                      .HasForeignKey(e => e.BinID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // InventoryTransaction
            modelBuilder.Entity<InventoryTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionID);

                entity.HasOne(e => e.InventoryBatch)
                      .WithMany(b => b.InventoryTransactions)
                      .HasForeignKey(e => e.BatchID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Plant)
                      .WithMany()
                      .HasForeignKey(e => e.PlantID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // StockAdjustment
            modelBuilder.Entity<StockAdjustment>(entity =>
            {
                entity.HasKey(e => e.AdjustmentID);

                entity.HasOne(e => e.InventoryBatch)
                      .WithMany(b => b.StockAdjustments)
                      .HasForeignKey(e => e.BatchID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.AdjustedBy)
                      .WithMany()
                      .HasForeignKey(e => e.AdjustedByUserID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ─────────────────────────────────────────────
            // Incoming QC Configuration
            // ─────────────────────────────────────────────

            // QCSpecification
            modelBuilder.Entity<QCSpecification>(entity =>
            {
                entity.HasKey(e => e.SpecID);
                entity.HasIndex(e => new { e.ProductID, e.ParameterName }).IsUnique();

                entity.HasOne(e => e.Product)
                      .WithMany()
                      .HasForeignKey(e => e.ProductID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UOM)
                      .WithMany()
                      .HasForeignKey(e => e.UOMID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // QCInspection
            modelBuilder.Entity<QCInspection>(entity =>
            {
                entity.HasKey(e => e.InspectionID);

                entity.HasOne(e => e.InventoryBatch)
                      .WithMany(b => b.QCInspections)
                      .HasForeignKey(e => e.BatchID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Inspector)
                      .WithMany()
                      .HasForeignKey(e => e.InspectorUserID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // QCInspectionResult
            modelBuilder.Entity<QCInspectionResult>(entity =>
            {
                entity.HasKey(e => e.ResultID);

                entity.HasOne(e => e.QCInspection)
                      .WithMany(i => i.QCInspectionResults)
                      .HasForeignKey(e => e.InspectionID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.QCSpecification)
                      .WithMany(s => s.QCInspectionResults)
                      .HasForeignKey(e => e.SpecID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // HoldQuarantineLog
            modelBuilder.Entity<HoldQuarantineLog>(entity =>
            {
                entity.HasKey(e => e.HoldID);

                entity.HasOne(e => e.InventoryBatch)
                      .WithMany(b => b.HoldQuarantineLogs)
                      .HasForeignKey(e => e.BatchID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.HeldBy)
                      .WithMany()
                      .HasForeignKey(e => e.HeldByUserID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ReleasedBy)
                      .WithMany()
                      .HasForeignKey(e => e.ReleasedByUserID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // NonConformanceReport
            modelBuilder.Entity<NonConformanceReport>(entity =>
            {
                entity.HasKey(e => e.NCRID);

                entity.HasOne(e => e.QCInspection)
                      .WithMany(i => i.NonConformanceReports)
                      .HasForeignKey(e => e.InspectionID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // CertificateOfAnalysis
            modelBuilder.Entity<CertificateOfAnalysis>(entity =>
            {
                entity.HasKey(e => e.COAID);
                entity.HasIndex(e => e.COANumber).IsUnique();

                entity.HasOne(e => e.InventoryBatch)
                      .WithMany(b => b.CertificatesOfAnalysis)
                      .HasForeignKey(e => e.BatchID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ═════════════════════════════════════════════
            // MODULE 3.1 : PRODUCTS & RECIPES CONFIGURATION
            // ═════════════════════════════════════════════

            // Recipe
            modelBuilder.Entity<Recipe>(b =>
            {
                b.ToTable("Recipes");
                b.HasKey(x => x.RecipeID);
                b.Property(x => x.RecipeCode).HasMaxLength(30);
                b.Property(x => x.Status).HasMaxLength(20);
                b.Property(x => x.StandardBatchSize).HasPrecision(18, 4);
                b.Property(x => x.YieldPercentage).HasPrecision(5, 2);
                b.Property(x => x.ChangeSummary).HasMaxLength(500);

                b.HasIndex(x => x.RecipeCode).IsUnique();
                b.HasIndex(x => new { x.ProductID, x.VersionNo }).IsUnique();
                b.HasIndex(x => x.ProductID)
                 .IsUnique()
                 .HasFilter("[IsCurrent] = 1")
                 .HasDatabaseName("UX_Recipes_OneCurrentPerProduct");

                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.BatchUOMID).OnDelete(DeleteBehavior.Restrict);

                b.HasOne<User>().WithMany().HasForeignKey(x => x.ApprovedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // RecipeItem
            modelBuilder.Entity<RecipeItem>(b =>
            {
                b.ToTable("RecipeItems");
                b.HasKey(x => x.RecipeItemID);
                b.Property(x => x.RequiredQty).HasPrecision(18, 4);
                b.Property(x => x.ScrapPercent).HasPrecision(5, 2);
                b.Property(x => x.Remarks).HasMaxLength(200);

                b.HasIndex(x => new { x.RecipeID, x.LineNo }).IsUnique();
                b.HasIndex(x => new { x.RecipeID, x.IngredientProductID }).IsUnique();
                b.HasIndex(x => x.IngredientProductID);

                b.HasOne(x => x.Recipe)
                 .WithMany(r => r.Items)
                 .HasForeignKey(x => x.RecipeID)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne<Product>().WithMany().HasForeignKey(x => x.IngredientProductID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.UOMID).OnDelete(DeleteBehavior.Restrict);
            });

            // RecipeApproval
            modelBuilder.Entity<RecipeApproval>(b =>
            {
                b.ToTable("RecipeApprovals");
                b.HasKey(x => x.ApprovalID);
                b.Property(x => x.Decision).HasMaxLength(15);
                b.Property(x => x.Comments).HasMaxLength(500);

                b.HasIndex(x => new { x.RecipeID, x.StepNo, x.ApproverUserID }).IsUnique();
                b.HasIndex(x => new { x.ApproverUserID, x.Decision });

                b.HasOne(x => x.Recipe)
                 .WithMany(r => r.Approvals)
                 .HasForeignKey(x => x.RecipeID)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne<User>().WithMany().HasForeignKey(x => x.RequestedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ApproverUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // RecipeChangeRequest
            modelBuilder.Entity<RecipeChangeRequest>(b =>
            {
                b.ToTable("RecipeChangeRequests");
                b.HasKey(x => x.ChangeID);
                b.Property(x => x.ChangeNo).HasMaxLength(30);
                b.Property(x => x.ChangeType).HasMaxLength(30);
                b.Property(x => x.Reason).HasMaxLength(500);
                b.Property(x => x.ImpactAssessment).HasMaxLength(1000);
                b.Property(x => x.Status).HasMaxLength(20);

                b.HasIndex(x => x.ChangeNo).IsUnique();
                b.HasIndex(x => x.Status);
                b.HasIndex(x => x.RecipeID);

                // Yahan (r => r.ChangeRequests) specify kiya hai
                b.HasOne(x => x.Recipe)
                 .WithMany(r => r.ChangeRequests)
                 .HasForeignKey(x => x.RecipeID)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.NewRecipe)
                 .WithMany()
                 .HasForeignKey(x => x.NewRecipeID)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne<User>().WithMany().HasForeignKey(x => x.ApprovedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // PackagingConfig
            modelBuilder.Entity<PackagingConfig>(b =>
            {
                b.ToTable("PackagingConfigs");
                b.HasKey(x => x.PackagingConfigID);
                b.Property(x => x.PackageType).HasMaxLength(30);
                b.Property(x => x.PackageName).HasMaxLength(100);
                b.Property(x => x.NetWeight).HasPrecision(18, 4);
                b.Property(x => x.GrossWeight).HasPrecision(18, 4);
                b.Property(x => x.Barcode).HasMaxLength(50);

                b.HasIndex(x => new { x.ProductID, x.PackageType, x.UnitsPerPack }).IsUnique();
                b.HasIndex(x => x.ProductID)
                 .IsUnique()
                 .HasFilter("[IsDefault] = 1")
                 .HasDatabaseName("UX_PackagingConfigs_OneDefaultPerProduct");
                b.HasIndex(x => x.Barcode)
                 .IsUnique()
                 .HasFilter("[Barcode] IS NOT NULL");


                // Nayi line (Sahi):
                b.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.WeightUOMID).OnDelete(DeleteBehavior.Restrict);

                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });
            // ═════════════════════════════════════════════
            // MODULE 3.2 : PRODUCTION & MANUFACTURING
            // ═════════════════════════════════════════════

            // 1. WorkCenter
            modelBuilder.Entity<WorkCenter>(b =>
            {
                b.ToTable("WorkCenters");
                b.HasKey(x => x.WorkCenterID);

                b.Property(x => x.WorkCenterCode).HasMaxLength(20);
                b.Property(x => x.WorkCenterName).HasMaxLength(100);
                b.Property(x => x.CenterType).HasMaxLength(30);
                b.Property(x => x.CapacityPerHour).HasPrecision(18, 4);

                b.HasIndex(x => new { x.PlantID, x.WorkCenterCode }).IsUnique();

                b.HasOne<Plant>().WithMany().HasForeignKey(x => x.PlantID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.CapacityUOMID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 2. ProductionRouting
            modelBuilder.Entity<ProductionRouting>(b =>
            {
                b.ToTable("ProductionRoutings");
                b.HasKey(x => x.RoutingID);

                b.Property(x => x.OperationName).HasMaxLength(100);
                b.Property(x => x.StandardMinutes).HasPrecision(9, 2);
                b.Property(x => x.SetupMinutes).HasPrecision(9, 2);

                b.HasIndex(x => new { x.ProductID, x.SequenceOrder }).IsUnique();

                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<WorkCenter>().WithMany().HasForeignKey(x => x.WorkCenterID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 3. Demand
            modelBuilder.Entity<Demand>(b =>
            {
                b.ToTable("Demands");
                b.HasKey(x => x.DemandID);

                b.Property(x => x.DemandCode).HasMaxLength(30);
                b.Property(x => x.Quantity).HasPrecision(18, 4);
                b.Property(x => x.Source).HasMaxLength(20);
                b.Property(x => x.ReferenceNo).HasMaxLength(60);
                b.Property(x => x.Status).HasMaxLength(20);
                b.Property(x => x.Notes).HasMaxLength(500);

                b.HasIndex(x => x.DemandCode).IsUnique();
                b.HasIndex(x => new { x.PlantID, x.Status, x.RequiredDate });

                b.HasOne<Plant>().WithMany().HasForeignKey(x => x.PlantID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.UOMID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 4. ProductionPlan
            modelBuilder.Entity<ProductionPlan>(b =>
            {
                b.ToTable("ProductionPlans");
                b.HasKey(x => x.PlanID);

                b.Property(x => x.PlanCode).HasMaxLength(30);
                b.Property(x => x.Status).HasMaxLength(20);
                b.Property(x => x.Notes).HasMaxLength(500);

                b.HasIndex(x => x.PlanCode).IsUnique();
                b.HasIndex(x => new { x.PlantID, x.Status, x.StartDate });

                b.HasOne<Plant>().WithMany().HasForeignKey(x => x.PlantID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ApprovedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 5. ProductionPlanLine
            modelBuilder.Entity<ProductionPlanLine>(b =>
            {
                b.ToTable("ProductionPlanLines");
                b.HasKey(x => x.PlanLineID);

                b.Property(x => x.PlannedQty).HasPrecision(18, 4);
                b.Property(x => x.Status).HasMaxLength(20);

                b.HasIndex(x => new { x.PlanID, x.LineNo }).IsUnique();
                b.HasIndex(x => x.ProductID);
                b.HasIndex(x => x.DemandID);

                b.HasOne(x => x.Plan)
                 .WithMany(p => p.Lines)
                 .HasForeignKey(x => x.PlanID)
                 .OnDelete(DeleteBehavior.Cascade);   // CASCADE

                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<Recipe>().WithMany().HasForeignKey(x => x.RecipeID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.UOMID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<Demand>().WithMany().HasForeignKey(x => x.DemandID).OnDelete(DeleteBehavior.Restrict);
            });

            // 6. PlanMaterialRequirement
            modelBuilder.Entity<PlanMaterialRequirement>(b =>
            {
                b.ToTable("PlanMaterialRequirements");
                b.HasKey(x => x.RequirementID);

                b.Property(x => x.RequiredQty).HasPrecision(18, 4);
                b.Property(x => x.AvailableQty).HasPrecision(18, 4);
                b.Property(x => x.ShortageQty)
                 .HasPrecision(18, 4)
                 .HasComputedColumnSql(
                     "CASE WHEN [RequiredQty] > [AvailableQty] THEN [RequiredQty] - [AvailableQty] ELSE CONVERT(decimal(18,4), 0) END",
                     stored: true);
                b.Property(x => x.SuggestedAction).HasMaxLength(20);

                b.HasIndex(x => new { x.PlanLineID, x.IngredientProductID }).IsUnique();
                b.HasIndex(x => new { x.ShortageQty, x.IngredientProductID });

                b.HasOne(x => x.PlanLine)
                 .WithMany(l => l.Requirements)
                 .HasForeignKey(x => x.PlanLineID)
                 .OnDelete(DeleteBehavior.Cascade);   // CASCADE

                b.HasOne<Product>().WithMany().HasForeignKey(x => x.IngredientProductID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.UOMID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CalculatedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 7. ProductionStatusHistory (Append-only Log)
            modelBuilder.Entity<ProductionStatusHistory>(b =>
            {
                b.ToTable("ProductionStatusHistory");
                b.HasKey(x => x.HistoryID);

                b.Property(x => x.EntityType).HasMaxLength(20);
                b.Property(x => x.FromStatus).HasMaxLength(20);
                b.Property(x => x.ToStatus).HasMaxLength(20);
                b.Property(x => x.Reason).HasMaxLength(300);
                b.Property(x => x.ChangedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasIndex(x => new { x.EntityType, x.EntityID, x.ChangedAt });

                b.HasOne<User>().WithMany().HasForeignKey(x => x.ChangedByUserID).OnDelete(DeleteBehavior.Restrict);
                // EntityID is polymorphic — no FK
            });

            // 8. ProductionOrder
            modelBuilder.Entity<ProductionOrder>(b =>
            {
                b.ToTable("ProductionOrders");
                b.HasKey(x => x.ProductionOrderID);

                b.Property(x => x.OrderNumber).HasMaxLength(30);
                b.Property(x => x.TargetQty).HasPrecision(18, 4);
                b.Property(x => x.ProducedQty).HasPrecision(18, 4);
                b.Property(x => x.RejectedQty).HasPrecision(18, 4);
                b.Property(x => x.Status).HasMaxLength(20);
                b.Property(x => x.Remarks).HasMaxLength(300);

                b.HasIndex(x => x.OrderNumber).IsUnique();
                b.HasIndex(x => new { x.PlantID, x.Status, x.PlannedStart });
                b.HasIndex(x => x.ProductID);
                b.HasIndex(x => x.PlanLineID);

                b.HasOne<Plant>().WithMany().HasForeignKey(x => x.PlantID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<ProductionPlanLine>().WithMany().HasForeignKey(x => x.PlanLineID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<Recipe>().WithMany().HasForeignKey(x => x.RecipeID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.UOMID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 9. ProductionOperation
            modelBuilder.Entity<ProductionOperation>(b =>
            {
                b.ToTable("ProductionOperations");
                b.HasKey(x => x.OperationID);

                b.Property(x => x.OperationName).HasMaxLength(100);
                b.Property(x => x.StandardMinutes).HasPrecision(9, 2);
                b.Property(x => x.Status).HasMaxLength(20);
                b.Property(x => x.ActualMinutes)
                 .HasComputedColumnSql("DATEDIFF(MINUTE, [StartTime], [EndTime])", stored: true);

                b.HasIndex(x => new { x.ProductionOrderID, x.SequenceOrder }).IsUnique();
                b.HasIndex(x => new { x.WorkCenterID, x.Status });

                b.HasOne<ProductionOrder>().WithMany().HasForeignKey(x => x.ProductionOrderID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<ProductionRouting>().WithMany().HasForeignKey(x => x.RoutingID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<WorkCenter>().WithMany().HasForeignKey(x => x.WorkCenterID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.OperatorUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 10. BatchMaterialIssue (Immutable Log)
            modelBuilder.Entity<BatchMaterialIssue>(b =>
            {
                b.ToTable("BatchMaterialIssues");
                b.HasKey(x => x.IssueID);

                b.Property(x => x.IssueType).HasMaxLength(10);
                b.Property(x => x.IssuedQty).HasPrecision(18, 4);
                b.Property(x => x.IssuedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasIndex(x => x.ProductionOrderID);
                b.HasIndex(x => x.BatchID);

                b.HasOne<ProductionOrder>().WithMany().HasForeignKey(x => x.ProductionOrderID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<InventoryBatch>().WithMany().HasForeignKey(x => x.BatchID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<RecipeItem>().WithMany().HasForeignKey(x => x.RecipeItemID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.UOMID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<InventoryTransaction>().WithMany().HasForeignKey(x => x.InventoryTransactionID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.IssuedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 11. BatchOutputLog (Immutable Log)
            modelBuilder.Entity<BatchOutputLog>(b =>
            {
                b.ToTable("BatchOutputLogs");
                b.HasKey(x => x.OutputID);

                b.Property(x => x.ProducedQty).HasPrecision(18, 4);
                b.Property(x => x.RejectedQty).HasPrecision(18, 4);
                b.Property(x => x.RecordedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                b.Property(x => x.Remarks).HasMaxLength(300);

                b.HasIndex(x => x.OutputBatchID).IsUnique();
                b.HasIndex(x => x.ProductionOrderID);

                b.HasOne<ProductionOrder>().WithMany().HasForeignKey(x => x.ProductionOrderID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<InventoryBatch>().WithMany().HasForeignKey(x => x.OutputBatchID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.UOMID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<PackagingConfig>().WithMany().HasForeignKey(x => x.PackagingConfigID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.RecordedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 12. ProductionWastage
            modelBuilder.Entity<ProductionWastage>(b =>
            {
                b.ToTable("ProductionWastages");
                b.HasKey(x => x.WastageID);

                b.Property(x => x.WastageType).HasMaxLength(20);
                b.Property(x => x.Quantity).HasPrecision(18, 4);
                b.Property(x => x.Stage).HasMaxLength(50);
                b.Property(x => x.Reason).HasMaxLength(300);
                b.Property(x => x.RecordedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasIndex(x => x.ProductionOrderID);
                b.HasIndex(x => new { x.WastageType, x.RecordedAt });

                b.HasOne<ProductionOrder>().WithMany().HasForeignKey(x => x.ProductionOrderID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<InventoryBatch>().WithMany().HasForeignKey(x => x.BatchID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<WorkCenter>().WithMany().HasForeignKey(x => x.WorkCenterID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.UOMID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.RecordedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 13. DowntimeReason
            modelBuilder.Entity<DowntimeReason>(b =>
            {
                b.ToTable("DowntimeReasons");
                b.HasKey(x => x.ReasonID);

                b.Property(x => x.ReasonCode).HasMaxLength(20);
                b.Property(x => x.ReasonName).HasMaxLength(100);
                b.Property(x => x.Category).HasMaxLength(20);
                b.Property(x => x.SubCategory).HasMaxLength(20);

                b.HasIndex(x => x.ReasonCode).IsUnique();

                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 14. DowntimeLog
            modelBuilder.Entity<DowntimeLog>(b =>
            {
                b.ToTable("DowntimeLogs");
                b.HasKey(x => x.DowntimeID);

                b.Property(x => x.DurationMinutes)
                 .HasComputedColumnSql("DATEDIFF(MINUTE, [StartTime], [EndTime])", stored: true);
                b.Property(x => x.Remarks).HasMaxLength(300);

                // Ek machine par ek hi open downtime
                b.HasIndex(x => x.WorkCenterID)
                 .IsUnique()
                 .HasFilter("[EndTime] IS NULL")
                 .HasDatabaseName("UX_DowntimeLogs_OneOpenPerWorkCenter");

                b.HasIndex(x => new { x.WorkCenterID, x.StartTime });
                b.HasIndex(x => x.ReasonID);

                b.HasOne<WorkCenter>().WithMany().HasForeignKey(x => x.WorkCenterID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<ProductionOrder>().WithMany().HasForeignKey(x => x.ProductionOrderID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<DowntimeReason>().WithMany().HasForeignKey(x => x.ReasonID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 15. ProductionException
            modelBuilder.Entity<ProductionException>(b =>
            {
                b.ToTable("ProductionExceptions");
                b.HasKey(x => x.ExceptionID);

                b.Property(x => x.ExceptionNo).HasMaxLength(30);
                b.Property(x => x.ExceptionType).HasMaxLength(30);
                b.Property(x => x.Severity).HasMaxLength(20);
                b.Property(x => x.Description).HasMaxLength(1000);
                b.Property(x => x.Status).HasMaxLength(20);
                b.Property(x => x.ResolutionNotes).HasMaxLength(1000);

                b.HasIndex(x => x.ExceptionNo).IsUnique();
                b.HasIndex(x => new { x.Status, x.Severity });
                b.HasIndex(x => x.ProductionOrderID);

                b.HasOne<ProductionOrder>().WithMany().HasForeignKey(x => x.ProductionOrderID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<WorkCenter>().WithMany().HasForeignKey(x => x.WorkCenterID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.AssignedToUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ResolvedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });
            // ═════════════════════════════════════════════
            // MODULE 3.3 : QUALITY CONTROL (Nayi Tables)
            // ═════════════════════════════════════════════

            // 1. QCParameter
            modelBuilder.Entity<QCParameter>(b =>
            {
                b.ToTable("QCParameters");
                b.HasKey(x => x.ParameterID);

                b.Property(x => x.ParameterCode).HasMaxLength(20);
                b.Property(x => x.ParameterName).HasMaxLength(100);
                b.Property(x => x.ParameterType).HasMaxLength(20);

                b.HasIndex(x => x.ParameterCode).IsUnique();
                b.HasIndex(x => x.ParameterName).IsUnique();

                b.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.DefaultUOMID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 2. QualityDecision (Append-only Log)
            modelBuilder.Entity<QualityDecision>(b =>
            {
                b.ToTable("QualityDecisions");
                b.HasKey(x => x.DecisionID);

                b.Property(x => x.Decision).HasMaxLength(20);
                b.Property(x => x.Reason).HasMaxLength(500);
                b.Property(x => x.DecidedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                // Index for latest decision per batch
                b.HasIndex(x => new { x.BatchID, x.DecidedAt });

                // Foreign Keys to existing Group 2 tables
                b.HasOne<InventoryBatch>().WithMany().HasForeignKey(x => x.BatchID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<QCInspection>().WithMany().HasForeignKey(x => x.InspectionID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<NonConformanceReport>().WithMany().HasForeignKey(x => x.NCRID).OnDelete(DeleteBehavior.Restrict);

                // Foreign Keys to User (Audit)
                b.HasOne<User>().WithMany().HasForeignKey(x => x.DecidedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ApprovedByUserID).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}