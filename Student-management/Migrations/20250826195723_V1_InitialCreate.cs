using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Student_Management.Migrations
{
    /// <inheritdoc />
    public partial class V1_InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TAIKHOAN",
                keyColumn: "MaTK",
                keyValue: 1,
                column: "MatKhau",
                value: "$2a$11$1ol1RrfNofzgfWAg49Xw3elTKj6wXPIFbGZBk6UJHV8VYIpgd1ICq");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TAIKHOAN",
                keyColumn: "MaTK",
                keyValue: 1,
                column: "MatKhau",
                value: "$2a$12$9e.gY9.jL5.kL5.kL5.kL.uB9.iB9.iB9.iB9.iB9.iB9.iB9.iB9");
        }
    }
}
