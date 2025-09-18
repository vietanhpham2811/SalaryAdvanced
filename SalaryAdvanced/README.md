# SalaryAdvanced Clean Architecture Setup Guide

Tôi đã setup xong Clean Architecture cho project SalaryAdvanced với các thành phần sau:

## ✅ Hoàn thành:

### 1. **Cấu trúc Clean Architecture**
```
SalaryAdvanced/
├── Domain/
│   ├── Entities/ (BaseEntity, Department, Employee, Role, RequestStatus, SalaryAdvanceRequest, SystemSetting)
│   ├── Enums/ (RequestStatus, UserRole)
│   └── Interfaces/
├── Application/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Services/
└── Infrastructure/
    ├── Data/ (ApplicationDbContext + Configurations)
    └── Repositories/
```

### 2. **NuGet Packages đã cài đặt:**
- `Microsoft.EntityFrameworkCore` v7.0.20
- `Microsoft.EntityFrameworkCore.Design` v7.0.20
- `Microsoft.EntityFrameworkCore.Tools` v7.0.20
- `Npgsql.EntityFrameworkCore.PostgreSQL` v7.0.18
- `BCrypt.Net-Next` v4.0.3

### 3. **Entity Framework Configuration:**
- ApplicationDbContext với tất cả Entity configurations
- Snake_case naming convention cho PostgreSQL
- Audit fields (CreatedAt, UpdatedAt) tự động
- Relationships và constraints đầy đủ
- Seed data cho reference tables (Roles, RequestStatuses, SystemSettings)

### 4. **Connection Strings đã cấu hình:**
- `appsettings.json`: Production database
- `appsettings.Development.json`: Development database

## 🔧 Cần làm tiếp:

### Bước 1: Cài đặt PostgreSQL
1. Tải và cài đặt PostgreSQL từ: https://www.postgresql.org/download/
2. Tạo database mới hoặc cập nhật connection string với thông tin đúng

### Bước 2: Cập nhật Connection String
Mở `appsettings.Development.json` và thay đổi:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=SalaryAdvancedDb_Dev;Username=YOUR_USERNAME;Password=YOUR_PASSWORD;"
  }
}
```

### Bước 3: Tạo và chạy Migration
```bash
# Tạo migration
dotnet ef migrations add InitialCreate

# Cập nhật database
dotnet ef database update
```

### Bước 4: Chạy ứng dụng
```bash
dotnet run
```

## 📋 Database Schema được tạo:

### Tables:
- **departments**: Phòng ban với manager
- **employees**: Nhân viên với department và role
- **roles**: Vai trò (Employee, Manager)
- **request_statuses**: Trạng thái request (Pending, Approved, Rejected)
- **salary_advance_requests**: Yêu cầu ứng lương
- **system_settings**: Cài đặt hệ thống (giới hạn %, số lần,...)

### Relationships:
- Department → Manager (Employee)
- Employee → Department (Many-to-One)
- Employee → Role (Many-to-One)
- SalaryAdvanceRequest → Employee (Many-to-One)
- SalaryAdvanceRequest → RequestStatus (Many-to-One)
- SalaryAdvanceRequest → ApprovedBy (Employee, nullable)

### Business Rules được implement:
- Unique constraints (codes, emails)
- Decimal precision cho salary và amount
- Audit timestamps
- Foreign key constraints với appropriate delete behaviors

## 🎯 Tiếp theo có thể phát triển:
1. Repository Pattern
2. Business Services
3. Authentication & Authorization
4. Validation
5. DTOs và Mapping
6. Blazor Components cho UI