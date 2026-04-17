using MojiiBackend.Infrastructure;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

//Infrastructure/DependencyInjection.cs
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MojiiBackend API",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MojiiBackend API v1");
        options.RoutePrefix = "swagger";
    });
}

//Infrastructure/DependencyInjection.cs
app.UseInfrastructure();

await app.RunAsync();
