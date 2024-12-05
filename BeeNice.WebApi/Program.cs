using System.Text;
using BeeNice.WebApi.Data;
using BeeNice.WebApi.Middleware;
using BeeNice.WebApi.Repositories;
using BeeNice.WebApi.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BeeNiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
        options.Password.RequiredLength = 10)
    .AddEntityFrameworkStores<BeeNiceDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
        };
    });

// Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IApiaryRepository, ApiaryRepository>();
builder.Services.AddScoped<IHiveRepository, HiveRepository>();
builder.Services.AddScoped<IBeeFamilyRepository, BeeFamilyRepository>();
builder.Services.AddScoped<IQueenRepository, QueenRepository>();
builder.Services.AddScoped<IHoneyCollectionRepository, HoneyCollectionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<JwtTokenMiddleware>(builder.Configuration["Jwt:Secret"]);
app.UseMiddleware<UserMiddleware>();
app.MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
