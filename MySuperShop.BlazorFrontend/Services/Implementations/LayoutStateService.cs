namespace MySuperShop.BlazorFrontend.Services.Implementations;

public class LayoutStateService
{
    private string _cssClass = "nav-link disabled";

    public string CssClass => _cssClass;

    public event Action? OnChange;

    public void SetCssClass(string cssClass)
    {
        _cssClass = cssClass;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}