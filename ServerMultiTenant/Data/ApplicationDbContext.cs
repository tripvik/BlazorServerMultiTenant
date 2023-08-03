using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ServerMultiTenant.Data
{
    public class ApplicationDbContext : MultiTenantIdentityDbContext
    {
        public ApplicationDbContext(ITenantInfo tenantInfo) : base(tenantInfo)
        {
        }

        public ApplicationDbContext(ITenantInfo tenantInfo, DbContextOptions options) : base(tenantInfo, options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(TenantInfo.ConnectionString ?? throw new InvalidOperationException());
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoItem>().IsMultiTenant();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}