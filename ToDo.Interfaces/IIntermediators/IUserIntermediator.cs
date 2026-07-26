namespace ToDo.Interfaces.IIntermediators;

public interface IUserIntermediator
{
    HttpClientHandler GetUserByIdAsync(string userId);

}
