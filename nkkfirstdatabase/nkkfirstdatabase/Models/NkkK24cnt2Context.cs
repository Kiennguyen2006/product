using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace nkkfirstdatabase.Models;

public partial class NkkK24cnt2Context : DbContext
{
    public NkkK24cnt2Context()
    {
    }

    public NkkK24cnt2Context(DbContextOptions<NkkK24cnt2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Nkkember> Nkkembers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ADMIN-PC\\SQLEXPRESS;Database=nkkK24CNT2;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Nkkember>(entity =>
        {
            entity.HasKey(e => e.NkkMemberId).HasName("PK__Nkkember__4B18CDBFBBFBDA3D");

            entity.ToTable("Nkkember");

            entity.Property(e => e.Nkkemail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nkkemail");
            entity.Property(e => e.Nkkfullname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nkkfullname");
            entity.Property(e => e.Nkkpassword)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nkkpassword");
            entity.Property(e => e.Nkkphone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nkkphone");
            entity.Property(e => e.Nkkuser)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nkkuser");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
