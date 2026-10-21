using FoodOpsERP.Models.AI.Entities;
using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.Entities.Manufacturing;
using FoodOpsERP.Models.Entities.Security;
using FoodOpsERP.Models.Workforce.HRPayroll.Entities;
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
        // ─────────────────────────────────────────────
        // Module 3.4 : Costing & Variance (7 Tables)
        // ─────────────────────────────────────────────
        public DbSet<CostComponent> CostComponents { get; set; }
        public DbSet<StandardItemCost> StandardItemCosts { get; set; }
        public DbSet<OverheadAllocation> OverheadAllocations { get; set; }
        public DbSet<BatchCostSummary> BatchCostSummaries { get; set; }
        public DbSet<BatchCostLine> BatchCostLines { get; set; }
        public DbSet<CostVariance> CostVariances { get; set; }
        public DbSet<VarianceThreshold> VarianceThresholds { get; set; }
        // ═════════════════════════════════════════════
        // SECURITY / AUTH DBSETS
        // ═════════════════════════════════════════════
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        // ─────────────────────────────────────────────
// Group 4 : Workforce & HR/Payroll (15 Tables)
// ─────────────────────────────────────────────
public DbSet<Department> Departments { get; set; }
public DbSet<Shift> Shifts { get; set; }
public DbSet<Employee> Employees { get; set; }
public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
public DbSet<LeaveType> LeaveTypes { get; set; }
public DbSet<LeaveBalance> LeaveBalances { get; set; }
public DbSet<LeaveRequest> LeaveRequests { get; set; }
public DbSet<AttendanceLog> AttendanceLogs { get; set; }
public DbSet<AttendanceCorrection> AttendanceCorrections { get; set; }
public DbSet<OvertimeRecord> OvertimeRecords { get; set; }
public DbSet<SalaryStructure> SalaryStructures { get; set; }
public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }
public DbSet<PayrollRun> PayrollRuns { get; set; }
public DbSet<PayrollLine> PayrollLines { get; set; }
public DbSet<PayrollApproval> PayrollApprovals { get; set; }

        // ─────────────────────────────────────────────
        // Group 5 : AI Engine (2 Tables)
        // ─────────────────────────────────────────────
        public DbSet<AIAssistantLog> AIAssistantLogs { get; set; }
        public DbSet<AIInsightCache> AIInsightCaches { get; set; }

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
            // ═════════════════════════════════════════════
            // MODULE 3.4 : COSTING & VARIANCE
            // ═════════════════════════════════════════════

            // 1. CostComponent
            modelBuilder.Entity<CostComponent>(b =>
            {
                b.ToTable("CostComponents");
                b.HasKey(x => x.ComponentID);
                b.Property(x => x.ComponentCode).HasMaxLength(20);
                b.Property(x => x.ComponentName).HasMaxLength(100);
                b.Property(x => x.ComponentType).HasMaxLength(20);
                b.Property(x => x.AllocationBasis).HasMaxLength(20);
                b.HasIndex(x => x.ComponentCode).IsUnique();
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 2. StandardItemCost
            modelBuilder.Entity<StandardItemCost>(b =>
            {
                b.ToTable("StandardItemCosts");
                b.HasKey(x => x.CostID);
                b.Property(x => x.StandardMaterialCost).HasPrecision(18, 4);
                b.Property(x => x.StandardLaborCost).HasPrecision(18, 4);
                b.Property(x => x.StandardOverheadCost).HasPrecision(18, 4);
                b.Property(x => x.TotalStandardCost)
                 .HasPrecision(18, 4)
                 .HasComputedColumnSql("[StandardMaterialCost] + [StandardLaborCost] + [StandardOverheadCost]", stored: true);
                b.Property(x => x.Status).HasMaxLength(20);
                b.Property(x => x.Notes).HasMaxLength(500);
                b.HasIndex(x => new { x.ProductID, x.EffectiveFrom }).IsUnique();
                b.HasIndex(x => x.ProductID)
                 .IsUnique()
                 .HasFilter("[Status] = 'Approved' AND [EffectiveTo] IS NULL")
                 .HasDatabaseName("UX_StandardItemCosts_OneApprovedPerProduct");
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.CostUOMID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ApprovedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 3. OverheadAllocation
            modelBuilder.Entity<OverheadAllocation>(b =>
            {
                b.ToTable("OverheadAllocations");
                b.HasKey(x => x.AllocationID);
                b.Property(x => x.AllocatedAmount).HasPrecision(18, 2);
                b.Property(x => x.BasisQty).HasPrecision(18, 4);
                b.Property(x => x.RatePerBasis)
                 .HasPrecision(18, 4)
                 .HasComputedColumnSql("CASE WHEN [BasisQty] = 0 THEN 0 ELSE [AllocatedAmount] / [BasisQty] END", stored: true);
                b.Property(x => x.Status).HasMaxLength(20);
                b.HasIndex(x => new { x.PlantID, x.ComponentID, x.PeriodMonth }).IsUnique();
                b.HasOne<Plant>().WithMany().HasForeignKey(x => x.PlantID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<CostComponent>().WithMany().HasForeignKey(x => x.ComponentID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.PostedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 4. BatchCostSummary
            modelBuilder.Entity<BatchCostSummary>(b =>
            {
                b.ToTable("BatchCostSummaries");
                b.HasKey(x => x.BatchCostID);
                b.Property(x => x.ActualMaterialCost).HasPrecision(18, 2);
                b.Property(x => x.ActualLaborCost).HasPrecision(18, 2);
                b.Property(x => x.ActualOverheadCost).HasPrecision(18, 2);
                b.Property(x => x.TotalBatchCost)
                 .HasPrecision(18, 2)
                 .HasComputedColumnSql("[ActualMaterialCost] + [ActualLaborCost] + [ActualOverheadCost]", stored: true);
                b.Property(x => x.GoodQty).HasPrecision(18, 4);
                b.Property(x => x.UnitCost)
                 .HasPrecision(18, 4)
                 .HasComputedColumnSql("CASE WHEN [GoodQty] = 0 THEN NULL ELSE ([ActualMaterialCost] + [ActualLaborCost] + [ActualOverheadCost]) / [GoodQty] END", stored: true);
                b.Property(x => x.StandardUnitCostSnapshot).HasPrecision(18, 4);
                b.Property(x => x.Status).HasMaxLength(20);
                b.HasIndex(x => x.ProductionOrderID).IsUnique();
                b.HasIndex(x => x.Status);
                b.HasOne<ProductionOrder>().WithMany().HasForeignKey(x => x.ProductionOrderID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.FinalizedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 5. BatchCostLine
            modelBuilder.Entity<BatchCostLine>(b =>
            {
                b.ToTable("BatchCostLines");
                b.HasKey(x => x.CostLineID);
                b.Property(x => x.SourceType).HasMaxLength(20);
                b.Property(x => x.Quantity).HasPrecision(18, 4);
                b.Property(x => x.Rate).HasPrecision(18, 4);
                b.Property(x => x.Amount)
                 .HasPrecision(18, 2)
                 .HasComputedColumnSql("ROUND([Quantity] * [Rate], 2)", stored: true);
                b.Property(x => x.Description).HasMaxLength(200);
                b.HasIndex(x => new { x.BatchCostID, x.ComponentID });
                b.HasOne(x => x.BatchCost)
                 .WithMany(s => s.Lines)
                 .HasForeignKey(x => x.BatchCostID)
                 .OnDelete(DeleteBehavior.Cascade);
                b.HasOne<CostComponent>().WithMany().HasForeignKey(x => x.ComponentID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductID).OnDelete(DeleteBehavior.Restrict);
            });

            // 6. CostVariance
            modelBuilder.Entity<CostVariance>(b =>
            {
                b.ToTable("CostVariances");
                b.HasKey(x => x.VarianceID);
                b.Property(x => x.MaterialPriceVariance).HasPrecision(18, 2);
                b.Property(x => x.MaterialUsageVariance).HasPrecision(18, 2);
                b.Property(x => x.LaborVariance).HasPrecision(18, 2);
                b.Property(x => x.OverheadVariance).HasPrecision(18, 2);
                b.Property(x => x.YieldVariance).HasPrecision(18, 2);
                b.Property(x => x.TotalVarianceAmount)
                 .HasPrecision(18, 2)
                 .HasComputedColumnSql("[MaterialPriceVariance] + [MaterialUsageVariance] + [LaborVariance] + [OverheadVariance] + [YieldVariance]", stored: true);
                b.Property(x => x.TotalVariancePercent).HasPrecision(9, 4);
                b.Property(x => x.Severity).HasMaxLength(20);
                b.Property(x => x.ReviewStatus).HasMaxLength(20);
                b.Property(x => x.ReviewComments).HasMaxLength(500);
                b.HasIndex(x => x.ProductionOrderID).IsUnique();
                b.HasIndex(x => new { x.ReviewStatus, x.Severity });
                b.HasOne<ProductionOrder>().WithMany().HasForeignKey(x => x.ProductionOrderID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ReviewedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 7. VarianceThreshold
            modelBuilder.Entity<VarianceThreshold>(b =>
            {
                b.ToTable("VarianceThresholds");
                b.HasKey(x => x.ThresholdID);
                b.Property(x => x.VarianceType).HasMaxLength(20);
                b.Property(x => x.WarningPercent).HasPrecision(5, 2);
                b.Property(x => x.CriticalPercent).HasPrecision(5, 2);
                b.HasIndex(x => new { x.PlantID, x.ProductID, x.VarianceType }).IsUnique();
                b.HasOne<Plant>().WithMany().HasForeignKey(x => x.PlantID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });
            // ═════════════════════════════════════════════
            // SECURITY / AUTH CONFIGURATION
            // ═════════════════════════════════════════════

            // 1. Role
            modelBuilder.Entity<Role>(b =>
            {
                b.ToTable("Roles");
                b.HasKey(x => x.RoleID);
                b.Property(x => x.RoleCode).HasMaxLength(30);
                b.Property(x => x.RoleName).HasMaxLength(100);
                b.Property(x => x.Description).HasMaxLength(300);

                b.HasIndex(x => x.RoleCode).IsUnique();

                // FIX: Audit FKs ke sath WithMany() specify kiya
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 2. Permission
            modelBuilder.Entity<Permission>(b =>
            {
                b.ToTable("Permissions");
                b.HasKey(x => x.PermissionID);
                b.Property(x => x.PermissionCode).HasMaxLength(50);
                b.Property(x => x.PermissionName).HasMaxLength(100);
                b.Property(x => x.ModuleKey).HasMaxLength(50);
                b.Property(x => x.Description).HasMaxLength(300);

                b.HasIndex(x => x.PermissionCode).IsUnique();
                b.HasIndex(x => x.ModuleKey);

                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 3. UserRole
            modelBuilder.Entity<UserRole>(b =>
            {
                b.ToTable("UserRoles");
                b.HasKey(x => x.UserRoleID);
                b.HasIndex(x => new { x.UserID, x.RoleID }).IsUnique();
                b.Property(x => x.AssignedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                // FIX: User navigation specify ki
                b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.Role).WithMany(r => r.UserRoles).HasForeignKey(x => x.RoleID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.AssignedBy).OnDelete(DeleteBehavior.Restrict);
            });

            // 4. RolePermission
            modelBuilder.Entity<RolePermission>(b =>
            {
                b.ToTable("RolePermissions");
                b.HasKey(x => x.RolePermissionID);
                b.HasIndex(x => new { x.RoleID, x.PermissionID }).IsUnique();
                b.Property(x => x.GrantedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasOne(x => x.Role).WithMany(r => r.RolePermissions).HasForeignKey(x => x.RoleID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.Permission).WithMany(p => p.RolePermissions).HasForeignKey(x => x.PermissionID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.GrantedBy).OnDelete(DeleteBehavior.Restrict);
            });
            // ═════════════════════════════════════════════
            // GROUP 4 : WORKFORCE & HR/PAYROLL
            // ═════════════════════════════════════════════

            // 1. Department
            modelBuilder.Entity<Department>(b =>
            {
                b.ToTable("Departments");
                b.HasKey(x => x.DepartmentID);
                b.Property(x => x.DepartmentCode).HasMaxLength(20);
                b.Property(x => x.DepartmentName).HasMaxLength(100);
                b.HasIndex(x => new { x.PlantID, x.DepartmentCode }).IsUnique();
                b.HasOne(x => x.Plant).WithMany().HasForeignKey(x => x.PlantID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 2. Shift
            modelBuilder.Entity<Shift>(b =>
            {
                b.ToTable("Shifts");
                b.HasKey(x => x.ShiftID);
                b.Property(x => x.ShiftCode).HasMaxLength(20);
                b.Property(x => x.ShiftName).HasMaxLength(50);
                b.HasIndex(x => new { x.PlantID, x.ShiftCode }).IsUnique();
                b.HasOne(x => x.Plant).WithMany().HasForeignKey(x => x.PlantID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 3. Employee
            modelBuilder.Entity<Employee>(b =>
            {
                b.ToTable("Employees");
                b.HasKey(x => x.EmployeeID);
                b.Property(x => x.EmployeeCode).HasMaxLength(30);
                b.Property(x => x.FirstName).HasMaxLength(50);
                b.Property(x => x.LastName).HasMaxLength(50);
                b.Property(x => x.CNIC).HasMaxLength(15);
                b.Property(x => x.Phone).HasMaxLength(20);
                b.Property(x => x.Email).HasMaxLength(100);
                b.Property(x => x.Designation).HasMaxLength(100);
                b.Property(x => x.EmploymentStatus).HasMaxLength(20);
                b.Property(x => x.BankAccountNo).HasMaxLength(30);
                b.Property(x => x.BankName).HasMaxLength(100);
                b.HasIndex(x => x.EmployeeCode).IsUnique();
                b.HasIndex(x => x.CNIC).IsUnique().HasFilter("[CNIC] IS NOT NULL");
                b.HasIndex(x => new { x.PlantID, x.DepartmentID });
                b.HasIndex(x => x.EmploymentStatus);
                b.HasOne(x => x.Plant).WithMany().HasForeignKey(x => x.PlantID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.Department).WithMany(d => d.Employees).HasForeignKey(x => x.DepartmentID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.Shift).WithMany(s => s.Employees).HasForeignKey(x => x.ShiftID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 4. EmployeeDocument
            modelBuilder.Entity<EmployeeDocument>(b =>
            {
                b.ToTable("EmployeeDocuments");
                b.HasKey(x => x.DocumentID);
                b.Property(x => x.DocumentType).HasMaxLength(30);
                b.Property(x => x.DocumentName).HasMaxLength(100);
                b.Property(x => x.FilePath).HasMaxLength(500);
                b.HasIndex(x => x.EmployeeID);
                b.HasOne(x => x.Employee).WithMany(e => e.Documents).HasForeignKey(x => x.EmployeeID).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.UploadedByUser).WithMany().HasForeignKey(x => x.UploadedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 5. LeaveType
            modelBuilder.Entity<LeaveType>(b =>
            {
                b.ToTable("LeaveTypes");
                b.HasKey(x => x.LeaveTypeID);
                b.Property(x => x.LeaveTypeCode).HasMaxLength(20);
                b.Property(x => x.LeaveTypeName).HasMaxLength(50);
                b.HasIndex(x => x.LeaveTypeCode).IsUnique();
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 6. LeaveBalance
            modelBuilder.Entity<LeaveBalance>(b =>
            {
                b.ToTable("LeaveBalances");
                b.HasKey(x => x.BalanceID);
                b.Property(x => x.EntitledDays).HasPrecision(5, 1);
                b.Property(x => x.UsedDays).HasPrecision(5, 1);
                b.Property(x => x.RemainingDays).HasPrecision(5, 1);
                b.HasIndex(x => new { x.EmployeeID, x.LeaveTypeID, x.Year }).IsUnique();
                b.HasOne(x => x.Employee).WithMany(e => e.LeaveBalances).HasForeignKey(x => x.EmployeeID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.LeaveType).WithMany(t => t.LeaveBalances).HasForeignKey(x => x.LeaveTypeID).OnDelete(DeleteBehavior.Restrict);
            });

            // 7. LeaveRequest
            modelBuilder.Entity<LeaveRequest>(b =>
            {
                b.ToTable("LeaveRequests");
                b.HasKey(x => x.LeaveRequestID);
                b.Property(x => x.RequestNo).HasMaxLength(30);
                b.Property(x => x.TotalDays).HasPrecision(5, 1);
                b.Property(x => x.Reason).HasMaxLength(500);
                b.Property(x => x.Status).HasMaxLength(20);
                b.Property(x => x.RejectionReason).HasMaxLength(300);
                b.HasIndex(x => x.RequestNo).IsUnique();
                b.HasIndex(x => new { x.EmployeeID, x.Status });
                b.HasIndex(x => x.Status);
                b.HasOne(x => x.Employee).WithMany(e => e.LeaveRequests).HasForeignKey(x => x.EmployeeID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.LeaveType).WithMany(t => t.LeaveRequests).HasForeignKey(x => x.LeaveTypeID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.ApprovedByUser).WithMany().HasForeignKey(x => x.ApprovedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 8. AttendanceLog
            modelBuilder.Entity<AttendanceLog>(b =>
            {
                b.ToTable("AttendanceLogs");
                b.HasKey(x => x.AttendanceID);
                b.Property(x => x.Status).HasMaxLength(20);
                b.Property(x => x.Remarks).HasMaxLength(200);
                b.HasIndex(x => new { x.EmployeeID, x.LogDate }).IsUnique();
                b.HasIndex(x => new { x.LogDate, x.Status });
                b.HasOne(x => x.Employee).WithMany(e => e.AttendanceLogs).HasForeignKey(x => x.EmployeeID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.Shift).WithMany(s => s.AttendanceLogs).HasForeignKey(x => x.ShiftID).OnDelete(DeleteBehavior.Restrict);
            });

            // 9. AttendanceCorrection
            modelBuilder.Entity<AttendanceCorrection>(b =>
            {
                b.ToTable("AttendanceCorrections");
                b.HasKey(x => x.CorrectionID);
                b.Property(x => x.Reason).HasMaxLength(300);
                b.Property(x => x.Status).HasMaxLength(20);
                b.HasIndex(x => x.Status);
                b.HasOne(x => x.AttendanceLog).WithMany(a => a.Corrections).HasForeignKey(x => x.AttendanceID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.RequestedByUser).WithMany().HasForeignKey(x => x.RequestedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.ApprovedByUser).WithMany().HasForeignKey(x => x.ApprovedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 10. OvertimeRecord
            modelBuilder.Entity<OvertimeRecord>(b =>
            {
                b.ToTable("OvertimeRecords");
                b.HasKey(x => x.OvertimeID);
                b.Property(x => x.OTHours).HasPrecision(5, 2);
                b.Property(x => x.OTRateMultiplier).HasPrecision(3, 2);
                b.Property(x => x.Reason).HasMaxLength(300);
                b.Property(x => x.Status).HasMaxLength(20);
                b.HasIndex(x => new { x.EmployeeID, x.OTDate });
                b.HasIndex(x => x.Status);
                b.HasOne(x => x.Employee).WithMany(e => e.OvertimeRecords).HasForeignKey(x => x.EmployeeID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.ApprovedByUser).WithMany().HasForeignKey(x => x.ApprovedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 11. SalaryStructure
            modelBuilder.Entity<SalaryStructure>(b =>
            {
                b.ToTable("SalaryStructures");
                b.HasKey(x => x.StructureID);
                b.Property(x => x.StructureCode).HasMaxLength(30);
                b.Property(x => x.StructureName).HasMaxLength(100);
                b.Property(x => x.BasicSalary).HasPrecision(18, 2);
                b.Property(x => x.HousingAllowance).HasPrecision(18, 2);
                b.Property(x => x.TransportAllowance).HasPrecision(18, 2);
                b.Property(x => x.OtherAllowance).HasPrecision(18, 2);
                b.HasIndex(x => x.StructureCode).IsUnique();
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 12. EmployeeSalary
            modelBuilder.Entity<EmployeeSalary>(b =>
            {
                b.ToTable("EmployeeSalaries");
                b.HasKey(x => x.EmployeeSalaryID);
                b.Property(x => x.BasicSalary).HasPrecision(18, 2);
                b.Property(x => x.HousingAllowance).HasPrecision(18, 2);
                b.Property(x => x.TransportAllowance).HasPrecision(18, 2);
                b.Property(x => x.OtherAllowance).HasPrecision(18, 2);
                b.Property(x => x.GrossSalary).HasPrecision(18, 2);
                b.Property(x => x.Status).HasMaxLength(20);
                b.HasIndex(x => new { x.EmployeeID, x.EffectiveFrom }).IsUnique();
                b.HasIndex(x => x.EmployeeID)
                 .IsUnique()
                 .HasFilter("[Status] = 'Active' AND [EffectiveTo] IS NULL")
                 .HasDatabaseName("UX_EmployeeSalaries_OneActivePerEmployee");
                b.HasOne(x => x.Employee).WithMany(e => e.Salaries).HasForeignKey(x => x.EmployeeID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.Structure).WithMany(s => s.EmployeeSalaries).HasForeignKey(x => x.StructureID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 13. PayrollRun
            modelBuilder.Entity<PayrollRun>(b =>
            {
                b.ToTable("PayrollRuns");
                b.HasKey(x => x.PayrollRunID);
                b.Property(x => x.RunNo).HasMaxLength(30);
                b.Property(x => x.Status).HasMaxLength(20);
                b.Property(x => x.TotalBasicPay).HasPrecision(18, 2);
                b.Property(x => x.TotalAllowances).HasPrecision(18, 2);
                b.Property(x => x.TotalOvertimePay).HasPrecision(18, 2);
                b.Property(x => x.TotalDeductions).HasPrecision(18, 2);
                b.Property(x => x.NetPayable).HasPrecision(18, 2);
                b.HasIndex(x => x.RunNo).IsUnique();
                b.HasIndex(x => new { x.PlantID, x.PeriodMonth, x.PeriodYear }).IsUnique();
                b.HasIndex(x => x.Status);
                b.HasOne(x => x.Plant).WithMany().HasForeignKey(x => x.PlantID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserID).OnDelete(DeleteBehavior.Restrict);
            });

            // 14. PayrollLine
            modelBuilder.Entity<PayrollLine>(b =>
            {
                b.ToTable("PayrollLines");
                b.HasKey(x => x.PayrollLineID);
                b.Property(x => x.BasicPay).HasPrecision(18, 2);
                b.Property(x => x.HousingAllowance).HasPrecision(18, 2);
                b.Property(x => x.TransportAllowance).HasPrecision(18, 2);
                b.Property(x => x.OtherAllowance).HasPrecision(18, 2);
                b.Property(x => x.OvertimePay).HasPrecision(18, 2);
                b.Property(x => x.LeaveDeduction).HasPrecision(18, 2);
                b.Property(x => x.OtherDeduction).HasPrecision(18, 2);
                b.Property(x => x.GrossPay).HasPrecision(18, 2);
                b.Property(x => x.TotalDeductions).HasPrecision(18, 2);
                b.Property(x => x.NetPay).HasPrecision(18, 2);
                b.Property(x => x.OTHours).HasPrecision(5, 2);
                b.Property(x => x.Remarks).HasMaxLength(200);
                b.HasIndex(x => new { x.PayrollRunID, x.EmployeeID }).IsUnique();
                b.HasOne(x => x.PayrollRun).WithMany(r => r.Lines).HasForeignKey(x => x.PayrollRunID).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Employee).WithMany(e => e.PayrollLines).HasForeignKey(x => x.EmployeeID).OnDelete(DeleteBehavior.Restrict);
            });

            // 15. PayrollApproval
            modelBuilder.Entity<PayrollApproval>(b =>
            {
                b.ToTable("PayrollApprovals");
                b.HasKey(x => x.ApprovalID);
                b.Property(x => x.Decision).HasMaxLength(20);
                b.Property(x => x.Comments).HasMaxLength(500);
                b.HasIndex(x => new { x.PayrollRunID, x.StepNo, x.ApproverUserID }).IsUnique();
                b.HasIndex(x => x.PayrollRunID);
                b.HasOne(x => x.PayrollRun).WithMany(r => r.Approvals).HasForeignKey(x => x.PayrollRunID).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.ApproverUser).WithMany().HasForeignKey(x => x.ApproverUserID).OnDelete(DeleteBehavior.Restrict);
            });
            // ═════════════════════════════════════════════
            // GROUP 5 : AI ENGINE
            // ═════════════════════════════════════════════

            // 1. AIAssistantLog
            modelBuilder.Entity<AIAssistantLog>(b =>
            {
                b.ToTable("AIAssistantLogs");
                b.HasKey(x => x.AILogID);

                b.Property(x => x.DomainContext).HasMaxLength(30);
                b.Property(x => x.QuestionSummary).HasMaxLength(500);
                b.Property(x => x.AnswerSummary).HasMaxLength(1000);
                b.Property(x => x.ModelName).HasMaxLength(50);
                b.Property(x => x.ErrorMessage).HasMaxLength(500);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasIndex(x => new { x.UserID, x.CreatedAt });
                b.HasIndex(x => new { x.DomainContext, x.CreatedAt });
                b.HasIndex(x => x.CreatedAt);

                b.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // 2. AIInsightCache
            modelBuilder.Entity<AIInsightCache>(b =>
            {
                b.ToTable("AIInsightCaches");
                b.HasKey(x => x.CacheID);

                b.Property(x => x.ModuleContext).HasMaxLength(50);
                b.Property(x => x.CacheKey).HasMaxLength(100);
                b.Property(x => x.PayloadJson).HasColumnType("nvarchar(max)");
                b.Property(x => x.GeneratedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasIndex(x => new { x.ModuleContext, x.CacheKey }).IsUnique();
                b.HasIndex(x => x.ExpiresAt);
            });
        }
    }
}