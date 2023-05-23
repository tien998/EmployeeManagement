using System.Diagnostics;
using EmployeeManagement.DbConnection;
using EmployeeManagement.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Tiêm vào thùng chứa DI (DI Container) những dịch vụ (Services) cho Web API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Empl ManagV1", Description = "Employee Management version 1", Version = "V1" });
});

var app = builder.Build();

// Thực thi những dịch vụ (Services) đã được tiêm (Injected)
app.UseSwagger();
app.UseSwaggerUI(opts =>
{
    opts.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management version 1");
});

app.MapGet("/Employees", async () =>
{
    return await EmployDb.Get();
});

app.MapGet("/Employee/{id}", async (int id, ILogger<EmployDb> logger, HttpContext context) =>
{
    var e = await EmployDb.Get(id);
    var rowNull = EmployDb.rowNull;

    if (rowNull)
    {
        context.Response.StatusCode = 404;
    }
    return e;
});

app.MapPost("/Employee", (Employee e, ILogger<Employee> _logger) =>
{
    EmployDb.Create(e);
});

app.MapPut("/Employees", (Employee e) =>
{
    EmployDb.Update(e);
});

app.MapDelete("/Employee/{id}", (int id) =>
{
    EmployDb.Delete(id);
});

app.MapGet("/", () => "Hello World!");


app.Run();
