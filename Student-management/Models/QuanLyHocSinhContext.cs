using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Student_management.Models;

public partial class QuanLyHocSinhContext : DbContext
{
    public QuanLyHocSinhContext()
    {
    }

    public QuanLyHocSinhContext(DbContextOptions<QuanLyHocSinhContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Diem> Diems { get; set; }

    public virtual DbSet<GiaoVien> GiaoViens { get; set; }

    public virtual DbSet<HocKy> HocKies { get; set; }

    public virtual DbSet<HocPhi> HocPhis { get; set; }

    public virtual DbSet<HocSinh> HocSinhs { get; set; }

    public virtual DbSet<LoaiDiem> LoaiDiems { get; set; }

    public virtual DbSet<Lop> Lops { get; set; }

    public virtual DbSet<MonHoc> MonHocs { get; set; }

    public virtual DbSet<NamHoc> NamHocs { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<PhanCongGiangDay> PhanCongGiangDays { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Diem>(entity =>
        {
            entity.HasKey(e => e.MaDiem).HasName("PK__Diem__333260251C997AC0");

            entity.ToTable("Diem");

            entity.Property(e => e.DiemSo).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.LanThi).HasDefaultValue(1);
            entity.Property(e => e.MaHs).HasColumnName("MaHS");

            entity.HasOne(d => d.MaHocKyNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaHocKy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Diem_HocKy");

            entity.HasOne(d => d.MaHsNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaHs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Diem_HocSinh");

            entity.HasOne(d => d.MaLoaiDiemNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaLoaiDiem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Diem_LoaiDiem");

            entity.HasOne(d => d.MaMonHocNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaMonHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Diem_MonHoc");
        });

        modelBuilder.Entity<GiaoVien>(entity =>
        {
            entity.HasKey(e => e.MaGv).HasName("PK__GiaoVien__2725AEF345C5DE21");

            entity.ToTable("GiaoVien");

            entity.HasIndex(e => e.MaNguoiDung, "UQ__GiaoVien__C539D7638EE101D2").IsUnique();

            entity.Property(e => e.MaGv).HasColumnName("MaGV");
            entity.Property(e => e.TrinhDoChuyenMon).HasMaxLength(100);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithOne(p => p.GiaoVien)
                .HasForeignKey<GiaoVien>(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GiaoVien_NguoiDung");
        });

        modelBuilder.Entity<HocKy>(entity =>
        {
            entity.HasKey(e => e.MaHocKy).HasName("PK__HocKy__1EB551101E79378D");

            entity.ToTable("HocKy");

            entity.Property(e => e.HeSo).HasDefaultValue(1);
            entity.Property(e => e.TenHocKy).HasMaxLength(50);

            entity.HasOne(d => d.MaNamHocNavigation).WithMany(p => p.HocKies)
                .HasForeignKey(d => d.MaNamHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HocKy_NamHoc");
        });

        modelBuilder.Entity<HocPhi>(entity =>
        {
            entity.HasKey(e => e.MaHocPhi).HasName("PK__HocPhi__929232A2444F48B9");

            entity.ToTable("HocPhi");

            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.MaHs).HasColumnName("MaHS");
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaHocKyNavigation).WithMany(p => p.HocPhis)
                .HasForeignKey(d => d.MaHocKy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HocPhi_HocKy");

            entity.HasOne(d => d.MaHsNavigation).WithMany(p => p.HocPhis)
                .HasForeignKey(d => d.MaHs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HocPhi_HocSinh");
        });

        modelBuilder.Entity<HocSinh>(entity =>
        {
            entity.HasKey(e => e.MaHs).HasName("PK__HocSinh__2725A6EF24C28167");

            entity.ToTable("HocSinh");

            entity.HasIndex(e => e.MaNguoiDung, "UQ__HocSinh__C539D7639329342C").IsUnique();

            entity.Property(e => e.MaHs).HasColumnName("MaHS");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Đang học");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.HocSinhs)
                .HasForeignKey(d => d.MaLop)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HocSinh_Lop");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithOne(p => p.HocSinh)
                .HasForeignKey<HocSinh>(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HocSinh_NguoiDung");
        });

        modelBuilder.Entity<LoaiDiem>(entity =>
        {
            entity.HasKey(e => e.MaLoaiDiem).HasName("PK__LoaiDiem__77BE9E4A79CEF4AD");

            entity.ToTable("LoaiDiem");

            entity.Property(e => e.TenLoaiDiem).HasMaxLength(50);
        });

        modelBuilder.Entity<Lop>(entity =>
        {
            entity.HasKey(e => e.MaLop).HasName("PK__Lop__3B98D2730EC949E3");

            entity.ToTable("Lop");

            entity.Property(e => e.MaGvcn).HasColumnName("MaGVCN");
            entity.Property(e => e.TenLop).HasMaxLength(50);

            entity.HasOne(d => d.MaGvcnNavigation).WithMany(p => p.Lops)
                .HasForeignKey(d => d.MaGvcn)
                .HasConstraintName("FK_Lop_GiaoVien");

            entity.HasOne(d => d.MaNamHocNavigation).WithMany(p => p.Lops)
                .HasForeignKey(d => d.MaNamHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lop_NamHoc");
        });

        modelBuilder.Entity<MonHoc>(entity =>
        {
            entity.HasKey(e => e.MaMonHoc).HasName("PK__MonHoc__4127737FDAF05CF5");

            entity.ToTable("MonHoc");

            entity.HasIndex(e => e.TenMonHoc, "UQ__MonHoc__AB9BBBD6EEA3A208").IsUnique();

            entity.Property(e => e.TenMonHoc).HasMaxLength(100);
        });

        modelBuilder.Entity<NamHoc>(entity =>
        {
            entity.HasKey(e => e.MaNamHoc).HasName("PK__NamHoc__7DBADD740F0503A3");

            entity.ToTable("NamHoc");

            entity.Property(e => e.TenNamHoc).HasMaxLength(50);
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D7623FF69CDB");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.Email, "UQ__NguoiDun__A9D10534697CDFEA").IsUnique();

            entity.Property(e => e.AnhDaiDien).HasMaxLength(255);
            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("SDT");
        });

        modelBuilder.Entity<PhanCongGiangDay>(entity =>
        {
            entity.HasKey(e => e.MaPhanCong).HasName("PK__PhanCong__C279D916CD8B2C59");

            entity.ToTable("PhanCongGiangDay");

            entity.HasIndex(e => new { e.MaGv, e.MaMonHoc, e.MaLop, e.MaHocKy }, "UQ_PhanCong").IsUnique();

            entity.Property(e => e.MaGv).HasColumnName("MaGV");

            entity.HasOne(d => d.MaGvNavigation).WithMany(p => p.PhanCongGiangDays)
                .HasForeignKey(d => d.MaGv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhanCong_GiaoVien");

            entity.HasOne(d => d.MaHocKyNavigation).WithMany(p => p.PhanCongGiangDays)
                .HasForeignKey(d => d.MaHocKy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhanCong_HocKy");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.PhanCongGiangDays)
                .HasForeignKey(d => d.MaLop)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhanCong_Lop");

            entity.HasOne(d => d.MaMonHocNavigation).WithMany(p => p.PhanCongGiangDays)
                .HasForeignKey(d => d.MaMonHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhanCong_MonHoc");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__TaiKhoan__AD7C6529D5719975");

            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.TenDangNhap, "UQ__TaiKhoan__55F68FC0ACD64ED9").IsUnique();

            entity.HasIndex(e => e.MaNguoiDung, "UQ__TaiKhoan__C539D763E0F16677").IsUnique();

            entity.Property(e => e.MatKhauHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithOne(p => p.TaiKhoan)
                .HasForeignKey<TaiKhoan>(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaiKhoan_NguoiDung");

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaVaiTro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaiKhoan_VaiTro");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro).HasName("PK__VaiTro__C24C41CF80424A01");

            entity.ToTable("VaiTro");

            entity.HasIndex(e => e.TenVaiTro, "UQ__VaiTro__1DA55814C9496B45").IsUnique();

            entity.Property(e => e.TenVaiTro).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
