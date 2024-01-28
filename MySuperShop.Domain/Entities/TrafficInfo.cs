namespace MySuperShop.Domain.Entities;

public class TrafficInfo : IEntity
{
    public Guid Id { get; init; }

    private string _path = null!;

    public string Path
    {
        get => _path;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Path must be not empty", nameof(value));
            _path = value;
        }
    }

    private long _countOfVisits;

    public long CountOfVisits
    {
        get => _countOfVisits;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            _countOfVisits = value;
        }
    }

    public TrafficInfo(string path)
    {
        Path = path;
        CountOfVisits = 1;
    } 
}