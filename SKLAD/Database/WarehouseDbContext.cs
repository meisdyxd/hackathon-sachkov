using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using SKLAD.Database.SeedData;
using SKLAD.Entities;

namespace SKLAD.Database
{
    // ЭТО УЖАС, ЧТО ТО СДЕЛАЛ НАДЕЮСЬ РАБОТАЕТ. СИД ДАТА ДОБАВИТЬ ТРУДНО, ОН РУГАЕТСЯ НА Guid.NewGuid() я хз че с этим делать.
    // в сид дате супер старый данные, там прям зародыш был
    public class WarehouseDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Warehouse> Warehouses => Set<Warehouse>();
        public DbSet<StorageZone> StorageZones => Set<StorageZone>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<ProductMovement> ProductMovements => Set<ProductMovement>();
        public DbSet<AuditQuest> AuditQuests => Set<AuditQuest>();
        public DbSet<PickingTaskEntity> PickingTasks => Set<PickingTaskEntity>();

        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options)
        : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StorageZone>()
                .HasOne(sz => sz.Warehouse)
                .WithMany(w => w.Zones)
                .HasForeignKey(sz => sz.WarehouseId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.StorageZone)
                .WithMany()
                .HasForeignKey(p => p.StorageZoneId);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Supplier)
                .WithMany()
                .HasForeignKey(po => po.SupplierId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.PurchaseOrder)
                .WithMany(po => po.Items)
                .HasForeignKey(oi => oi.PurchaseOrderId);

            modelBuilder.Entity<ProductMovement>()
                .HasOne(pm => pm.Product)
                .WithMany()
                .HasForeignKey(pm => pm.ProductId);

            //modelBuilder.Entity<Product>().HasData(
            //    GetData.GetSeedProducts()
            //);

            //modelBuilder.Entity<ProductMovement>().HasData(
            //    GetSeedMovements()
            //);
        }
    }
}
