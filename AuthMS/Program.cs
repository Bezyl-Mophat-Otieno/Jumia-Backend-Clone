using AuthMS.Data;
using AuthMS.Extensions;
using AuthMS.Models;
using AuthMS.Services;
using AuthMS.Services.Iservice;
using AuthMS.Utilities;
using JumiaAzureServiceBus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuring JWT options

builder.Services.Configure<JWToptions>(builder.Configuration.GetSection("JWToptions"));

// Configuring Identity Framework
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>();

// Register Automapper 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Register services .
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IJWT, JWTservice>();
builder.Services.AddScoped<IMessageBus, MessageBus>();

//Adding the Authentication and Authorization Scheme

builder.AddAuth();




// Add connection string 

builder.Services.AddDbContext<ApplicationDBContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMigrations();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
