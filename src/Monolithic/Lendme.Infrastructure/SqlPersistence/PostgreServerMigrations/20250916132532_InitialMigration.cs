using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lendme.Infrastructure.SqlPersistence.PostgreServerMigrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:hstore", ",,");

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookingNumber = table.Column<string>(type: "text", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    RenterId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ConfirmedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    PickedUpAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ReturnedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CancellationReason = table.Column<string>(type: "text", nullable: false),
                    CancellationType = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookingStatusHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookingId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStatus = table.Column<int>(type: "integer", nullable: false),
                    ToStatus = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    ChangedBy = table.Column<string>(type: "text", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Metadata = table.Column<Dictionary<string, string>>(type: "hstore", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingStatusHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    IconUrl = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    Bio = table.Column<string>(type: "text", nullable: false),
                    AvatarUrl = table.Column<string>(type: "text", nullable: false),
                    CoverPhotoUrl = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    VerificationLevel = table.Column<int>(type: "integer", nullable: false),
                    IsIdentityVerified = table.Column<bool>(type: "boolean", nullable: false),
                    IdentityVerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Preferences = table.Column<string>(type: "jsonb", nullable: false),
                    Metadata = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastActiveAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookingFinancials",
                columns: table => new
                {
                    BookingId = table.Column<Guid>(type: "uuid", nullable: false),
                    item_price = table.Column<decimal>(type: "numeric", nullable: false),
                    rental_days = table.Column<int>(type: "integer", nullable: false),
                    DailyRate = table.Column<decimal>(type: "numeric", nullable: false),
                    SubTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountCode = table.Column<string>(type: "text", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    PenaltyAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    RefundAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    DamageCompensation = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingFinancials", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_BookingFinancials_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingPayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Purpose = table.Column<int>(type: "integer", nullable: false),
                    Method = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExternalPaymentId = table.Column<string>(type: "text", nullable: false),
                    PaymentProvider = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FailedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FailureReason = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingPayments_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_handovers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActualDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    location_method = table.Column<int>(type: "integer", nullable: false),
                    location_address = table.Column<string>(type: "text", nullable: false),
                    location_meeting_point = table.Column<string>(type: "text", nullable: false),
                    location_instructions = table.Column<string>(type: "text", nullable: false),
                    location_latitude = table.Column<string>(type: "text", nullable: false),
                    location_longitude = table.Column<string>(type: "text", nullable: false),
                    location_coordinates = table.Column<string>(type: "text", nullable: false),
                    condition_grade = table.Column<int>(type: "integer", nullable: false),
                    condition_description = table.Column<string>(type: "text", nullable: false),
                    condition_damages = table.Column<string>(type: "text", nullable: false),
                    condition_has_accessories = table.Column<bool>(type: "boolean", nullable: false),
                    condition_missing_accessories = table.Column<string>(type: "text", nullable: false),
                    condition_is_clean = table.Column<bool>(type: "boolean", nullable: false),
                    condition_is_functional = table.Column<bool>(type: "boolean", nullable: false),
                    photos = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    verify_owner_present = table.Column<bool>(type: "boolean", nullable: false),
                    verify_renter_present = table.Column<bool>(type: "boolean", nullable: false),
                    verify_owner_signature = table.Column<string>(type: "text", nullable: false),
                    verify_renter_signature = table.Column<string>(type: "text", nullable: false),
                    verify_signed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    verify_code = table.Column<string>(type: "text", nullable: false),
                    verify_is_disputed = table.Column<bool>(type: "boolean", nullable: false),
                    verify_dispute_reason = table.Column<string>(type: "text", nullable: false),
                    BookingId1 = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_handovers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_item_handovers_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_handovers_bookings_BookingId1",
                        column: x => x.BookingId1,
                        principalTable: "bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    DailyPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    WeeklyPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    MonthlyPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    DepositAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_preferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    general_preferences = table.Column<string>(type: "jsonb", nullable: false),
                    notification_preferences = table.Column<string>(type: "jsonb", nullable: false),
                    privacy_preferences = table.Column<string>(type: "jsonb", nullable: false),
                    owner_preferences = table.Column<string>(type: "jsonb", nullable: false),
                    renter_preferences = table.Column<string>(type: "jsonb", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_preferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_preferences_UserProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    District = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    Building = table.Column<string>(type: "text", nullable: false),
                    Apartment = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    ContactPhone = table.Column<string>(type: "text", nullable: false),
                    DeliveryInstructions = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddresses_UserProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalListings = table.Column<int>(type: "integer", nullable: false),
                    ActiveListings = table.Column<int>(type: "integer", nullable: false),
                    CompletedRentalsAsOwner = table.Column<int>(type: "integer", nullable: false),
                    CancelledRentalsAsOwner = table.Column<int>(type: "integer", nullable: false),
                    TotalEarnings = table.Column<decimal>(type: "numeric", nullable: false),
                    ThisMonthEarnings = table.Column<decimal>(type: "numeric", nullable: false),
                    ThisYearEarnings = table.Column<decimal>(type: "numeric", nullable: false),
                    AverageRatingAsOwner = table.Column<double>(type: "double precision", nullable: false),
                    ReviewCountAsOwner = table.Column<int>(type: "integer", nullable: false),
                    AverageResponseTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ResponseRate = table.Column<double>(type: "double precision", nullable: false),
                    AcceptanceRate = table.Column<double>(type: "double precision", nullable: false),
                    CompletedRentalsAsRenter = table.Column<int>(type: "integer", nullable: false),
                    CancelledRentalsAsRenter = table.Column<int>(type: "integer", nullable: false),
                    TotalSpent = table.Column<decimal>(type: "numeric", nullable: false),
                    ThisMonthSpent = table.Column<decimal>(type: "numeric", nullable: false),
                    AverageRatingAsRenter = table.Column<double>(type: "double precision", nullable: false),
                    ReviewCountAsRenter = table.Column<int>(type: "integer", nullable: false),
                    MemberSince = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProfileViews = table.Column<int>(type: "integer", nullable: false),
                    TrustScore = table.Column<int>(type: "integer", nullable: false),
                    LastCalculatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStatistics_UserProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VerificationDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    DocumentNumber = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FrontImageUrl = table.Column<string>(type: "text", nullable: false),
                    BackImageUrl = table.Column<string>(type: "text", nullable: false),
                    SelfieWithDocUrl = table.Column<string>(type: "text", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProcessedBy = table.Column<string>(type: "text", nullable: false),
                    RejectionReason = table.Column<string>(type: "text", nullable: false),
                    ExtractedData = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerificationDocuments_UserProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_details",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Tags = table.Column<string>(type: "jsonb", nullable: false),
                    ItemId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_item_details_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_details_Items_ItemId1",
                        column: x => x.ItemId1,
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemAvailabilities",
                columns: table => new
                {
                    ItemDetailsId = table.Column<Guid>(type: "uuid", nullable: false),
                    location_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Point"),
                    location_longitude = table.Column<double>(type: "double precision", precision: 18, scale: 6, nullable: false),
                    location_latitude = table.Column<double>(type: "double precision", precision: 18, scale: 6, nullable: false),
                    location_address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    location_city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    location_district = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    location_radius_meters = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemAvailabilities", x => x.ItemDetailsId);
                    table.ForeignKey(
                        name: "FK_ItemAvailabilities_item_details_ItemDetailsId",
                        column: x => x.ItemDetailsId,
                        principalTable: "item_details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemDetailsId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "text", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    AiTags = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemImages_item_details_ItemDetailsId",
                        column: x => x.ItemDetailsId,
                        principalTable: "item_details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RentalTerms",
                columns: table => new
                {
                    ItemDetailsId = table.Column<Guid>(type: "uuid", nullable: false),
                    terms_pickup_instructions = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    terms_usage_guidelines = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    terms_included_accessories = table.Column<string>(type: "jsonb", nullable: false),
                    terms_cancellation_policy = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    terms_requires_deposit = table.Column<bool>(type: "boolean", nullable: false),
                    terms_requires_insurance = table.Column<bool>(type: "boolean", nullable: false),
                    terms_restricted_uses = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalTerms", x => x.ItemDetailsId);
                    table.ForeignKey(
                        name: "FK_RentalTerms_item_details_ItemDetailsId",
                        column: x => x.ItemDetailsId,
                        principalTable: "item_details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayments_BookingId",
                table: "BookingPayments",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_item_details_ItemId1",
                table: "item_details",
                column: "ItemId1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemDetails_ItemId",
                table: "item_details",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_item_handovers_BookingId",
                table: "item_handovers",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_item_handovers_BookingId1",
                table: "item_handovers",
                column: "BookingId1");

            migrationBuilder.CreateIndex(
                name: "IX_item_handovers_Type_Status",
                table: "item_handovers",
                columns: new[] { "Type", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemDetails_Location_Coordinates",
                table: "ItemAvailabilities",
                columns: new[] { "location_latitude", "location_longitude" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemImages_ItemDetailsId",
                table: "ItemImages",
                column: "ItemDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_UserId",
                table: "user_preferences",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses_ProfileId",
                table: "UserAddresses",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatistics_ProfileId",
                table: "UserStatistics",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VerificationDocuments_ProfileId",
                table: "VerificationDocuments",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingFinancials");

            migrationBuilder.DropTable(
                name: "BookingPayments");

            migrationBuilder.DropTable(
                name: "BookingStatusHistories");

            migrationBuilder.DropTable(
                name: "item_handovers");

            migrationBuilder.DropTable(
                name: "ItemAvailabilities");

            migrationBuilder.DropTable(
                name: "ItemImages");

            migrationBuilder.DropTable(
                name: "RentalTerms");

            migrationBuilder.DropTable(
                name: "user_preferences");

            migrationBuilder.DropTable(
                name: "UserAddresses");

            migrationBuilder.DropTable(
                name: "UserStatistics");

            migrationBuilder.DropTable(
                name: "VerificationDocuments");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "item_details");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
