using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using BC = BCrypt.Net.BCrypt;
using ToDoEntityModels.Models;

namespace ToDoApi.Utilities;

public static class Securit
{
    public static string HashPassword(string password)
    {
        return BC.HashPassword(password);
    }

    public static bool VerifyPassword(string plainPassword, string encryptedPassword)
    {
        return BC.Verify(plainPassword, encryptedPassword);
    }
}
