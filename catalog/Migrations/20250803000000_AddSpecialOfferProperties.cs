using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GloboTicket.Catalog.Migrations
{
    public partial class AddSpecialOfferProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add IsOnSpecialOffer column with default value of false
            migrationBuilder.AddColumn<bool>(
                name: "IsOnSpecialOffer",
                table: "Events",
                nullable: false,
                defaultValue: false);

            // Add OriginalPrice column with default value equal to current Price
            migrationBuilder.AddColumn<int>(
                name: "OriginalPrice",
                table: "Events",
                nullable: false,
                defaultValueSql: "Price");

            // Update existing records to set OriginalPrice equal to Price
            migrationBuilder.Sql("UPDATE Events SET OriginalPrice = Price");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnSpecialOffer",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "Events");
        }
    }
}
