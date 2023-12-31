using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionMS.Migrations
{
    /// <inheritdoc />
    public partial class AddedtheCouponCodeFieldonSalesModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CouponCode",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponCode",
                table: "Sales");
        }
    }
}
