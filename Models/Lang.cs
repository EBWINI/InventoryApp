namespace InventoryApp;

public static class Lang
{
    private static string L => Preferences.Default.Get("Lang", "ar");

    public static string AppTitle => L == "ar" ? "تطبيق الجرد" : "Inventory App";
    public static string SessionsTitle => L == "ar" ? "جلسات الجرد" : "Inventory Sessions";
    public static string SessionsSubtitle => L == "ar" ? "قائمة جلسات الجرد" : "Session List";
    public static string EmptySessions => L == "ar" ? "لا توجد جلسات بعد" : "No sessions yet";
    public static string NewSession => L == "ar" ? "＋ جلسة جديدة" : "＋ New Session";
    public static string Refresh => L == "ar" ? "🔄 تحديث" : "🔄 Refresh";
    public static string SearchPlaceholder => L == "ar" ? "بحث..." : "Search...";
    public static string SelectWarehouse => L == "ar" ? "اختر المستودع" : "Select Warehouse";
    public static string Cancel => L == "ar" ? "إلغاء" : "Cancel";

    public static string ServerUrl => L == "ar" ? "رابط السيرفر" : "Server URL";
    public static string ShopName => L == "ar" ? "اسم المحل (اختياري)" : "Shop Name (optional)";
    public static string Username => L == "ar" ? "اسم المستخدم" : "Username";
    public static string Password => L == "ar" ? "كلمة المرور" : "Password";
    public static string LoginButton => L == "ar" ? "🔐  تسجيل الدخول" : "🔐  Login";
    public static string Connecting => L == "ar" ? "جاري الاتصال..." : "Connecting...";
    public static string FillRequired => L == "ar" ? "يرجى تعبئة جميع الحقول المطلوبة" : "Please fill all required fields";
    public static string LoginFailed => L == "ar" ? "فشل تسجيل الدخول. تحقق من البيانات ورابط السيرفر" : "Login failed. Check your credentials and server URL.";
    public static string Error => L == "ar" ? "خطأ:" : "Error:";

    public static string ScanBarcode => L == "ar" ? "مسح الباركود" : "Scan Barcode";
    public static string BarcodePlaceholder => L == "ar" ? "أدخل الباركود أو امسح" : "Enter barcode or scan";
    public static string Lookup => L == "ar" ? "🔍 بحث" : "🔍 Lookup";
    public static string CameraScan => L == "ar" ? "📷 مسح بالكاميرا" : "📷 Scan with Camera";
    public static string NoItems => L == "ar" ? "لا توجد أصناف" : "No items loaded";
    public static string CloseSession => L == "ar" ? "🔒 إغلاق الجلسة" : "🔒 Close";
    public static string Closed => L == "ar" ? "✅ مغلق" : "✅ Closed";
    public static string ItemsCounted => L == "ar" ? "صنف تم جردها" : "items counted";
    public static string Total => L == "ar" ? "المجموع" : "total";
    public static string Expected => L == "ar" ? "المتوقع:" : "Expected:";
    public static string CloseConfirmTitle => L == "ar" ? "إغلاق الجلسة" : "Close Session";
    public static string CloseConfirmMessage => L == "ar" ? "هل أنت متأكد؟ سيتم تحديث الكميات في المخزون." : "Are you sure? This will update stock quantities.";
    public static string CloseConfirmYes => L == "ar" ? "نعم، أغلق" : "Yes, Close";
    public static string CloseConfirmCancel => L == "ar" ? "إلغاء" : "Cancel";
    public static string ClosedDone => L == "ar" ? "تم الإغلاق و تحديث المخزون" : "Session closed and stock updated";
    public static string CloseError => L == "ar" ? "فشل إغلاق الجلسة" : "Failed to close session";
    public static string EditQuantity => L == "ar" ? "تعديل الكمية" : "Edit Quantity";
    public static string EnterQuantity => L == "ar" ? "أدخل الكمية لـ" : "Enter quantity for";
    public static string Ok => L == "ar" ? "موافق" : "OK";
    public static string NotFound => L == "ar" ? "غير موجود" : "Not Found";
    public static string NotFoundMessage => L == "ar" ? "الباركود '{0}' غير موجود في هذه الجلسة" : "Barcode '{0}' not found in this session";

    public static string Settings => L == "ar" ? "الإعدادات" : "Settings";
    public static string Language => L == "ar" ? "اللغة" : "Language";
    public static string Arabic => L == "ar" ? "العربية" : "Arabic";
    public static string English => L == "ar" ? "English" : "English";
    public static string Theme => L == "ar" ? "الوضع" : "Theme";
    public static string Light => L == "ar" ? "فاتح" : "Light";
    public static string Dark => L == "ar" ? "داكن" : "Dark";
    public static string Save => L == "ar" ? "حفظ" : "Save";
    public static string Logout => L == "ar" ? "تسجيل خروج" : "Logout";
    public static string LogoutConfirm => L == "ar" ? "هل تريد تسجيل الخروج؟" : "Are you sure you want to logout?";

    public static string SessionCount => L == "ar" ? "عدد الجرد" : "Inventory Count";
    public static string ItemsCountedFormat => L == "ar" ? "{0} / {1} صنف تم جردها  ({2} إجمالي)" : "{0} / {1} items counted  ({2} total)";
}
