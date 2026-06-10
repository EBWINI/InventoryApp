namespace InventoryApp.Models;

public class InventorySession
{
    public int Id { get; set; }
    public int? WarehouseId { get; set; }
    public string WarehouseName { get; set; } = "";
    public string StartDate { get; set; } = "";
    public string? EndDate { get; set; }
    public string Status { get; set; } = "";
    public string? Notes { get; set; }
}

public class InventoryItem
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public string? Barcode { get; set; }
    public int ExpectedQuantity { get; set; }
    public int CountedQuantity { get; set; }
}

public class SessionDetail
{
    public InventorySession Session { get; set; } = new();
    public List<InventoryItem> Items { get; set; } = new();
}

public class ApiResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}

public class ScanResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int ItemId { get; set; }
    public string? ProductName { get; set; }
    public int NewCount { get; set; }
    public int ExpectedQuantity { get; set; }
}

public class Warehouse
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
