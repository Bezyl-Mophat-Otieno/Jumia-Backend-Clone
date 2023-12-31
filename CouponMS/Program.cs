using CouponMS.Data;
using CouponMS.Extensions;
using CouponMS.Services;
using CouponMS.Services.Iservices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Stripe Configuration

Stripe.StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:apikey").Value;

// Register Automapper Service 

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add connection string 

builder.Services.AddDbContext<ApplicationDBContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// Adding the extended functionalities : Enabling JWT Route Protection
builder.AddAuth();

// Registering services
builder.Services.AddScoped<ICoupon, Couponservice>();

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
