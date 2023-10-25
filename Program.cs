using System.Text;
using ApiAvocados;
using ApiAvocados.Models;
using ApiAvocados.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
     options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        }); ;

        // instalar dotnet add package Swashbuckle.AspNetCore.Filters --version 7.0.11
        options.OperationFilter<SecurityRequirementsOperationFilter>();
    }
);


builder.Services.AddDbContext<ApiContext>();
builder.Services.AddScoped<IWhaterService, WhaterService>();
builder.Services.AddScoped<IAvocadosService, AvocadoService>();
builder.Services.AddScoped<IImageUploadService, ImageUploadService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                 Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)
            ),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    }
);

builder.Services.AddAuthorization(
    options =>
    {
        options.AddPolicy("Admin", policy => policy.RequireRole(UserRole.Admin.ToString()));
        options.AddPolicy("Customer", policy => policy.RequireRole(UserRole.Customer.ToString()));
        options.AddPolicy("All", policy => policy.RequireRole(UserRole.Customer.ToString(), UserRole.Admin.ToString()));
    }
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("http://localhost:4200") // Reemplaza con el origen permitido
            .AllowAnyMethod()
            .AllowAnyHeader()
        );
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

//app.UseHttpsRedirection();

app.UseAuthorization();



app.MapControllers();

app.Run();

