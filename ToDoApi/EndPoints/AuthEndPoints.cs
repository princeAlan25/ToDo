
using ToDoShared.DTOs;
using ToDoApi.IIntermediators;
using ToDoApi.IRepositories;

namespace ToDoApi.EndPoints;

public static partial class Program
{
    public static void MapAuthenticationEndPoints(this WebApplication app)
    {
        RouteGroupBuilder authEndPointBuilder = app.MapGroup("/auth").WithTags("Authentication EndPoints");

        authEndPointBuilder.MapPost(
        pattern: "/signup",
        handler: (IAuthentication authService, SignUpRequestDto signUpReqeust) =>
        {
            return authService.SignUp(signUpReqeust);
        }).WithName("SignUp");

        authEndPointBuilder.MapPost(
            pattern: "/login",
            handler: (IAuthentication authService ,LoginRequestDto loginReqeust) =>
            {
                return authService.SignIn(loginReqeust);
            }).WithName("SignIn");

    }
}
