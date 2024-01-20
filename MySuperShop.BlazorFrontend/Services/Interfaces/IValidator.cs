namespace MySuperShop.BlazorFrontend.Services.Interfaces
{
    public interface IValidator
    {
        void Validate<T>(object value);
    }
}
