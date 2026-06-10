using ZXing.Net.Maui;

namespace InventoryApp;

public static class ScanService
{
    public static async Task<string?> ScanBarcodeAsync(Page parent)
    {
        var page = new ScanPage();
        var tcs = new TaskCompletionSource<string?>();

        page.BarcodeDetected += (_, barcode) =>
        {
            tcs.TrySetResult(barcode);
        };

        parent.Navigation.PushModalAsync(page);

        try
        {
            return await tcs.Task;
        }
        finally
        {
            await parent.Navigation.PopModalAsync();
        }
    }
}

public partial class ScanPage : ContentPage
{
    public event EventHandler<string?>? BarcodeDetected;
    private bool _isProcessing;

    public ScanPage()
    {
        BackgroundColor = Colors.Black;

        var barcodeView = new ZXing.Net.Maui.Controls.CameraBarcodeReaderView
        {
            Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormats.OneDimensional,
                AutoRotate = true,
                Multiple = false,
                TryHarder = true
            },
            IsDetecting = true,
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        barcodeView.BarcodesDetected += (sender, args) =>
        {
            if (_isProcessing) return;
            _isProcessing = true;

            var result = args.Results?.FirstOrDefault();
            if (result?.Value != null)
            {
                try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); } catch { }
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    barcodeView.IsDetecting = false;
                    BarcodeDetected?.Invoke(this, result.Value);
                });
            }
        };

        var cancelBtn = new Button
        {
            Text = "Cancel",
            BackgroundColor = Colors.Gray,
            TextColor = Colors.White,
            HeightRequest = 50,
            CornerRadius = 8,
            Margin = new Thickness(16)
        };
        cancelBtn.Clicked += (_, _) =>
        {
            BarcodeDetected?.Invoke(this, null);
        };

        Content = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto)
            },
            Children =
            {
                barcodeView,
                cancelBtn
            }
        };
        Grid.SetRow(cancelBtn, 1);
    }
}
