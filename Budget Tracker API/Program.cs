using Budget_Tracker_API.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// --- NEW ADDITION: Add CORS Policy ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b =>
        {
            b.AllowAnyOrigin()  // Allows the frontend to connect from localhost:3000, 4200, etc.
             .AllowAnyMethod()  // Fixes the 405 error (allows OPTIONS, GET, POST, PUT, DELETE)
             .AllowAnyHeader(); // Allows custom headers (like Authorization tokens)
        });
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // This effectively breaks the loop by nulling out the repeating reference
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddControllers();

// --- PREVIOUS FIX: Remove conflicting OpenApi lines ---
// builder.Services.AddOpenApi(); <--- REMOVE THIS

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.

// --- NEW ADDITION: Enable CORS Middleware ---
// IMPORTANT: This must be placed BEFORE UseAuthorization
app.UseCors("AllowAll");

// app.MapOpenApi(); <--- REMOVE THIS (Conflict)

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // --- PREVIOUS FIX: Relative path for Swagger JSON ---
    c.SwaggerEndpoint("swagger/v1/swagger.json", "My API V1");

    // Keep this to make Swagger the homepage
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();