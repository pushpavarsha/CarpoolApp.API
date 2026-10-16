using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarpoolApp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRideStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "RideRequests");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "RideRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RideRequestStatuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideRequestStatuses", x => x.StatusId);
                });

            migrationBuilder.InsertData(
                table: "RideRequestStatuses",
                columns: new[] { "StatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Approved" },
                    { 3, "Rejected" },
                    { 4, "Completed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RideRequests_StatusId",
                table: "RideRequests",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_RideRequests_RideRequestStatuses_StatusId",
                table: "RideRequests",
                column: "StatusId",
                principalTable: "RideRequestStatuses",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideRequests_RideRequestStatuses_StatusId",
                table: "RideRequests");

            migrationBuilder.DropTable(
                name: "RideRequestStatuses");

            migrationBuilder.DropIndex(
                name: "IX_RideRequests_StatusId",
                table: "RideRequests");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "RideRequests");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "RideRequests",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
