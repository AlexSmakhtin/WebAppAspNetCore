namespace MySuperShop.Domain.Entities
{
    public class Product: IEntity
    {
        public Guid Id { get; init; }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty", nameof(value));
                _name = value;
            }
        }

        private decimal _price;

        public decimal Price
        {
            get => _price;
            set
            {
                if(value<0)
                    throw new ArgumentException("Price cannot be less then 0", nameof(value));
                _price = value;
            }
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Product()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Product(string name, decimal price)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Name = name;
            Price = price;
        }
    }
}