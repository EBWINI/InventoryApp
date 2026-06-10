using InventoryApp.Services;
using InventoryApp.Models;

namespace InventoryApp.Pages;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _api;

    public LoginPage(ApiService api)
    {
        InitializeComponent();
        _api = api;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ServerUrlEntry.Text = Preferences.Default.Get("ServerUrl", "http://192.168.1.80:7744");
        UsernameEntry.Text = Preferences.Default.Get("Username", "");
        ShopNameEntry.Text = Preferences.Default.Get("ShopName", "");
        PasswordEntry.Text = "";

        TitleLabel.Text = Lang.AppTitle;
        SubtitleLabel.Text = Lang.AppTitle;
        ServerUrlLabel.Text = Lang.ServerUrl;
        ShopNameLabel.Text = Lang.ShopName;
        UsernameLabel.Text = Lang.Username;
        PasswordLabel.Text = Lang.Password;
        LoginBtn.Text = Lang.LoginButton;
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        var serverUrl = ServerUrlEntry.Text?.Trim();
        var username = UsernameEntry.Text?.Trim();
        var password = PasswordEntry.Text;
        var shopName = ShopNameEntry.Text?.Trim();

        if (string.IsNullOrEmpty(serverUrl) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            StatusLabel.Text = "Please fill all required fields";
            StatusLabel.TextColor = Colors.Red;
            return;
        }

        LoginBtn.IsEnabled = false;
        StatusLabel.Text = "Connecting...";
        StatusLabel.TextColor = Colors.Gray;

        _api.BaseUrl = serverUrl;
        var success = await _api.LoginAsync(username, password, shopName);

        if (success)
        {
            Preferences.Default.Set("ServerUrl", serverUrl);
            Preferences.Default.Set("Username", username);
            if (!string.IsNullOrEmpty(shopName))
                Preferences.Default.Set("ShopName", shopName);
            Application.Current!.MainPage = new AppShell();
        }
        else
        {
            var err = _api.LastError;
            StatusLabel.Text = string.IsNullOrEmpty(err) ? "Login failed. Check your credentials and server URL." : $"Error: {err}";
            StatusLabel.TextColor = Colors.Red;
            LoginBtn.IsEnabled = true;
        }
    }
}
