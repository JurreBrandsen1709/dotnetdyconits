using DotnetDyconits.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// // add services
// builder.Services.AddTransient(next => new RequestDelegate(context => Task.CompletedTask));
// builder.Services.AddTransient<ObjectWrapper>();

// builder.Services.AddTransient<DyconitMiddleware>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseMiddleware<DyconitMiddleware>();


app.UseStaticFiles();

app.UseRouting();

app.UseDyconitsMiddleware();

app.UseAuthorization();

app.MapControllers();

app.Run();
