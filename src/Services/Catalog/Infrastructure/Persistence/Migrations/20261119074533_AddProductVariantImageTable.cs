using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Vendora.Services.Catalog.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductVariantImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_variant_images",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_image_id = table.Column<long>(type: "bigint", nullable: false),
                    product_variant_id = table.Column<long>(type: "bigint", nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_variant_images", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_variant_images_product_images_product_image_id",
                        column: x => x.product_image_id,
                        principalTable: "product_images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_variant_images_product_variants_product_variant_id",
                        column: x => x.product_variant_id,
                        principalTable: "product_variants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ux_product_images_primary",
                table: "product_images",
                column: "product_id",
                unique: true,
                filter: "is_primary = TRUE");

            migrationBuilder.CreateIndex(
                name: "IX_product_variant_images_product_image_id",
                table: "product_variant_images",
                column: "product_image_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_variant_images_product_variant_id_product_image_id",
                table: "product_variant_images",
                columns: new[] { "product_variant_id", "product_image_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_variant_images_product_variant_id_sort_order",
                table: "product_variant_images",
                columns: new[] { "product_variant_id", "sort_order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_product_variant_images_primary",
                table: "product_variant_images",
                column: "product_variant_id",
                unique: true,
                filter: "is_primary = TRUE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_variant_images");

            migrationBuilder.DropIndex(
                name: "ux_product_images_primary",
                table: "product_images");
        }
    }
}
