using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace LendMe.Catalog.Infrastructure.SqlPersistence.PostgreServerMigrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_trgm", ",,")
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    icon_url = table.Column<string>(type: "text", nullable: true),
                    display_order = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                    table.ForeignKey(
                        name: "fk_categories_categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "rents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    type = table.Column<string>(type: "text", nullable: false, defaultValue: "Point"),
                    min_price = table.Column<double>(type: "double precision", nullable: false),
                    max_price = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    district = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    radius_meters = table.Column<int>(type: "integer", nullable: false, defaultValue: 1000),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    location = table.Column<Point>(type: "geography(POINT, 4326)", nullable: false),
                    terms_pickup_instructions = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    terms_usage_guidelines = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    terms_included_accessories = table.Column<string>(type: "jsonb", nullable: false),
                    terms_cancellation_policy = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    terms_requires_deposit = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    terms_requires_insurance = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    terms_restricted_uses = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    identify_number = table.Column<string>(type: "text", nullable: false),
                    daily_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    weekly_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    monthly_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    deposit_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    is_available = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_items_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "item_details",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    tags = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_item_details", x => x.id);
                    table.ForeignKey(
                        name: "fk_item_details_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rent_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rent_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rent_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_rent_items_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_rent_items_rents_rent_id",
                        column: x => x.rent_id,
                        principalTable: "rents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_details_id = table.Column<Guid>(type: "uuid", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    thumbnail_url = table.Column<string>(type: "text", nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    ai_tags = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_item_image", x => x.id);
                    table.ForeignKey(
                        name: "fk_item_image_item_details_item_details_id",
                        column: x => x.item_details_id,
                        principalTable: "item_details",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_categories_name",
                table: "categories",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_categories_parent_id",
                table: "categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_item_details_item_id",
                table: "item_details",
                column: "item_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_item_image_item_details_id",
                table: "item_image",
                column: "item_details_id");

            migrationBuilder.CreateIndex(
                name: "ix_items_category_id",
                table: "items",
                column: "category_id",
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_items_created_at",
                table: "items",
                column: "created_at",
                descending: new bool[0],
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_items_daily_price",
                table: "items",
                column: "daily_price",
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_items_is_available",
                table: "items",
                column: "is_available",
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_items_status",
                table: "items",
                column: "status",
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_items_title",
                table: "items",
                column: "title")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "ix_rent_items_item_id_rent_id",
                table: "rent_items",
                columns: new[] { "item_id", "rent_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_rent_items_rent_id",
                table: "rent_items",
                column: "rent_id");

            migrationBuilder.CreateIndex(
                name: "ix_rents_city",
                table: "rents",
                column: "city");

            migrationBuilder.CreateIndex(
                name: "ix_rents_district",
                table: "rents",
                column: "district");

            migrationBuilder.CreateIndex(
                name: "ix_rents_location",
                table: "rents",
                column: "location")
                .Annotation("Npgsql:IndexMethod", "gist");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item_image");

            migrationBuilder.DropTable(
                name: "rent_items");

            migrationBuilder.DropTable(
                name: "item_details");

            migrationBuilder.DropTable(
                name: "rents");

            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
