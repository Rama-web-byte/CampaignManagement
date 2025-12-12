using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampaignManagement.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsActiveFromCampaign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Campaigns");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Campaigns",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
