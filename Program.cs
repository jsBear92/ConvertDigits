using Microsoft.EntityFrameworkCore;
using ConvertDigits.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add a DbContext to the container to store temporarily the numbers in memory
// This is to avoid the need to create a database for the project
// Whenever the project is restarted, the numbers will be lost
builder.Services.AddDbContext<NumberContext>(opt =>
    opt.UseInMemoryDatabase("Numbers"));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Enable CORS
app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();
