using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Student_Management.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MONHOC",
                columns: table => new
                {
                    MaMonHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenMonHoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoTiet = table.Column<int>(type: "int", nullable: true),
                    HeSo = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonHoc", x => x.MaMonHoc);
                });

            migrationBuilder.CreateTable(
                name: "NAMHOC",
                columns: table => new
                {
                    MaNamHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNamHoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NamHoc", x => x.MaNamHoc);
                });

            migrationBuilder.CreateTable(
                name: "PHONGHOC",
                columns: table => new
                {
                    MaPhong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SucChua = table.Column<int>(type: "int", nullable: true),
                    ViTri = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhongHoc", x => x.MaPhong);
                });

            migrationBuilder.CreateTable(
                name: "GIAOVIEN",
                columns: table => new
                {
                    MaGV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaMonHoc = table.Column<int>(type: "int", nullable: true),
                    MonHocMaMonHoc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiaoVien", x => x.MaGV);
                    table.ForeignKey(
                        name: "FK_GIAOVIEN_MONHOC_MonHocMaMonHoc",
                        column: x => x.MonHocMaMonHoc,
                        principalTable: "MONHOC",
                        principalColumn: "MaMonHoc");
                });

            migrationBuilder.CreateTable(
                name: "HOCKY",
                columns: table => new
                {
                    MaHK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenHK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaNamHoc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocKy", x => x.MaHK);
                    table.ForeignKey(
                        name: "FK_HocKy_NamHoc",
                        column: x => x.MaNamHoc,
                        principalTable: "NAMHOC",
                        principalColumn: "MaNamHoc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LOP",
                columns: table => new
                {
                    MaLop = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLop = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiSo = table.Column<int>(type: "int", nullable: true),
                    MaNamHoc = table.Column<int>(type: "int", nullable: false),
                    MaGVCN = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LopHoc", x => x.MaLop);
                    table.ForeignKey(
                        name: "FK_LopHoc_GiaoVien",
                        column: x => x.MaGVCN,
                        principalTable: "GIAOVIEN",
                        principalColumn: "MaGV");
                    table.ForeignKey(
                        name: "FK_LopHoc_NamHoc",
                        column: x => x.MaNamHoc,
                        principalTable: "NAMHOC",
                        principalColumn: "MaNamHoc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HOCSINH",
                columns: table => new
                {
                    MaHS = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaLop = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocSinh", x => x.MaHS);
                    table.ForeignKey(
                        name: "FK_HocSinh_LopHoc",
                        column: x => x.MaLop,
                        principalTable: "LOP",
                        principalColumn: "MaLop",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LICHHOC",
                columns: table => new
                {
                    MaLichHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaLop = table.Column<int>(type: "int", nullable: false),
                    MaMonHoc = table.Column<int>(type: "int", nullable: false),
                    MaGV = table.Column<int>(type: "int", nullable: false),
                    MaPhong = table.Column<int>(type: "int", nullable: false),
                    MaHK = table.Column<int>(type: "int", nullable: false),
                    ThuTrongTuan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TietHoc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichHoc", x => x.MaLichHoc);
                    table.ForeignKey(
                        name: "FK_LichHoc_GiaoVien",
                        column: x => x.MaGV,
                        principalTable: "GIAOVIEN",
                        principalColumn: "MaGV",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LichHoc_HocKy",
                        column: x => x.MaHK,
                        principalTable: "HOCKY",
                        principalColumn: "MaHK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LichHoc_LopHoc",
                        column: x => x.MaLop,
                        principalTable: "LOP",
                        principalColumn: "MaLop",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LichHoc_MonHoc",
                        column: x => x.MaMonHoc,
                        principalTable: "MONHOC",
                        principalColumn: "MaMonHoc",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LichHoc_PhongHoc",
                        column: x => x.MaPhong,
                        principalTable: "PHONGHOC",
                        principalColumn: "MaPhong",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PHANCONG_GIANGDAY",
                columns: table => new
                {
                    MaPC = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGV = table.Column<int>(type: "int", nullable: false),
                    MaMonHoc = table.Column<int>(type: "int", nullable: false),
                    MaLop = table.Column<int>(type: "int", nullable: false),
                    MaHK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanCongGiangDay", x => x.MaPC);
                    table.ForeignKey(
                        name: "FK_PhanCong_GiaoVien",
                        column: x => x.MaGV,
                        principalTable: "GIAOVIEN",
                        principalColumn: "MaGV",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhanCong_HocKy",
                        column: x => x.MaHK,
                        principalTable: "HOCKY",
                        principalColumn: "MaHK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhanCong_LopHoc",
                        column: x => x.MaLop,
                        principalTable: "LOP",
                        principalColumn: "MaLop",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhanCong_MonHoc",
                        column: x => x.MaMonHoc,
                        principalTable: "MONHOC",
                        principalColumn: "MaMonHoc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DIEM",
                columns: table => new
                {
                    MaDiem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHS = table.Column<int>(type: "int", nullable: false),
                    MaMonHoc = table.Column<int>(type: "int", nullable: false),
                    MaHK = table.Column<int>(type: "int", nullable: false),
                    DiemMieng = table.Column<double>(type: "float", nullable: true),
                    Diem15Phut = table.Column<double>(type: "float", nullable: true),
                    Diem1Tiet = table.Column<double>(type: "float", nullable: true),
                    DiemThi = table.Column<double>(type: "float", nullable: true),
                    DiemTB = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiemSo", x => x.MaDiem);
                    table.ForeignKey(
                        name: "FK_DiemSo_HocKy",
                        column: x => x.MaHK,
                        principalTable: "HOCKY",
                        principalColumn: "MaHK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiemSo_HocSinh",
                        column: x => x.MaHS,
                        principalTable: "HOCSINH",
                        principalColumn: "MaHS",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiemSo_MonHoc",
                        column: x => x.MaMonHoc,
                        principalTable: "MONHOC",
                        principalColumn: "MaMonHoc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HOCPHI",
                columns: table => new
                {
                    MaHP = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHS = table.Column<int>(type: "int", nullable: false),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NgayDong = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaHK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocPhi", x => x.MaHP);
                    table.ForeignKey(
                        name: "FK_HocPhi_HocKy",
                        column: x => x.MaHK,
                        principalTable: "HOCKY",
                        principalColumn: "MaHK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HocPhi_HocSinh",
                        column: x => x.MaHS,
                        principalTable: "HOCSINH",
                        principalColumn: "MaHS",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TAIKHOAN",
                columns: table => new
                {
                    MaTK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaGV = table.Column<int>(type: "int", nullable: true),
                    MaHS = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.MaTK);
                    table.ForeignKey(
                        name: "FK_TaiKhoan_GiaoVien",
                        column: x => x.MaGV,
                        principalTable: "GIAOVIEN",
                        principalColumn: "MaGV");
                    table.ForeignKey(
                        name: "FK_TaiKhoan_HocSinh",
                        column: x => x.MaHS,
                        principalTable: "HOCSINH",
                        principalColumn: "MaHS");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DIEM_MaHK",
                table: "DIEM",
                column: "MaHK");

            migrationBuilder.CreateIndex(
                name: "IX_DIEM_MaHS",
                table: "DIEM",
                column: "MaHS");

            migrationBuilder.CreateIndex(
                name: "IX_DIEM_MaMonHoc",
                table: "DIEM",
                column: "MaMonHoc");

            migrationBuilder.CreateIndex(
                name: "IX_GIAOVIEN_MonHocMaMonHoc",
                table: "GIAOVIEN",
                column: "MonHocMaMonHoc");

            migrationBuilder.CreateIndex(
                name: "IX_HOCKY_MaNamHoc",
                table: "HOCKY",
                column: "MaNamHoc");

            migrationBuilder.CreateIndex(
                name: "IX_HOCPHI_MaHK",
                table: "HOCPHI",
                column: "MaHK");

            migrationBuilder.CreateIndex(
                name: "IX_HOCPHI_MaHS",
                table: "HOCPHI",
                column: "MaHS");

            migrationBuilder.CreateIndex(
                name: "IX_HOCSINH_MaLop",
                table: "HOCSINH",
                column: "MaLop");

            migrationBuilder.CreateIndex(
                name: "IX_LICHHOC_MaGV",
                table: "LICHHOC",
                column: "MaGV");

            migrationBuilder.CreateIndex(
                name: "IX_LICHHOC_MaHK",
                table: "LICHHOC",
                column: "MaHK");

            migrationBuilder.CreateIndex(
                name: "IX_LICHHOC_MaLop",
                table: "LICHHOC",
                column: "MaLop");

            migrationBuilder.CreateIndex(
                name: "IX_LICHHOC_MaMonHoc",
                table: "LICHHOC",
                column: "MaMonHoc");

            migrationBuilder.CreateIndex(
                name: "IX_LICHHOC_MaPhong",
                table: "LICHHOC",
                column: "MaPhong");

            migrationBuilder.CreateIndex(
                name: "IX_LOP_MaGVCN",
                table: "LOP",
                column: "MaGVCN");

            migrationBuilder.CreateIndex(
                name: "IX_LOP_MaNamHoc",
                table: "LOP",
                column: "MaNamHoc");

            migrationBuilder.CreateIndex(
                name: "IX_PHANCONG_GIANGDAY_MaGV",
                table: "PHANCONG_GIANGDAY",
                column: "MaGV");

            migrationBuilder.CreateIndex(
                name: "IX_PHANCONG_GIANGDAY_MaHK",
                table: "PHANCONG_GIANGDAY",
                column: "MaHK");

            migrationBuilder.CreateIndex(
                name: "IX_PHANCONG_GIANGDAY_MaLop",
                table: "PHANCONG_GIANGDAY",
                column: "MaLop");

            migrationBuilder.CreateIndex(
                name: "IX_PHANCONG_GIANGDAY_MaMonHoc",
                table: "PHANCONG_GIANGDAY",
                column: "MaMonHoc");

            migrationBuilder.CreateIndex(
                name: "IX_TAIKHOAN_MaGV",
                table: "TAIKHOAN",
                column: "MaGV");

            migrationBuilder.CreateIndex(
                name: "IX_TAIKHOAN_MaHS",
                table: "TAIKHOAN",
                column: "MaHS");

            migrationBuilder.CreateIndex(
                name: "IX_TAIKHOAN_TenDangNhap",
                table: "TAIKHOAN",
                column: "TenDangNhap",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DIEM");

            migrationBuilder.DropTable(
                name: "HOCPHI");

            migrationBuilder.DropTable(
                name: "LICHHOC");

            migrationBuilder.DropTable(
                name: "PHANCONG_GIANGDAY");

            migrationBuilder.DropTable(
                name: "TAIKHOAN");

            migrationBuilder.DropTable(
                name: "PHONGHOC");

            migrationBuilder.DropTable(
                name: "HOCKY");

            migrationBuilder.DropTable(
                name: "HOCSINH");

            migrationBuilder.DropTable(
                name: "LOP");

            migrationBuilder.DropTable(
                name: "GIAOVIEN");

            migrationBuilder.DropTable(
                name: "NAMHOC");

            migrationBuilder.DropTable(
                name: "MONHOC");
        }
    }
}
