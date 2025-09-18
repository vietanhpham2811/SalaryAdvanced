using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SalaryAdvanced.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "request_statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_request_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "system_settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    setting_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    setting_value = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_system_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    manager_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_departments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employee_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    full_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    department_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    basic_salary = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    hire_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_employees", x => x.id);
                    table.ForeignKey(
                        name: "f_k_employees__roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_employees_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "salary_advance_requests",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    request_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    submitted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    approved_by_id = table.Column<int>(type: "integer", nullable: true),
                    rejection_reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_salary_advance_requests", x => x.id);
                    table.ForeignKey(
                        name: "f_k_salary_advance_requests_employees_approved_by_id",
                        column: x => x.approved_by_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "f_k_salary_advance_requests_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_salary_advance_requests_request_statuses_status_id",
                        column: x => x.status_id,
                        principalTable: "request_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "request_statuses",
                columns: new[] { "id", "created_at", "description", "name", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 18, 3, 8, 13, 264, DateTimeKind.Utc).AddTicks(7601), "Chờ phê duyệt", "Pending", new DateTime(2025, 9, 18, 3, 8, 13, 264, DateTimeKind.Utc).AddTicks(7602) },
                    { 2, new DateTime(2025, 9, 18, 3, 8, 13, 264, DateTimeKind.Utc).AddTicks(7603), "Đã phê duyệt", "Approved", new DateTime(2025, 9, 18, 3, 8, 13, 264, DateTimeKind.Utc).AddTicks(7604) },
                    { 3, new DateTime(2025, 9, 18, 3, 8, 13, 264, DateTimeKind.Utc).AddTicks(7605), "Đã từ chối", "Rejected", new DateTime(2025, 9, 18, 3, 8, 13, 264, DateTimeKind.Utc).AddTicks(7606) }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "created_at", "description", "name", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 18, 3, 8, 13, 264, DateTimeKind.Utc).AddTicks(6911), "Nhân viên", "Employee", new DateTime(2025, 9, 18, 3, 8, 13, 264, DateTimeKind.Utc).AddTicks(6911) },
                    { 2, new DateTime(2025, 9, 18, 3, 8, 13, 264, DateTimeKind.Utc).AddTicks(6913), "Quản lý", "Manager", new DateTime(2025, 9, 18, 3, 8, 13, 264, DateTimeKind.Utc).AddTicks(6914) }
                });

            migrationBuilder.InsertData(
                table: "system_settings",
                columns: new[] { "id", "created_at", "description", "setting_key", "setting_value", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 18, 3, 8, 13, 265, DateTimeKind.Utc).AddTicks(1009), "Tỷ lệ % tối đa mỗi lần ứng lương", "MAX_ADVANCE_PERCENTAGE", "50", new DateTime(2025, 9, 18, 3, 8, 13, 265, DateTimeKind.Utc).AddTicks(1010) },
                    { 2, new DateTime(2025, 9, 18, 3, 8, 13, 265, DateTimeKind.Utc).AddTicks(1011), "Tỷ lệ % tối đa hàng tháng", "MAX_MONTHLY_PERCENTAGE", "70", new DateTime(2025, 9, 18, 3, 8, 13, 265, DateTimeKind.Utc).AddTicks(1012) },
                    { 3, new DateTime(2025, 9, 18, 3, 8, 13, 265, DateTimeKind.Utc).AddTicks(1014), "Số lần ứng tối đa mỗi tháng", "MAX_REQUESTS_PER_MONTH", "2", new DateTime(2025, 9, 18, 3, 8, 13, 265, DateTimeKind.Utc).AddTicks(1015) },
                    { 4, new DateTime(2025, 9, 18, 3, 8, 13, 265, DateTimeKind.Utc).AddTicks(1016), "Ngày bắt đầu có thể gửi yêu cầu", "REQUEST_START_DAY", "1", new DateTime(2025, 9, 18, 3, 8, 13, 265, DateTimeKind.Utc).AddTicks(1016) },
                    { 5, new DateTime(2025, 9, 18, 3, 8, 13, 265, DateTimeKind.Utc).AddTicks(1059), "Ngày kết thúc có thể gửi yêu cầu", "REQUEST_END_DAY", "25", new DateTime(2025, 9, 18, 3, 8, 13, 265, DateTimeKind.Utc).AddTicks(1059) }
                });

            migrationBuilder.CreateIndex(
                name: "i_x_departments_code",
                table: "departments",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_departments_manager_id",
                table: "departments",
                column: "manager_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_employees_department_id",
                table: "employees",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "i_x_employees_email",
                table: "employees",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_employees_employee_code",
                table: "employees",
                column: "employee_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_employees_role_id",
                table: "employees",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "i_x_request_statuses_name",
                table: "request_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_roles_name",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_salary_advance_requests_approved_by_id",
                table: "salary_advance_requests",
                column: "approved_by_id");

            migrationBuilder.CreateIndex(
                name: "i_x_salary_advance_requests_employee_id",
                table: "salary_advance_requests",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "i_x_salary_advance_requests_processed_at",
                table: "salary_advance_requests",
                column: "processed_at");

            migrationBuilder.CreateIndex(
                name: "i_x_salary_advance_requests_request_code",
                table: "salary_advance_requests",
                column: "request_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_salary_advance_requests_status_id",
                table: "salary_advance_requests",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "i_x_salary_advance_requests_submitted_at",
                table: "salary_advance_requests",
                column: "submitted_at");

            migrationBuilder.CreateIndex(
                name: "i_x_system_settings_setting_key",
                table: "system_settings",
                column: "setting_key",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "f_k_departments__employees_manager_id",
                table: "departments",
                column: "manager_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_departments__employees_manager_id",
                table: "departments");

            migrationBuilder.DropTable(
                name: "salary_advance_requests");

            migrationBuilder.DropTable(
                name: "system_settings");

            migrationBuilder.DropTable(
                name: "request_statuses");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "departments");
        }
    }
}
