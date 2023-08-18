using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.Xml;
using System.Text;
using WebServer.Authentication;
using WebServer.Models.ClinicData.Entities;
using WebServer.Models.DTOs;
using WebServer.Models.MedicineData;
using WebServer.Models.UserData;
using WebServer.Services;
using WebServer.Services.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContextPool<UserContext>(options =>
{
    string ConnectionString = builder.Configuration["ConnectionStrings:UserDataConnectionString"];
    options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
});
builder.Services.AddDbContextPool<CompanyContext>(options =>
{
    string ConnectionString = builder.Configuration["ConnectionStrings:MedicineDbConnectionString"];
    options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
});
builder.Services.AddDbContextPool<IssuerContext>(options =>
{
    string ConnectionString = builder.Configuration["ConnectionStrings:MedicineDbConnectionString"];
    options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
});
builder.Services.AddDbContextPool<MedicineContext>(options =>
{
    string ConnectionString = builder.Configuration["ConnectionStrings:MedicineDbConnectionString"];
    options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
});

builder.Services.AddScoped<IDbService<User, User, User>, UserService>();
builder.Services.AddScoped<IDbService<Doctor, Doctor, Doctor>, DoctorService>();
builder.Services.AddScoped<IDbService<Patient, PatientBasicDTO, PatientDetailsDTO>, PatientService>();
builder.Services.AddScoped<IDbService<Company, Company, Company>, CompanyService>();
builder.Services.AddScoped<IDbService<Issuer, Issuer, Issuer>, IssuerService>();
builder.Services.AddScoped<IDbService<Medicine, Medicine, Medicine>, MedicineService>();

builder.Services.AddScoped<ITokenService<JWTToken>, JWTManager>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        builder.AllowAnyOrigin()
       .AllowAnyMethod()
       .AllowAnyHeader());
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(bearer =>
{
    bearer.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Web Server",
            Version = String.Format("{0}.{1}.{2}", builder.Configuration["Version:Major"], builder.Configuration["Version:Minor"], builder.Configuration["Version:Patch"]),
            Description = "Web Server Endpoint for Medical app"
        });

    swagger.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
    {
        Description = "JWT Authentication",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "JWT"
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:7241").AllowCredentials());
app.MapControllers();
app.Run();
