using ApiGateway.Extensions;
using Application;
using JwtIdentity;
using SqlServerOrm;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddJwtIdentity(builder.Configuration.GetSection("Identity:Token").Value);
builder.Services.AddSqlServerOrm(builder.Configuration.GetConnectionString("Default"));

var app = builder.Build();

app.UseExceptionHandler(ApplicationExceptionHandler.Options);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSqlServerOrm();
app.UseJwtIdentity();

app.MapControllers();

app.Run();
