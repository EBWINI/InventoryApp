using InventoryApp.Services;
using InventoryApp.Models;

namespace InventoryApp.Pages;

[QueryProperty(nameof(SessionId), "sessionId")]
public partial class CountPage : ContentPage
{
    private readonly ApiService _api;
    private int _sessionId;
    private List<InventoryItem> _items = new();

    public int SessionId
    {
        get => _sessionId;
        set
        {
            _sessionId = value;
            _ = LoadSession();
        }
    }

    public CountPage(ApiService api)
    {
        InitializeComponent();
        _api = api;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ScanTitle.Text = Lang.ScanBarcode;
        BarcodeEntry.Placeholder = Lang.BarcodePlaceholder;
        ScanBtn.Text = Lang.Lookup;
        CameraBtn.Text = Lang.CameraScan;
        NoItemsLabel.Text = Lang.NoItems;
        Title = Lang.SessionCount;
        if (!string.IsNullOrEmpty(BarcodeEntry.Text))
            BarcodeEntry.Focus();
    }

    private async Task LoadSession()
    {
        var detail = await _api.GetSessionAsync(_sessionId);
        if (detail == null) return;

        WarehouseLabel.Text = $"{detail.Session.WarehouseName} — {detail.Session.StartDate}";
        StatusLabel.Text = detail.Session.Status;

        _items = detail.Items;
        ItemsList.ItemsSource = _items;
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        var counted = _items.Count(i => i.CountedQuantity > 0);
        var total = _items.Count;
        ProgressLabel.Text = string.Format(Lang.ItemsCountedFormat, counted, total, _items.Sum(i => i.CountedQuantity));
        ProgressBar.Progress = total > 0 ? (double)counted / total : 0;
    }

    private async void OnScanBarcode(object? sender, EventArgs e)
    {
        var barcode = BarcodeEntry.Text?.Trim();
        if (string.IsNullOrEmpty(barcode)) return;

        var result = await _api.ScanBarcodeAsync(_sessionId, barcode);
        if (result?.Success == true)
        {
            var item = _items.FirstOrDefault(i => i.Id == result.ItemId);
            if (item != null)
            {
                item.CountedQuantity = result.NewCount;
                ItemsList.ItemsSource = null;
                ItemsList.ItemsSource = _items;
                UpdateProgress();
                BarcodeEntry.Text = "";

                try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); } catch { }
            }
        }
        else
        {
            await DisplayAlert(Lang.NotFound, string.Format(Lang.NotFoundMessage, barcode), Lang.Ok);
        }
    }

    private async void OnIncrement(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is InventoryItem item)
        {
            item.CountedQuantity++;
            await _api.UpdateItemCountAsync(item.Id, item.CountedQuantity);
            ItemsList.ItemsSource = null;
            ItemsList.ItemsSource = _items;
            UpdateProgress();
            try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); } catch { }
        }
    }

    private async void OnDecrement(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is InventoryItem item && item.CountedQuantity > 0)
        {
            item.CountedQuantity--;
            await _api.UpdateItemCountAsync(item.Id, item.CountedQuantity);
            ItemsList.ItemsSource = null;
            ItemsList.ItemsSource = _items;
            UpdateProgress();
            try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); } catch { }
        }
    }

    private async void OnEditQuantity(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is InventoryItem item)
        {
            var result = await DisplayPromptAsync(
                Lang.EditQuantity,
                $"{Lang.EnterQuantity} {item.ProductName}",
                Lang.Ok, Lang.Cancel,
                placeholder: item.CountedQuantity.ToString(),
                keyboard: Keyboard.Numeric);

            if (int.TryParse(result, out var qty) && qty >= 0)
            {
                item.CountedQuantity = qty;
                await _api.UpdateItemCountAsync(item.Id, qty);
                ItemsList.ItemsSource = null;
                ItemsList.ItemsSource = _items;
                UpdateProgress();
            }
        }
    }

    private async void OnCameraScan(object? sender, EventArgs e)
    {
        var result = await ScanService.ScanBarcodeAsync(this);
        if (result != null)
        {
            BarcodeEntry.Text = result;
            OnScanBarcode(null, EventArgs.Empty);
        }
    }

    private async void OnCloseSession(object? sender, EventArgs e)
    {
        var confirm = await DisplayAlert(Lang.CloseConfirmTitle, Lang.CloseConfirmMessage, Lang.CloseConfirmYes, Lang.CloseConfirmCancel);
        if (!confirm) return;

        var success = await _api.CloseSessionAsync(_sessionId);
        if (success)
        {
            CloseBtn.IsEnabled = false;
            CloseBtn.Text = Lang.Closed;
            StatusLabel.Text = Lang.Closed;
            await DisplayAlert(Lang.Ok, Lang.ClosedDone, Lang.Ok);
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert(Lang.Error, Lang.CloseError, Lang.Ok);
        }
    }
}
