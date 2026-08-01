using Microsoft.AspNetCore.Authentication.JwtBearer;
using ToDoApi.EndPoints;
using ToDoApi.IIntermediators;
using ToDoApi.IRepositories;
using ToDoApi.Repositories;
using ToDoApi.Services;
using ToDoEntityModels.DataContexts;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new SecurityTokenInvalidSigningKeyException())),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("authenticated", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

//configuring middleware error handlers
builder.Services.AddToDoContextService();
builder.Services.AddProblemDetails();

//Repositories registration
builder.Services.AddTransient<IUserRepository, UserRepo>();
builder.Services.AddTransient<IRoleRepository, RoleRepo>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepo>();

//intermediators/services registration
builder.Services.AddTransient<IUserIntermediator, UserService>();
builder.Services.AddTransient<IAuthentication, AuthService>();
builder.Services.AddTransient<IRoleIntermediator, RoleService>();
builder.Services.AddTransient<ICategoryIntermediator, CategoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseExceptionHandler();
    app.UseSwagger().UseSwaggerUI(options => options.DocumentTitle = "ToDo API documentation");
};
app.UseHttpsRedirection();
app.UseAuthentication().UseAuthorization();

//endpoints mapping
app.MapGet("/", () => "Welcome to the api");
app.MapAuthenticationEndPoints();
app.MapUserEndPoints();
app.MapRoleEndPoints();
app.MapCategoryEndpoints();

app.Run();
