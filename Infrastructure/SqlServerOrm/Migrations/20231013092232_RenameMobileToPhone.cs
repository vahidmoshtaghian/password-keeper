using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServerOrm.Migrations
{
    /// <inheritdoc />
    public partial class RenameMobileToPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Mobile",
                schema: "Actor",
                table: "People",
                newName: "Phone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                schema: "Actor",
                table: "People",
                newName: "Mobile");
        }
    }
}
