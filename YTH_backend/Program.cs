using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using YTH_backend.Data;
using YTH_backend.Infrastructure.Email;
using YTH_backend.Models.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var jwtSecret = builder.Configuration["Jwt:Secret"];
// var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
//                 ?? throw new InvalidOperationException("JWT_SECRET is not set.");

var smtpPassword = builder.Configuration["Email:SmtpPassword"] ?? "test";
                   // ?? Environment.GetEnvironmentVariable("SMTP_PASSWORD")
                   // ?? throw new InvalidOperationException("SMTP_PASSWORD is not configured.");

// Add services
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,        // или true + ValidIssuer, если нужно
            ValidateAudience = false,      // или true + ValidAudience
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            // ВАЖНО: чтобы roles работали с [Authorize(Roles = "...")]
            RoleClaimType = "roles"
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = ctx =>
            {
                if (ctx.Principal.Identity is not ClaimsIdentity id)
                    return Task.CompletedTask;

                var contextJson = id.FindFirst("context")?.Value;
                if (string.IsNullOrWhiteSpace(contextJson))
                {
                    ctx.Fail("Missing 'context' claim.");
                    return Task.CompletedTask;
                }

                try
                {
                    using var doc = JsonDocument.Parse(contextJson);
                    var root = doc.RootElement;
                    
                    if (!root.TryGetProperty("email", out var emailEl) 
                        || emailEl.ValueKind != JsonValueKind.String
                        || string.IsNullOrWhiteSpace(emailEl.GetString()))
                    {
                        ctx.Fail("Claim 'email' is required in 'context' for role 'with_confirmed_email'.");
                        return Task.CompletedTask;
                    }

                    id.AddClaim(new Claim("email", emailEl.GetString()));
                    
                    if (!root.TryGetProperty("id", out var idEl) 
                        || idEl.ValueKind != JsonValueKind.String
                        || string.IsNullOrWhiteSpace(idEl.GetString()))
                    {
                        ctx.Fail("Claim 'id' is required in 'context' for role 'with_confirmed_email'.");
                        return Task.CompletedTask;
                    }
                    
                    id.AddClaim(new Claim("id", idEl.GetString()));

                    // Опциональные поля — без fail
                    // if (root.TryGetProperty("id", out var idEl) 
                    //     && idEl.ValueKind == JsonValueKind.String)
                    // {
                    //     id.AddClaim(new Claim("id", idEl.GetString()));
                    // }
                }
                catch (Exception ex)
                {
                    ctx.Fail($"Failed to parse 'context': {ex.Message}");
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "YTH API", Version = "v1" });

    // Добавляем поддержку Bearer токена
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите JWT в формате: {ваш_токен}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("logged_in", policy => policy.RequireRole("logged_in", "student", "admin", "superadmin"))
    .AddPolicy("admin", policy => policy.RequireRole("admin", "superadmin"));

builder.Services.AddSingleton(new JwtSettings(jwtSecret));
//TODO
builder.Services.AddSingleton<IEmailService>(new MailKitEmailService(
    smtpHost: "smtp.example.com",
    smtpPort: 587,
    smtpUser: "user@example.com",
    smtpPassword: smtpPassword,
    fromEmail: "no-reply@example.com",
    fromName: "MyApp"
));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDistributedMemoryCache(); // ← локальный, в памяти
}
else
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        var redisConn = Environment.GetEnvironmentVariable("REDIS_CONNECTION")
                        ?? throw new InvalidOperationException("REDIS_CONNECTION is not set.");

        options.Configuration = redisConn;
        options.InstanceName = "yth_login_";
    });
}

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing."),
        npgsqlOptions => npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "healthy", time = DateTime.UtcNow }));
// Initialize DB (create DB file if not exists)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.Use(async (ctx, next) =>
{
    ctx.Response.Headers["X-Frame-Options"] = "DENY"; // или "SAMEORIGIN"
    await next();
});
app.Use(async (ctx, next) =>
{
    ctx.Response.Headers["X-Content-Type-Options"] = "nosniff";
    await next();
});
app.UseRouting();
app.UseAuthentication();  
app.UseAuthorization();
app.MapControllers();
app.Run();