namespace TWBuildingAssistant.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Climates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Climates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Factions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProvincialEffects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PublicOrder = table.Column<int>(nullable: false),
                    RegularFood = table.Column<int>(nullable: false),
                    FertilityDependentFood = table.Column<int>(nullable: false),
                    ProvincialSanitation = table.Column<int>(nullable: false),
                    ResearchRate = table.Column<int>(nullable: false),
                    Growth = table.Column<int>(nullable: false),
                    Fertility = table.Column<int>(nullable: false),
                    ReligiousOsmosis = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvincialEffects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionalEffects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PublicOrder = table.Column<int>(nullable: false),
                    RegularFood = table.Column<int>(nullable: false),
                    FertilityDependentFood = table.Column<int>(nullable: false),
                    ProvincialSanitation = table.Column<int>(nullable: false),
                    ResearchRate = table.Column<int>(nullable: false),
                    Growth = table.Column<int>(nullable: false),
                    Fertility = table.Column<int>(nullable: false),
                    ReligiousOsmosis = table.Column<int>(nullable: false),
                    RegionalSanitation = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionalEffects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weathers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weathers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Fertility = table.Column<int>(nullable: false),
                    ClimateId = table.Column<int>(nullable: false),
                    ProvincialEffectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Provinces_Climates_ClimateId",
                        column: x => x.ClimateId,
                        principalTable: "Climates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Provinces_ProvincialEffects_ProvincialEffectId",
                        column: x => x.ProvincialEffectId,
                        principalTable: "ProvincialEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Religions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    ProvincialEffectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Religions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Religions_ProvincialEffects_ProvincialEffectId",
                        column: x => x.ProvincialEffectId,
                        principalTable: "ProvincialEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TechnologyLevels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FactionId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    UniversalProvincialEffectId = table.Column<int>(nullable: true),
                    AntilegacyProvincialEffectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologyLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnologyLevels_ProvincialEffects_AntilegacyProvincialEffectId",
                        column: x => x.AntilegacyProvincialEffectId,
                        principalTable: "ProvincialEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnologyLevels_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechnologyLevels_ProvincialEffects_UniversalProvincialEffectId",
                        column: x => x.UniversalProvincialEffectId,
                        principalTable: "ProvincialEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bonuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<int>(nullable: false),
                    Category = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    RegionalEffectId = table.Column<int>(nullable: true),
                    ProvincialEffectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bonuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bonuses_ProvincialEffects_ProvincialEffectId",
                        column: x => x.ProvincialEffectId,
                        principalTable: "ProvincialEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bonuses_RegionalEffects_RegionalEffectId",
                        column: x => x.RegionalEffectId,
                        principalTable: "RegionalEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuildingLevels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    ParentBuildingLevelId = table.Column<int>(nullable: true),
                    RegionalEffectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildingLevels_BuildingLevels_ParentBuildingLevelId",
                        column: x => x.ParentBuildingLevelId,
                        principalTable: "BuildingLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuildingLevels_RegionalEffects_RegionalEffectId",
                        column: x => x.RegionalEffectId,
                        principalTable: "RegionalEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WeatherEffects",
                columns: table => new
                {
                    ClimateId = table.Column<int>(nullable: false),
                    WeatherId = table.Column<int>(nullable: false),
                    ProvincialEffectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherEffects", x => new { x.ClimateId, x.WeatherId });
                    table.ForeignKey(
                        name: "FK_WeatherEffects_Climates_ClimateId",
                        column: x => x.ClimateId,
                        principalTable: "Climates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeatherEffects_ProvincialEffects_ProvincialEffectId",
                        column: x => x.ProvincialEffectId,
                        principalTable: "ProvincialEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeatherEffects_Weathers_WeatherId",
                        column: x => x.WeatherId,
                        principalTable: "Weathers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    IsCity = table.Column<bool>(nullable: false),
                    ResourceId = table.Column<int>(nullable: true),
                    IsCoastal = table.Column<bool>(nullable: false),
                    SlotsCountOffset = table.Column<int>(nullable: false),
                    ProvinceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regions_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Regions_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Influences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReligionId = table.Column<int>(nullable: true),
                    Value = table.Column<int>(nullable: false),
                    RegionalEffectId = table.Column<int>(nullable: true),
                    ProvincialEffectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Influences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Influences_ProvincialEffects_ProvincialEffectId",
                        column: x => x.ProvincialEffectId,
                        principalTable: "ProvincialEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Influences_RegionalEffects_RegionalEffectId",
                        column: x => x.RegionalEffectId,
                        principalTable: "RegionalEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Influences_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuildingBranches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    SlotType = table.Column<int>(nullable: false),
                    RegionType = table.Column<int>(nullable: true),
                    AllowParallel = table.Column<bool>(nullable: false),
                    RootBuildingLevelId = table.Column<int>(nullable: false),
                    ReligionId = table.Column<int>(nullable: true),
                    ResourceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildingBranches_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuildingBranches_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuildingBranches_BuildingLevels_RootBuildingLevelId",
                        column: x => x.RootBuildingLevelId,
                        principalTable: "BuildingLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuildingLevelLocks",
                columns: table => new
                {
                    BuildingLevelId = table.Column<int>(nullable: false),
                    TechnologyLevelId = table.Column<int>(nullable: false),
                    Antilegacy = table.Column<bool>(nullable: false),
                    Lock = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingLevelLocks", x => new { x.TechnologyLevelId, x.BuildingLevelId });
                    table.ForeignKey(
                        name: "FK_BuildingLevelLocks_BuildingLevels_BuildingLevelId",
                        column: x => x.BuildingLevelId,
                        principalTable: "BuildingLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuildingLevelLocks_TechnologyLevels_TechnologyLevelId",
                        column: x => x.TechnologyLevelId,
                        principalTable: "TechnologyLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuildingBranchUses",
                columns: table => new
                {
                    FactionId = table.Column<int>(nullable: false),
                    BuildingBranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingBranchUses", x => new { x.FactionId, x.BuildingBranchId });
                    table.ForeignKey(
                        name: "FK_BuildingBranchUses_BuildingBranches_BuildingBranchId",
                        column: x => x.BuildingBranchId,
                        principalTable: "BuildingBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuildingBranchUses_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bonuses_ProvincialEffectId",
                table: "Bonuses",
                column: "ProvincialEffectId");

            migrationBuilder.CreateIndex(
                name: "IX_Bonuses_RegionalEffectId",
                table: "Bonuses",
                column: "RegionalEffectId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingBranches_ReligionId",
                table: "BuildingBranches",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingBranches_ResourceId",
                table: "BuildingBranches",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingBranches_RootBuildingLevelId",
                table: "BuildingBranches",
                column: "RootBuildingLevelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuildingBranchUses_BuildingBranchId",
                table: "BuildingBranchUses",
                column: "BuildingBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingLevelLocks_BuildingLevelId",
                table: "BuildingLevelLocks",
                column: "BuildingLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingLevels_ParentBuildingLevelId",
                table: "BuildingLevels",
                column: "ParentBuildingLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingLevels_RegionalEffectId",
                table: "BuildingLevels",
                column: "RegionalEffectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Influences_ProvincialEffectId",
                table: "Influences",
                column: "ProvincialEffectId");

            migrationBuilder.CreateIndex(
                name: "IX_Influences_RegionalEffectId",
                table: "Influences",
                column: "RegionalEffectId");

            migrationBuilder.CreateIndex(
                name: "IX_Influences_ReligionId",
                table: "Influences",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_ClimateId",
                table: "Provinces",
                column: "ClimateId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_ProvincialEffectId",
                table: "Provinces",
                column: "ProvincialEffectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ProvinceId",
                table: "Regions",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ResourceId",
                table: "Regions",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Religions_ProvincialEffectId",
                table: "Religions",
                column: "ProvincialEffectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnologyLevels_AntilegacyProvincialEffectId",
                table: "TechnologyLevels",
                column: "AntilegacyProvincialEffectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnologyLevels_FactionId",
                table: "TechnologyLevels",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologyLevels_UniversalProvincialEffectId",
                table: "TechnologyLevels",
                column: "UniversalProvincialEffectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeatherEffects_ProvincialEffectId",
                table: "WeatherEffects",
                column: "ProvincialEffectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeatherEffects_WeatherId",
                table: "WeatherEffects",
                column: "WeatherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bonuses");

            migrationBuilder.DropTable(
                name: "BuildingBranchUses");

            migrationBuilder.DropTable(
                name: "BuildingLevelLocks");

            migrationBuilder.DropTable(
                name: "Influences");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "WeatherEffects");

            migrationBuilder.DropTable(
                name: "BuildingBranches");

            migrationBuilder.DropTable(
                name: "TechnologyLevels");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Weathers");

            migrationBuilder.DropTable(
                name: "Religions");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "BuildingLevels");

            migrationBuilder.DropTable(
                name: "Factions");

            migrationBuilder.DropTable(
                name: "Climates");

            migrationBuilder.DropTable(
                name: "ProvincialEffects");

            migrationBuilder.DropTable(
                name: "RegionalEffects");
        }
    }
}