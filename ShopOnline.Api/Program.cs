global using ShopOnline.Api.Data;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Api.Repositories;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ShopOnlineDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnection"));
});

//×¢²áController
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

//¿çÓò·ÃÎÊ
app.UseCors(policy =>
    policy.WithOrigins("https://localhost:7003/", "https://localhost:7003")
    .AllowAnyMethod()
    .WithHeaders(HeaderNames.ContentType)
);


app.UseAuthorization();

app.MapControllers();

app.Run();
