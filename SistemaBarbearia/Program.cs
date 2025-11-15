using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaBarbearia.Data;
using SistemaBarbearia.Models;
using SistemaBarbearia.Enums;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
       .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
       .AddEnvironmentVariables();
;
; // Permite sobrescrever configs via ENV

// -------------------- Database --------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
                  ?? Environment.GetEnvironmentVariable("DB_CONNECTION")
                  ?? throw new InvalidOperationException("Connection string não definida.");
    options.UseNpgsql(connStr);
});

// -------------------- Identity --------------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = true;

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// -------------------- JWT Authentication --------------------
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// -------------------- Swagger --------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Digite: Bearer {seu token aqui}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// -------------------- Controllers --------------------
builder.Services.AddControllers();

// -------------------- CORS --------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// -------------------- Middleware --------------------
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// -------------------- Seed Roles & AdminMaster --------------------
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = { UserRoles.Cliente, UserRoles.Funcionario, UserRoles.Admin, UserRoles.AdminMaster };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Ler ADMIN_PHONE e ADMIN_PASSWORD das variáveis de ambiente
    var adminPhone = Environment.GetEnvironmentVariable("ADMIN_PHONE")
                     ?? builder.Configuration["Admin:Phone"];
    var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD")
                        ?? builder.Configuration["Admin:Password"];

    if (string.IsNullOrWhiteSpace(adminPhone) || string.IsNullOrWhiteSpace(adminPassword))
        throw new Exception("ADMIN_PHONE ou ADMIN_PASSWORD não definidos!");

    var masterUser = await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == adminPhone);

    if (masterUser == null)
    {
        masterUser = new ApplicationUser
        {
            UserName = adminPhone,
            PhoneNumber = adminPhone,
            PhoneNumberConfirmed = true,
            NomeCompleto = "Administrador Master",
            TipoUsuario = UserRoles.AdminMaster,
            Nivel = UserLevels.AdminMaster
        };

        var result = await userManager.CreateAsync(masterUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(masterUser, UserRoles.AdminMaster);
        }
        else
        {
            throw new Exception("Falha ao criar AdminMaster: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}

// -------------------- Apply Migrations --------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();
