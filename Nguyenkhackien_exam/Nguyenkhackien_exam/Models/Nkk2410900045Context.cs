using Microsoft.EntityFrameworkCore;

namespace Nguyenkhackien_exam.Models;

public partial class Nkk2410900045Context : DbContext
{
    public Nkk2410900045Context()
    {
    }

    public Nkk2410900045Context(DbContextOptions<Nkk2410900045Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Nkkemployee> Nkkemployees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Nkkemployee>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__NKKEmplo__3214EC0762A6AF07");

            entity.ToTable("NKKEmployee");

            entity.Property(e => e.Nkkactive)
                .HasColumnName("NKKActive");

            entity.Property(e => e.NkkbirthDay)
                .HasColumnName("NKKBirthDay");

            entity.Property(e => e.Nkkemail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NKKEmail");

            entity.Property(e => e.Nkkgender)
                .HasMaxLength(10)
                .HasColumnName("NKKGender");

            entity.Property(e => e.Nkkname)
                .HasMaxLength(100)
                .HasColumnName("NKKName");

            entity.Property(e => e.Nkkphone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("NKKPhone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}