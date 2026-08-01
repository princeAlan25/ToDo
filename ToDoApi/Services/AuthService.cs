using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using ToDoApi.IIntermediators;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using ToDoApi.IRepositories;
using ToDoApi.DTOs;
using ToDoEntityModels.Models;
using ToDoApi.Utilities;

namespace ToDoApi.Services;

public class AuthService(IConfiguration config, IUserRepository userRepo) : IAuthentication
{
    public IResult SignIn(LoginUserDto loginUserRequest)
    {
        if(Validator.ValidateUserSignIn(loginUserRequest) is not null) return Results.BadRequest(Validator.ValidateUserSignIn(loginUserRequest));
        User? user = userRepo.GetUserByEmailAsync(loginUserRequest.email).Result;
        if (user == null || !Securit.VerifyPassword(loginUserRequest.password, user.Password)) return Results.Unauthorized();
        return Results.Ok(new
        {
            AccessToken = GenerateAuthToken(user.UserId, user.Name),
            TokenType = "Bearer"
        });
    }

    public IResult SignUp(CreateUserDto createUserRequest)
    {
        if(Validator.ValidateUserSignUp(createUserRequest) is not null) return Results.BadRequest(Validator.ValidateUserSignUp(createUserRequest));
        UserDto user = userRepo.CreateUserAsync(createUserRequest).Result;
        if (user is null) return Results.BadRequest("Signup failed");
        return Results.Created("/user", user);
    }

    private string GenerateAuthToken(Guid userId, string userName)
    {
        try
        {
            List<Claim> claims = [
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, userName)
            ];

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? throw new SecurityTokenInvalidSigningKeyException()));
            SigningCredentials cridentials = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(3),
                signingCredentials: cridentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch(Exception ex)
        {
            return ex.Message;
        }
    }
}
