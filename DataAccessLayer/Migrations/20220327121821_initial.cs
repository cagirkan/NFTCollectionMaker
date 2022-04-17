using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    CollectionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollectionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.CollectionID);
                    table.ForeignKey(
                        name: "FK_Collections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Artworks",
                columns: table => new
                {
                    ArtworkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtworkName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CollectionID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artworks", x => x.ArtworkID);
                    table.ForeignKey(
                        name: "FK_Artworks_Collections_CollectionID",
                        column: x => x.CollectionID,
                        principalTable: "Collections",
                        principalColumn: "CollectionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionAnalytics",
                columns: table => new
                {
                    CollectionAnalyticID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value = table.Column<int>(type: "int", nullable: false),
                    CollectionID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionAnalytics", x => x.CollectionAnalyticID);
                    table.ForeignKey(
                        name: "FK_CollectionAnalytics_Collections_CollectionID",
                        column: x => x.CollectionID,
                        principalTable: "Collections",
                        principalColumn: "CollectionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LayerTypes",
                columns: table => new
                {
                    LayerTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LayerTypeName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CollectionID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LayerTypes", x => x.LayerTypeID);
                    table.ForeignKey(
                        name: "FK_LayerTypes_Collections_CollectionID",
                        column: x => x.CollectionID,
                        principalTable: "Collections",
                        principalColumn: "CollectionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtworkTags",
                columns: table => new
                {
                    ArtworkTagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtworkID = table.Column<int>(type: "int", nullable: false),
                    TagID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtworkTags", x => x.ArtworkTagID);
                    table.ForeignKey(
                        name: "FK_ArtworkTags_Artworks_ArtworkID",
                        column: x => x.ArtworkID,
                        principalTable: "Artworks",
                        principalColumn: "ArtworkID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtworkTags_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionLayers",
                columns: table => new
                {
                    CollectionLayerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollectionLayerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LayerIndex = table.Column<int>(type: "int", nullable: false),
                    Popularity = table.Column<int>(type: "int", nullable: false),
                    CollectionID = table.Column<int>(type: "int", nullable: false),
                    LayerTypeID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionLayers", x => x.CollectionLayerID);
                    table.ForeignKey(
                        name: "FK_CollectionLayers_Collections_CollectionID",
                        column: x => x.CollectionID,
                        principalTable: "Collections",
                        principalColumn: "CollectionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionLayers_LayerTypes_LayerTypeID",
                        column: x => x.LayerTypeID,
                        principalTable: "LayerTypes",
                        principalColumn: "LayerTypeID");
                });

            migrationBuilder.CreateTable(
                name: "ArtworkLayers",
                columns: table => new
                {
                    ArtworkLayerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtworkID = table.Column<int>(type: "int", nullable: false),
                    CollectionLayerID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtworkLayers", x => x.ArtworkLayerID);
                    table.ForeignKey(
                        name: "FK_ArtworkLayers_Artworks_ArtworkID",
                        column: x => x.ArtworkID,
                        principalTable: "Artworks",
                        principalColumn: "ArtworkID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtworkLayers_CollectionLayers_CollectionLayerID",
                        column: x => x.CollectionLayerID,
                        principalTable: "CollectionLayers",
                        principalColumn: "CollectionLayerID");
                });

            migrationBuilder.CreateTable(
                name: "LayerTags",
                columns: table => new
                {
                    LayerTagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollectionLayerID = table.Column<int>(type: "int", nullable: false),
                    TagID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LayerTags", x => x.LayerTagID);
                    table.ForeignKey(
                        name: "FK_LayerTags_CollectionLayers_CollectionLayerID",
                        column: x => x.CollectionLayerID,
                        principalTable: "CollectionLayers",
                        principalColumn: "CollectionLayerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LayerTags_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtworkLayers_ArtworkID",
                table: "ArtworkLayers",
                column: "ArtworkID");

            migrationBuilder.CreateIndex(
                name: "IX_ArtworkLayers_CollectionLayerID",
                table: "ArtworkLayers",
                column: "CollectionLayerID");

            migrationBuilder.CreateIndex(
                name: "IX_Artworks_CollectionID",
                table: "Artworks",
                column: "CollectionID");

            migrationBuilder.CreateIndex(
                name: "IX_ArtworkTags_ArtworkID",
                table: "ArtworkTags",
                column: "ArtworkID");

            migrationBuilder.CreateIndex(
                name: "IX_ArtworkTags_TagID",
                table: "ArtworkTags",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionAnalytics_CollectionID",
                table: "CollectionAnalytics",
                column: "CollectionID");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionLayers_CollectionID",
                table: "CollectionLayers",
                column: "CollectionID");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionLayers_LayerTypeID",
                table: "CollectionLayers",
                column: "LayerTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_UserId",
                table: "Collections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LayerTags_CollectionLayerID",
                table: "LayerTags",
                column: "CollectionLayerID");

            migrationBuilder.CreateIndex(
                name: "IX_LayerTags_TagID",
                table: "LayerTags",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_LayerTypes_CollectionID",
                table: "LayerTypes",
                column: "CollectionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtworkLayers");

            migrationBuilder.DropTable(
                name: "ArtworkTags");

            migrationBuilder.DropTable(
                name: "CollectionAnalytics");

            migrationBuilder.DropTable(
                name: "LayerTags");

            migrationBuilder.DropTable(
                name: "Artworks");

            migrationBuilder.DropTable(
                name: "CollectionLayers");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "LayerTypes");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
