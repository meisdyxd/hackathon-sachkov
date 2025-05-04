using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SKLAD.Database;
using SKLAD.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SKLAD.Entities;
using Microsoft.OpenApi.Models;
using SKLAD.BackgroundServices;

// о боже упаси

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<AuthService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Warehouse API", Version = "v1" });

    // Добавьте JWT-авторизацию в Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

//builder.Services.AddHostedService<ReplenishmentService>(); // как и говорил идея отвалилась почти сразу)
builder.Services.AddHostedService<InventoryService>();
builder.Services.AddScoped<QuestService>();
builder.Services.AddScoped<AuditQuestGenerator>();
builder.Services.AddScoped<ShortagePredictor>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<PlacementService>();
builder.Services.AddScoped<WarehouseService>();
builder.Services.AddScoped<StorageZoneService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductTransferService>();
builder.Services.AddScoped<ReportsService>();
builder.Services.AddDbContext<WarehouseDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgresql"));
    });

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<WarehouseDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("WarehouseManager", policy =>
        policy.RequireRole("Admin", "Manager"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // роль
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new ApplicationRole
        {
            Name = "Admin",
            Description = "Full access"
        });
    }
    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new ApplicationRole
        {
            Name = "User",
            Description = "Mini access"
        });
    }
    if (!await roleManager.RoleExistsAsync("Manager"))
    {
        await roleManager.CreateAsync(new ApplicationRole
        {
            Name = "Manager",
            Description = "Medium access"
        });
    }

    // админка
    var adminUser = await userManager.FindByEmailAsync("admin@warehouse.com");
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = "admin@warehouse.com",
            Email = "admin@warehouse.com",
            FirstName = "Admin",
            LastName = "User"
        };
        await userManager.CreateAsync(adminUser, "P@ssw0rd!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Warehouse API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
