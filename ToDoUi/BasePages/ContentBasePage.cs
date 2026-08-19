using ToDoUi.Networking.Interfaces;
using ToDoUi.Views;

namespace ToDoUi.BasePages;

public partial class ContentBasePage: ContentPage
{
    protected async override void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            ITokenService? tokenService = 
                IPlatformApplication.Current?.Services?.GetService<ITokenService>()
                ?? Handler?.MauiContext?.Services?.GetService<ITokenService>();
            if (tokenService == null) return;

            bool hasAccessToken = await tokenService.HasAccessTokenAsync();
            if (!hasAccessToken)
            {
                if (Shell.Current == null) return;
                await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }
}
