using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleApp106.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Batch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentTblId = table.Column<int>(nullable: false),
                    EquipmentProcessId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", nullable: true),
                    DateTimeUploaded = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Compressor_Cleaning",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>(nullable: false),
                    Date = table.Column<string>(nullable: true),
                    TD1 = table.Column<string>(nullable: true),
                    TS1 = table.Column<string>(nullable: true),
                    TD2 = table.Column<string>(nullable: true),
                    TS2 = table.Column<string>(nullable: true),
                    PD1 = table.Column<string>(nullable: true),
                    PD2 = table.Column<string>(nullable: true),
                    DT1 = table.Column<string>(nullable: true),
                    DT2 = table.Column<string>(nullable: true),
                    PR1 = table.Column<string>(nullable: true),
                    PR2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compressor_Cleaning", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compressor_Cleaning_Batch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Compressor_Error",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>(nullable: false),
                    rowAffected = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compressor_Error", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compressor_Error_Batch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Compressor_Predicted",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>(nullable: false),
                    Date = table.Column<string>(nullable: true),
                    TD1 = table.Column<string>(nullable: true),
                    TS1 = table.Column<string>(nullable: true),
                    TD2 = table.Column<string>(nullable: true),
                    TS2 = table.Column<string>(nullable: true),
                    PD1 = table.Column<string>(nullable: true),
                    PD2 = table.Column<string>(nullable: true),
                    DT1 = table.Column<string>(nullable: true),
                    DT2 = table.Column<string>(nullable: true),
                    PR1 = table.Column<string>(nullable: true),
                    PR2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compressor_Predicted", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compressor_Predicted_Batch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Compressor_Processed",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>(nullable: false),
                    Date = table.Column<string>(nullable: true),
                    TD1 = table.Column<string>(nullable: true),
                    TS1 = table.Column<string>(nullable: true),
                    TD2 = table.Column<string>(nullable: true),
                    TS2 = table.Column<string>(nullable: true),
                    PD1 = table.Column<string>(nullable: true),
                    PD2 = table.Column<string>(nullable: true),
                    DT1 = table.Column<string>(nullable: true),
                    DT2 = table.Column<string>(nullable: true),
                    PR1 = table.Column<string>(nullable: true),
                    PR2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compressor_Processed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compressor_Processed_Batch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Compressor_Staging",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>(nullable: false),
                    Date = table.Column<string>(nullable: true),
                    TD1 = table.Column<string>(nullable: true),
                    TS1 = table.Column<string>(nullable: true),
                    TD2 = table.Column<string>(nullable: true),
                    TS2 = table.Column<string>(nullable: true),
                    PD1 = table.Column<string>(nullable: true),
                    PD2 = table.Column<string>(nullable: true),
                    DT1 = table.Column<string>(nullable: true),
                    DT2 = table.Column<string>(nullable: true),
                    PR1 = table.Column<string>(nullable: true),
                    PR2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compressor_Staging", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compressor_Staging_Batch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compressor_Cleaning_BatchId",
                table: "Compressor_Cleaning",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Compressor_Error_BatchId",
                table: "Compressor_Error",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Compressor_Predicted_BatchId",
                table: "Compressor_Predicted",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Compressor_Processed_BatchId",
                table: "Compressor_Processed",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Compressor_Staging_BatchId",
                table: "Compressor_Staging",
                column: "BatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Compressor_Cleaning");

            migrationBuilder.DropTable(
                name: "Compressor_Error");

            migrationBuilder.DropTable(
                name: "Compressor_Predicted");

            migrationBuilder.DropTable(
                name: "Compressor_Processed");

            migrationBuilder.DropTable(
                name: "Compressor_Staging");

            migrationBuilder.DropTable(
                name: "Batch");
        }
    }
}
