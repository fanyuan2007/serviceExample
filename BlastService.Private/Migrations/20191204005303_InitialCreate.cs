using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace BlastService.Private.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "Fragmentation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    P10 = table.Column<double>(nullable: true),
                    P20 = table.Column<double>(nullable: true),
                    P30 = table.Column<double>(nullable: true),
                    P40 = table.Column<double>(nullable: true),
                    P50 = table.Column<double>(nullable: true),
                    P60 = table.Column<double>(nullable: true),
                    P70 = table.Column<double>(nullable: true),
                    P80 = table.Column<double>(nullable: true),
                    P90 = table.Column<double>(nullable: true),
                    TopSize = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fragmentation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    UTCOffsetHours = table.Column<int>(nullable: false),
                    UTCOffsetMinutes = table.Column<int>(nullable: false),
                    UTCIdName = table.Column<string>(nullable: false),
                    UTCIsDaylightSavingTime = table.Column<bool>(nullable: false),
                    LocalTransformation = table.Column<string>(type: "jsonb", nullable: true),
                    Mapping = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patterns",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Stage = table.Column<string>(nullable: false),
                    FaceAngle = table.Column<double>(nullable: false),
                    SubDrill = table.Column<double>(nullable: false),
                    HoleUsage = table.Column<string>(nullable: false),
                    PatternType = table.Column<string>(type: "text", nullable: false),
                    Purpose = table.Column<string>(nullable: false),
                    IsElectronic = table.Column<bool>(nullable: false),
                    PowderFactor = table.Column<double>(nullable: false),
                    RockFactor = table.Column<double>(nullable: false),
                    RockSG = table.Column<double>(nullable: false),
                    ValidationState = table.Column<string>(type: "text", nullable: false),
                    DesignBoundary = table.Column<MultiLineString>(type: "geography", nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Bench = table.Column<string>(nullable: true),
                    Pit = table.Column<string>(nullable: true),
                    Phase = table.Column<string>(nullable: true),
                    Area = table.Column<double>(nullable: true),
                    Volume = table.Column<double>(nullable: true),
                    AverageHoleLength = table.Column<double>(nullable: true),
                    TotalHoleLength = table.Column<double>(nullable: true),
                    TotalScore = table.Column<double>(nullable: true),
                    GeologyCode = table.Column<string>(nullable: true),
                    PatternTemplateName = table.Column<string>(nullable: true),
                    ChargingTemplateName = table.Column<string>(nullable: true),
                    MaxHoleFired = table.Column<int>(nullable: true),
                    MaxWeightFired = table.Column<double>(nullable: true),
                    MetricScores = table.Column<string>(nullable: true),
                    ActualBoundary = table.Column<MultiLineString>(type: "geography", nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false),
                    ActualFragmentId = table.Column<Guid>(nullable: true),
                    DesignFragmentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patterns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patterns_Fragmentation_ActualFragmentId",
                        column: x => x.ActualFragmentId,
                        principalTable: "Fragmentation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patterns_Fragmentation_DesignFragmentId",
                        column: x => x.DesignFragmentId,
                        principalTable: "Fragmentation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patterns_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Holes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    AreaOfInfluence = table.Column<double>(nullable: false),
                    VolumeOfInfluence = table.Column<double>(nullable: false),
                    HoleState = table.Column<string>(nullable: false),
                    HoleUsage = table.Column<string>(nullable: false),
                    LayoutType = table.Column<string>(nullable: false),
                    DesignDiameter = table.Column<double>(nullable: false),
                    DesignSubDrill = table.Column<double>(nullable: false),
                    ValidationState = table.Column<int>(nullable: false),
                    DesignAzimuth = table.Column<double>(nullable: false),
                    DesignDip = table.Column<double>(nullable: false),
                    DesignLength = table.Column<double>(nullable: false),
                    DesignCollar = table.Column<Point>(type: "geography", nullable: false),
                    DesignToe = table.Column<Point>(type: "geography", nullable: false),
                    DesignBenchCollar = table.Column<double>(nullable: true),
                    DesignBenchToe = table.Column<double>(nullable: true),
                    Accuracy = table.Column<double>(nullable: true),
                    LengthAccuracy = table.Column<double>(nullable: true),
                    DesignChargeWeight = table.Column<double>(nullable: true),
                    DesignChargeThickness = table.Column<double>(nullable: true),
                    ActualChargeWeight = table.Column<double>(nullable: true),
                    ActualChargeThickness = table.Column<double>(nullable: true),
                    ChargeTemplateName = table.Column<string>(nullable: true),
                    FragmentSize = table.Column<double>(nullable: true),
                    PowderFactor = table.Column<double>(nullable: true),
                    GeologyCode = table.Column<string>(nullable: true),
                    DesignTrace = table.Column<LineString>(type: "geography", nullable: true),
                    ActualTrace = table.Column<LineString>(type: "geography", nullable: true),
                    ActualAzimuth = table.Column<double>(nullable: true),
                    ActualDip = table.Column<double>(nullable: true),
                    ActualLength = table.Column<double>(nullable: true),
                    ActualCollar = table.Column<Point>(type: "geography", nullable: true),
                    ActualToe = table.Column<Point>(type: "geography", nullable: true),
                    DrillPatternId = table.Column<Guid>(nullable: false),
                    BlastPatternId = table.Column<Guid>(nullable: true),
                    ActualFragmentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Holes_Fragmentation_ActualFragmentId",
                        column: x => x.ActualFragmentId,
                        principalTable: "Fragmentation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Holes_Patterns_BlastPatternId",
                        column: x => x.BlastPatternId,
                        principalTable: "Patterns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Holes_Patterns_DrillPatternId",
                        column: x => x.DrillPatternId,
                        principalTable: "Patterns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChargingIntervals",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    From = table.Column<double>(nullable: false),
                    To = table.Column<double>(nullable: false),
                    ProfileType = table.Column<string>(type: "text", nullable: false),
                    Consumable = table.Column<string>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Deck = table.Column<string>(nullable: true),
                    HoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargingIntervals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChargingIntervals_Holes_HoleId",
                        column: x => x.HoleId,
                        principalTable: "Holes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChargingIntervals_HoleId",
                table: "ChargingIntervals",
                column: "HoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Holes_ActualFragmentId",
                table: "Holes",
                column: "ActualFragmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Holes_BlastPatternId",
                table: "Holes",
                column: "BlastPatternId");

            migrationBuilder.CreateIndex(
                name: "IX_Holes_DrillPatternId",
                table: "Holes",
                column: "DrillPatternId");

            migrationBuilder.CreateIndex(
                name: "IX_Patterns_ActualFragmentId",
                table: "Patterns",
                column: "ActualFragmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Patterns_DesignFragmentId",
                table: "Patterns",
                column: "DesignFragmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Patterns_ProjectId",
                table: "Patterns",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChargingIntervals");

            migrationBuilder.DropTable(
                name: "Holes");

            migrationBuilder.DropTable(
                name: "Patterns");

            migrationBuilder.DropTable(
                name: "Fragmentation");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
