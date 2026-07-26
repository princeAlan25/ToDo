using BC = BCrypt.Net.BCrypt;

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
