using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CC_Karriarpartner.Migrations
{
    /// <inheritdoc />
    public partial class userUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailVerification",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerification",
                table: "Users");
        }
    }
}
