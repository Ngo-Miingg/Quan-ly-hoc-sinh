using Microsoft.EntityFrameworkCore;
namespace Student_Management.Models;

public partial class QuanLyHocSinhContext : DbContext
{
    // --- Constructor ---
    public QuanLyHocSinhContext(DbContextOptions<QuanLyHocSinhContext> options)
        : base(options)
    {
    }

    // --- DbSets ---
    public virtual DbSet<Diem> DiemSos { get; set; }
    public virtual DbSet<GiaoVien> GiaoViens { get; set; }
    public virtual DbSet<HocKy> HocKys { get; set; }
    public virtual DbSet<HocPhi> HocPhis { get; set; }
    public virtual DbSet<HocSinh> HocSinhs { get; set; }
    public virtual DbSet<LichHoc> LichHocs { get; set; }
    public virtual DbSet<Lop> LopHocs { get; set; }
    public virtual DbSet<MonHoc> MonHocs { get; set; }
    public virtual DbSet<NamHoc> NamHocs { get; set; }
    public virtual DbSet<PhanCongGiangDay> PhanCongGiangDays { get; set; }
    public virtual DbSet<PhongHoc> PhongHocs { get; set; }
    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ... (Toàn bộ phần cấu hình Fluent API của bạn được giữ nguyên ở đây)

        modelBuilder.Entity<Diem>(entity =>
        {
            entity.HasKey(e => e.MaDiemSo).HasName("PK_DiemSo");
            entity.ToTable("DIEM");

            entity.Property(e => e.MaDiemSo).HasColumnName("MaDiem");
            entity.Property(e => e.MaHocSinh).HasColumnName("MaHS");
            entity.Property(e => e.MaHocKy).HasColumnName("MaHK");
            entity.Property(e => e.DiemTrungBinh).HasColumnName("DiemTB");

            entity.HasOne(d => d.HocSinh).WithMany(p => p.DiemSos)
                .HasForeignKey(d => d.MaHocSinh)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_DiemSo_HocSinh");

            entity.HasOne(d => d.MonHoc).WithMany(p => p.DiemSos)
                .HasForeignKey(d => d.MaMonHoc)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_DiemSo_MonHoc");

            entity.HasOne(d => d.HocKy).WithMany(p => p.DiemSos)
                .HasForeignKey(d => d.MaHocKy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_DiemSo_HocKy");
        });

        modelBuilder.Entity<GiaoVien>(entity =>
        {
            entity.HasKey(e => e.MaGiaoVien).HasName("PK_GiaoVien");
            entity.ToTable("GIAOVIEN");
            entity.Property(e => e.MaGiaoVien).HasColumnName("MaGV");
            entity.Property(e => e.SoDienThoai).HasColumnName("SDT");
        });

        modelBuilder.Entity<HocKy>(entity =>
        {
            entity.HasKey(e => e.MaHocKy).HasName("PK_HocKy");
            entity.ToTable("HOCKY");
            entity.Property(e => e.MaHocKy).HasColumnName("MaHK");
            entity.Property(e => e.TenHocKy).HasColumnName("TenHK");

            entity.HasOne(d => d.NamHoc).WithMany(p => p.HocKys)
                .HasForeignKey(d => d.MaNamHoc)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_HocKy_NamHoc");
        });

        modelBuilder.Entity<HocPhi>(entity =>
        {
            entity.HasKey(e => e.MaHocPhi).HasName("PK_HocPhi");
            entity.ToTable("HOCPHI");
            entity.Property(e => e.MaHocPhi).HasColumnName("MaHP");
            entity.Property(e => e.MaHocSinh).HasColumnName("MaHS");
            entity.Property(e => e.MaHocKy).HasColumnName("MaHK");

            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.HocSinh).WithMany(p => p.HocPhis)
                .HasForeignKey(d => d.MaHocSinh)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_HocPhi_HocSinh");

            entity.HasOne(d => d.HocKy).WithMany(p => p.HocPhis)
                .HasForeignKey(d => d.MaHocKy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_HocPhi_HocKy");
        });

        modelBuilder.Entity<HocSinh>(entity =>
        {
            entity.HasKey(e => e.MaHocSinh).HasName("PK_HocSinh");
            entity.ToTable("HOCSINH");
            entity.Property(e => e.MaHocSinh).HasColumnName("MaHS");
            entity.Property(e => e.MaLopHoc).HasColumnName("MaLop");
            entity.Property(e => e.SoDienThoai).HasColumnName("SDT");

            entity.HasOne(d => d.LopHoc).WithMany(p => p.HocSinhs)
                .HasForeignKey(d => d.MaLopHoc)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_HocSinh_LopHoc");
        });

        modelBuilder.Entity<LichHoc>(entity =>
        {
            entity.HasKey(e => e.MaLichHoc).HasName("PK_LichHoc");
            entity.ToTable("LICHHOC");
            entity.Property(e => e.MaGiaoVien).HasColumnName("MaGV");
            entity.Property(e => e.MaHocKy).HasColumnName("MaHK");
            entity.Property(e => e.MaLopHoc).HasColumnName("MaLop");
            entity.Property(e => e.MaPhongHoc).HasColumnName("MaPhong");

            entity.HasOne(d => d.GiaoVien).WithMany(p => p.LichHocs).HasForeignKey(d => d.MaGiaoVien).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_LichHoc_GiaoVien");
            entity.HasOne(d => d.HocKy).WithMany(p => p.LichHocs).HasForeignKey(d => d.MaHocKy).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_LichHoc_HocKy");
            entity.HasOne(d => d.LopHoc).WithMany(p => p.LichHocs).HasForeignKey(d => d.MaLopHoc).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_LichHoc_LopHoc");
            entity.HasOne(d => d.MonHoc).WithMany(p => p.LichHocs).HasForeignKey(d => d.MaMonHoc).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_LichHoc_MonHoc");
            entity.HasOne(d => d.PhongHoc).WithMany(p => p.LichHocs).HasForeignKey(d => d.MaPhongHoc).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_LichHoc_PhongHoc");
        });

        modelBuilder.Entity<Lop>(entity =>
        {
            entity.HasKey(e => e.MaLopHoc).HasName("PK_LopHoc");
            entity.ToTable("LOP");
            entity.Property(e => e.MaLopHoc).HasColumnName("MaLop");
            entity.Property(e => e.TenLopHoc).HasColumnName("TenLop");
            entity.Property(e => e.MaGiaoVienChuNhiem).HasColumnName("MaGVCN");

            entity.HasOne(d => d.GiaoVienChuNhiem).WithMany(p => p.LopHocs)
                .HasForeignKey(d => d.MaGiaoVienChuNhiem)
                .HasConstraintName("FK_LopHoc_GiaoVien");

            entity.HasOne(d => d.NamHoc).WithMany(p => p.LopHocs)
                .HasForeignKey(d => d.MaNamHoc)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_LopHoc_NamHoc");
        });

        modelBuilder.Entity<MonHoc>(entity =>
        {
            entity.HasKey(e => e.MaMonHoc).HasName("PK_MonHoc");
            entity.ToTable("MONHOC");
        });

        modelBuilder.Entity<NamHoc>(entity =>
        {
            entity.HasKey(e => e.MaNamHoc).HasName("PK_NamHoc");
            entity.ToTable("NAMHOC");
        });

        modelBuilder.Entity<PhanCongGiangDay>(entity =>
        {
            entity.HasKey(e => e.MaPhanCong).HasName("PK_PhanCongGiangDay");
            entity.ToTable("PHANCONG_GIANGDAY");
            entity.Property(e => e.MaPhanCong).HasColumnName("MaPC");
            entity.Property(e => e.MaGiaoVien).HasColumnName("MaGV");
            entity.Property(e => e.MaHocKy).HasColumnName("MaHK");
            entity.Property(e => e.MaLopHoc).HasColumnName("MaLop");

            entity.HasOne(d => d.GiaoVien).WithMany(p => p.PhanCongGiangDays).HasForeignKey(d => d.MaGiaoVien).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_PhanCong_GiaoVien");
            entity.HasOne(d => d.HocKy).WithMany(p => p.PhanCongGiangDays).HasForeignKey(d => d.MaHocKy).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_PhanCong_HocKy");
            entity.HasOne(d => d.LopHoc).WithMany(p => p.PhanCongGiangDays).HasForeignKey(d => d.MaLopHoc).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_PhanCong_LopHoc");
            entity.HasOne(d => d.MonHoc).WithMany(p => p.PhanCongGiangDays).HasForeignKey(d => d.MaMonHoc).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_PhanCong_MonHoc");
        });

        modelBuilder.Entity<PhongHoc>(entity =>
        {
            entity.HasKey(e => e.MaPhongHoc).HasName("PK_PhongHoc");
            entity.ToTable("PHONGHOC");
            entity.Property(e => e.MaPhongHoc).HasColumnName("MaPhong");
            entity.Property(e => e.TenPhongHoc).HasColumnName("TenPhong");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK_TaiKhoan");
            entity.ToTable("TAIKHOAN");
            entity.HasIndex(e => e.TenDangNhap).IsUnique();

            entity.Property(e => e.MaTaiKhoan).HasColumnName("MaTK");
            entity.Property(e => e.MaGiaoVien).HasColumnName("MaGV");
            entity.Property(e => e.MaHocSinh).HasColumnName("MaHS");

            entity.HasOne(d => d.GiaoVien).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaGiaoVien)
                .HasConstraintName("FK_TaiKhoan_GiaoVien");

            entity.HasOne(d => d.HocSinh).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaHocSinh)
                .HasConstraintName("FK_TaiKhoan_HocSinh");
        });

        // THAY ĐỔI: Mã hóa mật khẩu an toàn khi seed data
        modelBuilder.Entity<TaiKhoan>().HasData(
                new TaiKhoan
                {
                    MaTaiKhoan = 1,
                    TenDangNhap = "admin",
                    // Mật khẩu mặc định là "admin@123", nhưng được hash an toàn
                    MatKhau = BCrypt.Net.BCrypt.HashPassword("admin@123"),
                    VaiTro = "Admin"
                }
            );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}