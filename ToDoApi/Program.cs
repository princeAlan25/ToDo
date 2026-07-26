using ToDoApi.EndPoints;
using ToDoApi.IIntermediators;
using ToDoApi.IRepositories;
using ToDoApi.Repositories;
using ToDoApi.Services;
using ToDoEntityModels.DataContexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
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

app.MapGet("/", () =>
{
    return "Welcome to the api";
});

//endpoints mapping
app.MapUserEndPoints();

app.Run();
