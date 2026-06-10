using InventoryApp.Services;
using InventoryApp.Models;

namespace InventoryApp.Pages;

public partial class SessionsPage : ContentPage
{
    private readonly ApiService _api;
    private List<InventorySession> _allSessions = new();

    public SessionsPage(ApiService api)
    {
        InitializeComponent();
        _api = api;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        SessionsTitle.Text = Lang.SessionsTitle;
        SessionsSubtitle.Text = Lang.SessionsSubtitle;
        EmptyLabel.Text = Lang.EmptySessions;
        SessionSearch.Placeholder = Lang.SearchPlaceholder;
        CreateBtn.Text = Lang.NewSession;
        RefreshBtn.Text = Lang.Refresh;
        Title = Lang.SessionsTitle;
        await LoadSessions();
    }

    private async Task LoadSessions()
    {
        try
        {
            _allSessions = await _api.GetSessionsAsync();
            ApplyFilter();
        }
        finally
        {
            SessionsRefresh.IsRefreshing = false;
        }
    }

    private void ApplyFilter()
    {
        var query = SessionSearch.Text?.Trim().ToLower() ?? "";
        var filtered = string.IsNullOrEmpty(query)
            ? _allSessions
            : _allSessions.Where(s =>
                (s.WarehouseName?.ToLower().Contains(query) ?? false) ||
                (s.Notes?.ToLower().Contains(query) ?? false)).ToList();
        SessionsList.ItemsSource = filtered;
    }

    private async void OnRefreshing(object? sender, EventArgs e)
    {
        await LoadSessions();
    }

    private void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        ApplyFilter();
    }

    private async void OnSessionTapped(object? sender, TappedEventArgs e)
    {
        if (sender is Border border && border.BindingContext is InventorySession session)
        {
            await Shell.Current.GoToAsync($"CountPage?sessionId={session.Id}");
        }
    }

    private async void OnCreateSession(object? sender, EventArgs e)
    {
        var warehouses = await _api.GetWarehousesAsync();

        if (warehouses.Count == 0)
        {
            var id = await _api.CreateSessionAsync(null, null);
            if (id.HasValue)
                await Shell.Current.GoToAsync($"CountPage?sessionId={id.Value}");
            return;
        }

        var names = warehouses.Select(w => w.Name).ToArray();
        var selected = await DisplayActionSheet(Lang.SelectWarehouse, Lang.Cancel, null, names);

        if (selected == null) return;
        var warehouse = warehouses[Array.IndexOf(names, selected)];

        var id2 = await _api.CreateSessionAsync(warehouse.Id, null);
        if (id2.HasValue)
            await Shell.Current.GoToAsync($"CountPage?sessionId={id2.Value}");
    }

    private async void OnRefresh(object? sender, EventArgs e)
    {
        await LoadSessions();
    }
}
