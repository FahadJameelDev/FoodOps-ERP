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
        }
    }
}