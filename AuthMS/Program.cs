using AuthMS.Data;
using AuthMS.Extensions;
using AuthMS.Models;
using AuthMS.Services;
using AuthMS.Services.Iservice;
using AuthMS.SignalR.Hubs;
using AuthMS.Utilities;
using JumiaAzureServiceBus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var allowOriginsPolicy = "AllowAllOrigins";


// AddSignalR connection service 

builder.Services.AddSignalR();

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

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(allowOriginsPolicy, builder =>
    {
        builder
            .WithOrigins("http://localhost:5173") // Adjust this to your frontend's URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // If needed for authentication (e.g., using cookies)
    });
});




// Add connection string 

builder.Services.AddDbContext<ApplicationDBContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

var app = builder.Build();
app.UseCors(allowOriginsPolicy);

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

app.MapHub<RegistrationHub>("/register");

app.Run();
