using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using SalaryAdvanced.Data;
using SalaryAdvanced.Infrastructure.Data;
using SalaryAdvanced.Domain.Entities;
using SalaryAdvanced.Domain.Interfaces;
using SalaryAdvanced.Infrastructure.Repositories;
using SalaryAdvanced.Application.Interfaces;
using SalaryAdvanced.Application.Services;
using SalaryAdvanced.Infrastructure.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    
    // Enable detailed errors in development
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// Add Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    
    // User settings
    options.User.RequireUniqueEmail = true;
    
    // Sign in settings
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Add Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Employee", policy => 
        policy.RequireRole("Employee", "Manager"));
    options.AddPolicy("Manager", policy => 
        policy.RequireRole("Manager"));
});

// Add Repository pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IRequestStatusRepository, RequestStatusRepository>();
builder.Services.AddScoped<ISalaryAdvanceRequestRepository, SalaryAdvanceRequestRepository>();
builder.Services.AddScoped<ISystemSettingRepository, SystemSettingRepository>();

// Add Application Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ISalaryAdvanceService, SalaryAdvanceService>();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add Authentication State Provider for Blazor Server
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// Legacy service for sample pages
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    // Auto-migrate database in development
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        
        try
        {
            // Migrate database
            dbContext.Database.Migrate();
            
            // Seed roles
            await SeedRolesAndUsersAsync(roleManager, userManager, dbContext);
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
        }
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

// Helper method to seed initial data
static async Task SeedRolesAndUsersAsync(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
{
    // Create roles if they don't exist
    string[] roles = { "Manager", "Employee" };
    
    foreach (string role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = role, Description = role });
        }
    }
    
    // Create default admin user
    if (!await userManager.Users.AnyAsync())
    {
        // First, ensure we have departments and roles in the database
        if (!context.Departments.Any())
        {
            context.Departments.Add(new Department
            {
                Name = "Công nghệ thông tin",
                Code = "IT",
                Description = "Phòng CNTT"
            });
            await context.SaveChangesAsync();
        }
        
        // Seed ApplicationRoles - roleManager is already provided as parameter
        if (!await roleManager.RoleExistsAsync("Employee"))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = "Employee" });
        }
        
        if (!await roleManager.RoleExistsAsync("Manager"))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = "Manager" });
        }
        
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = "Admin" });
        }
        
        var department = context.Departments.First();
        
        var adminUser = new ApplicationUser
        {
            EmployeeCode = "ADM001",
            FullName = "System Administrator",
            UserName = "admin",
            Email = "admin@company.com",
            EmailConfirmed = true,
            DepartmentId = department.Id,
            BasicSalary = 25000000,
            HireDate = DateTime.Now.AddYears(-2),
            IsActive = true
        };
        
        var result = await userManager.CreateAsync(adminUser, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
            
            // Update department manager
            department.ManagerId = adminUser.Id;
            context.Departments.Update(department);
            await context.SaveChangesAsync();
        }
    }
}
