using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Student_Management.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TAIKHOAN",
                columns: new[] { "MaTK", "MaGV", "MaHS", "MatKhau", "TenDangNhap", "VaiTro" },
                values: new object[] { 1, null, null, "$2a$12$9e.gY9.jL5.kL5.kL5.kL.uB9.iB9.iB9.iB9.iB9.iB9.iB9.iB9", "admin", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TAIKHOAN",
                keyColumn: "MaTK",
                keyValue: 1);
        }
    }
}
