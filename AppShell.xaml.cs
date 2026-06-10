using InventoryApp.Pages;

namespace InventoryApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("CountPage", typeof(CountPage));
    }
}
