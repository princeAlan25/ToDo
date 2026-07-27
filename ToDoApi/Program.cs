using Microsoft.AspNetCore.Authentication.JwtBearer;
using ToDoApi.EndPoints;
using ToDoApi.IIntermediators;
using ToDoApi.IRepositories;
using ToDoApi.Repositories;
using ToDoApi.Services;
using ToDoEntityModels.DataContexts;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new SecurityTokenInvalidSigningKeyException())),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddToDoContextService();
builder.Services.AddProblemDetails();

//services registration
builder.Services.AddTransient<IUser, UserRepo>();
builder.Services.AddTransient<IUserIntermediator, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () =>
{
    return "Welcome to the api";
});

//endpoints mapping
app.MapUserEndPoints();

app.Run();
