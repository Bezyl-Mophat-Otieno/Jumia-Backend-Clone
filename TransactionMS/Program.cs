using Microsoft.EntityFrameworkCore;
using TransactionMS.Data;
using TransactionMS.Extensions;
using TransactionMS.Services;
using TransactionMS.Services.Iservice;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//Adding the Authentication and Authorization Scheme
builder.AddAuth();

// Add HttpClient to aid in the Communication between our external service
builder.Services.AddHttpClient("Products", c => c.BaseAddress = new Uri(builder.Configuration.GetSection("ServiceUrls:productMS").Value));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IProduct, ProductService>();
builder.Services.AddScoped<IOrder, OrderService>();

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



app.UseHttpsRedirection();
app.UseAuthentication();

app.UseMigrations();    

app.UseAuthorization();

app.MapControllers();

app.Run();
