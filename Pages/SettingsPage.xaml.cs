using System.Globalization;
using InventoryApp.Services;

namespace InventoryApp.Pages;

public partial class SettingsPage : ContentPage
{
    private readonly ApiService _api;

    public SettingsPage(ApiService api)
    {
        InitializeComponent();
        _api = api;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateUI();
        ServerEntry.Text = Preferences.Default.Get("ServerUrl", "http://192.168.1.80:7744");
    }

    private void UpdateUI()
    {
        Title = Lang.Settings;
        SettingsTitle.Text = $"⚙️ {Lang.Settings}";
        LangLabel.Text = Lang.Language;
        ArabicBtn.Text = $"🇸🇦 {Lang.Arabic}";
        EnglishBtn.Text = $"🇬🇧 {Lang.English}";
        ThemeLabel.Text = Lang.Theme;
        LightBtn.Text = $"☀️ {Lang.Light}";
        DarkBtn.Text = $"🌙 {Lang.Dark}";
        ServerLabel.Text = Lang.ServerUrl;
        SaveServerBtn.Text = Lang.Save;
        LogoutBtn.Text = $"🚪 {Lang.Logout}";

        var lang = Preferences.Default.Get("Lang", "ar");
        var theme = Preferences.Default.Get("Theme", "light");

        ArabicBtn.BackgroundColor = lang == "ar" ? Color.FromArgb("#4f46e5") : Color.FromArgb("#e2e8f0");
        ArabicBtn.TextColor = lang == "ar" ? Colors.White : Color.FromArgb("#475569");
        EnglishBtn.BackgroundColor = lang == "en" ? Color.FromArgb("#4f46e5") : Color.FromArgb("#e2e8f0");
        EnglishBtn.TextColor = lang == "en" ? Colors.White : Color.FromArgb("#475569");

        LightBtn.BackgroundColor = theme == "light" ? Color.FromArgb("#4f46e5") : Color.FromArgb("#e2e8f0");
        LightBtn.TextColor = theme == "light" ? Colors.White : Color.FromArgb("#475569");
        DarkBtn.BackgroundColor = theme == "dark" ? Color.FromArgb("#4f46e5") : Color.FromArgb("#e2e8f0");
        DarkBtn.TextColor = theme == "dark" ? Colors.White : Color.FromArgb("#475569");
    }

    private void OnArabicClicked(object? sender, EventArgs e)
    {
        Preferences.Default.Set("Lang", "ar");
        RestartApp();
    }

    private void OnEnglishClicked(object? sender, EventArgs e)
    {
        Preferences.Default.Set("Lang", "en");
        RestartApp();
    }

    private void OnLightClicked(object? sender, EventArgs e)
    {
        Preferences.Default.Set("Theme", "light");
        RestartApp();
    }

    private void OnDarkClicked(object? sender, EventArgs e)
    {
        Preferences.Default.Set("Theme", "dark");
        RestartApp();
    }

    private async void OnSaveServer(object? sender, EventArgs e)
    {
        var url = ServerEntry.Text?.Trim();
        if (!string.IsNullOrEmpty(url))
        {
            Preferences.Default.Set("ServerUrl", url);
            await DisplayAlert(Lang.Ok, "Server URL saved", Lang.Ok);
        }
    }

    private async void OnLogout(object? sender, EventArgs e)
    {
        var confirm = await DisplayAlert(Lang.Logout, Lang.LogoutConfirm, Lang.Logout, Lang.Cancel);
        if (!confirm) return;

        Preferences.Default.Remove("Username");
        Preferences.Default.Remove("ShopName");
        Application.Current!.MainPage = new NavigationPage(new LoginPage(_api));
    }

    private void RestartApp()
    {
        Application.Current!.MainPage = new NavigationPage(new LoginPage(_api));
    }
}
