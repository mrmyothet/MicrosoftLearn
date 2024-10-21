using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<FruitDb>(
    options =>
    {
        options.UseSqlServer(connectionString);
    },
    ServiceLifetime.Transient,
    ServiceLifetime.Transient
);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Version = "v1",
            Title = "Fruit API",
            Description = "API for managing a list of fruit and their stock status.",
            TermsOfService = new Uri("https://example.com/terms")
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapPost(
        "/fruitlist",
        async (Fruit fruit, FruitDb db) =>
        {
            db.Fruits.Add(fruit);
            await db.SaveChangesAsync();

            return Results.Created($"/fruitlist/{fruit.Id}", fruit);
        }
    )
    .WithTags("Add fruit to list");

//app.MapControllers();

app.Run();
