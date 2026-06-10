using System.Globalization;
using InventoryApp.Pages;
using InventoryApp.Services;

namespace InventoryApp;

public partial class App : Application
{
    public App(ApiService api)
    {
        InitializeComponent();

        var lang = Preferences.Default.Get("Lang", "ar");
        var culture = new CultureInfo(lang == "ar" ? "ar-SA" : "en-US");
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        MainPage = new NavigationPage(new LoginPage(api));
    }
}
