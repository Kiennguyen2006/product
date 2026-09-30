using Microsoft.EntityFrameworkCore;
using NetCoreLAB6_EF.Models;

namespace NetCoreLAB6_EF.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<NkkProduct> Products { get; set; }
        public DbSet<NkkCategory> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Bắt buộc EF Core map đúng tên bảng số ít dbo.Product và dbo.Category
            modelBuilder.Entity<NkkProduct>().ToTable("Product");
            modelBuilder.Entity<NkkCategory>().ToTable("Category");
        }
    }
}