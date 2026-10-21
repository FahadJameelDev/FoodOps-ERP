using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOps_ERP.Migrations
{
    /// <inheritdoc />
    public partial class Group5_AIEngine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AIAssistantLogs",
                columns: table => new
                {
                    AILogID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    DomainContext = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    QuestionSummary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AnswerSummary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModelName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TokensUsed = table.Column<int>(type: "int", nullable: true),
                    ExecutionTimeMs = table.Column<int>(type: "int", nullable: true),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIAssistantLogs", x => x.AILogID);
                    table.ForeignKey(
                        name: "FK_AIAssistantLogs_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AIInsightCaches",
                columns: table => new
                {
                    CacheID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleContext = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CacheKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIInsightCaches", x => x.CacheID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIAssistantLogs_CreatedAt",
                table: "AIAssistantLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AIAssistantLogs_DomainContext_CreatedAt",
                table: "AIAssistantLogs",
                columns: new[] { "DomainContext", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AIAssistantLogs_UserID_CreatedAt",
                table: "AIAssistantLogs",
                columns: new[] { "UserID", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AIInsightCaches_ExpiresAt",
                table: "AIInsightCaches",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_AIInsightCaches_ModuleContext_CacheKey",
                table: "AIInsightCaches",
                columns: new[] { "ModuleContext", "CacheKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AIAssistantLogs");

            migrationBuilder.DropTable(
                name: "AIInsightCaches");
        }
    }
}
