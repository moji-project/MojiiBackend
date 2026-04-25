using MojiiBackend.Infrastructure;
using Microsoft.OpenApi;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using MojiiBackend.Domain.Entities;

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
        Version = "v1",
        Description = "API REST MojiiBackend (Auth, Users, Posts, Events, Chat, Realtime, Reports, Feedback)."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
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

/* C est comme ça que j'ai généré un mot de pase de base
var hasher = new PasswordHasher<User>();
var hash = hasher.HashPassword(new User(), "Password1");
Console.WriteLine(hash);
*/

/* Et commme ça que j'ai vérifvié qu'il correspond bien (ça loggait 'résultat :Success')
var hasher = new PasswordHasher<User>(); // Remplace User par ta classe IdentityUser
var storedHash = "AQAAAAIAAYagAAAAEJMYvnVYX5xc6g1z+x87i4aLP/O2sLWIgp31WRqewgUw2vGfKTOApvSzWBnLzc58Ag==";
var passwordToTest = "Password1";

var result = hasher.VerifyHashedPassword(new User(), storedHash, passwordToTest);
Console.WriteLine("résultat :" + result);
*/


await app.RunAsync();
