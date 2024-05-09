using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SK.TinyScheduler.Database.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "job",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    cron = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "job_instance",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    state = table.Column<int>(type: "int", nullable: false),
                    error = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    cron = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_instance", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "step",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    job_id = table.Column<long>(type: "bigint", nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    break_on_error = table.Column<bool>(type: "bit", nullable: false),
                    retries = table.Column<int>(type: "int", nullable: false),
                    script = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    parent_step_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_step", x => x.id);
                    table.ForeignKey(
                        name: "FK_step_job_job_id",
                        column: x => x.job_id,
                        principalTable: "job",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_step_step_parent_step_id",
                        column: x => x.parent_step_id,
                        principalTable: "step",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "step_instance",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    state = table.Column<int>(type: "int", nullable: false),
                    error = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    job_instance_id = table.Column<long>(type: "bigint", nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    break_on_error = table.Column<bool>(type: "bit", nullable: false),
                    retries = table.Column<int>(type: "int", nullable: false),
                    script = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    parent_step_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_step_instance", x => x.id);
                    table.ForeignKey(
                        name: "FK_step_instance_job_instance_job_instance_id",
                        column: x => x.job_instance_id,
                        principalTable: "job_instance",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_step_instance_step_instance_parent_step_id",
                        column: x => x.parent_step_id,
                        principalTable: "step_instance",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_step_job_id",
                table: "step",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_step_parent_step_id",
                table: "step",
                column: "parent_step_id");

            migrationBuilder.CreateIndex(
                name: "IX_step_instance_job_instance_id",
                table: "step_instance",
                column: "job_instance_id");

            migrationBuilder.CreateIndex(
                name: "IX_step_instance_parent_step_id",
                table: "step_instance",
                column: "parent_step_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "step");

            migrationBuilder.DropTable(
                name: "step_instance");

            migrationBuilder.DropTable(
                name: "job");

            migrationBuilder.DropTable(
                name: "job_instance");
        }
    }
}
