using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjectARTEMIS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    InGameName = table.Column<string>(type: "text", nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ColorCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocialMedias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMedias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhitelistRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RealName = table.Column<string>(type: "text", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    FacebookUrl = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhitelistRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerOnlineStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerOnlineStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerOnlineStatuses_PlayerProfiles_PlayerProfileId",
                        column: x => x.PlayerProfileId,
                        principalTable: "PlayerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerProfileStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerProfileStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerProfileStatuses_PlayerProfiles_PlayerProfileId",
                        column: x => x.PlayerProfileId,
                        principalTable: "PlayerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialMediaHandles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SocialMediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Link = table.Column<string>(type: "text", nullable: false),
                    IsMain = table.Column<bool>(type: "boolean", nullable: false),
                    PlayerProfileId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaHandles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialMediaHandles_PlayerProfiles_PlayerProfileId",
                        column: x => x.PlayerProfileId,
                        principalTable: "PlayerProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WhitelistRequestStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RegistrationRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WhitelistRequestId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhitelistRequestStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WhitelistRequestStatuses_WhitelistRequests_WhitelistRequest~",
                        column: x => x.WhitelistRequestId,
                        principalTable: "WhitelistRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "ColorCode", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "#006633", "One of the oldest and most prestigious universities in Cebu, known for its rigorous academic programs and sprawling campuses.", "University of San Carlos (USC)" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "#800000", "Renowned for its excellence in engineering, technology, and producing top-notch board exam placers (Home of the Technologians).", "Cebu Institute of Technology – University (CIT-U)" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "#003366", "A highly respected Catholic institution known for outstanding business, IT, and engineering courses.", "University of San Jose – Recoletos (USJ-R)" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "#7B1113", "The premier state university in the region, highly recognized for its competitive arts, design, and computer science programs.", "University of the Philippines Cebu (UP Cebu)" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "#FFFFFF", "A top-tier medical and health sciences institution located near North Reclamation Area, producing world-class medical professionals.", "Cebu Doctors' University (CDU)" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerOnlineStatuses_PlayerProfileId",
                table: "PlayerOnlineStatuses",
                column: "PlayerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProfileStatuses_PlayerProfileId",
                table: "PlayerProfileStatuses",
                column: "PlayerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaHandles_PlayerProfileId",
                table: "SocialMediaHandles",
                column: "PlayerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_WhitelistRequestStatuses_WhitelistRequestId",
                table: "WhitelistRequestStatuses",
                column: "WhitelistRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerOnlineStatuses");

            migrationBuilder.DropTable(
                name: "PlayerProfileStatuses");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropTable(
                name: "SocialMediaHandles");

            migrationBuilder.DropTable(
                name: "SocialMedias");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WhitelistRequestStatuses");

            migrationBuilder.DropTable(
                name: "PlayerProfiles");

            migrationBuilder.DropTable(
                name: "WhitelistRequests");
        }
    }
}
