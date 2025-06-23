using Microsoft.EntityFrameworkCore;
using System;
using WarehouseApp.API.Data;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7073);

});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();




//app.MapGet("/", () => "Warehouse API is running");


app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();
//Test

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
