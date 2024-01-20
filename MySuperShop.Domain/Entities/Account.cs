using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MySuperShop.Domain.Services;

namespace MySuperShop.Domain.Entities
{
    public class Account : IEntity
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        private Guid _id;

        public Guid Id
        {
            get => _id;
            init => _id = value;
        }

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
                if (value.Length < 2 || value.Length > 100)
                    throw new ArgumentException("Name is not valid", nameof(value));
                _name = value;
            }
        }

        private static readonly EmailAddressAttribute EmailAddressAttribute = new();

        private string _email;

        public string EmailAddress
        {
            get => _email;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email cannot be empty", nameof(value));
                if (!EmailAddressAttribute.IsValid(value))
                    throw new ArgumentException("Email is not valid", nameof(value));
                _email = value;
            }
        }

        private string _hashedPassword;

        public string HashedPassword
        {
            get => _hashedPassword;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Password cannot be empty", nameof(value));
                _hashedPassword = value;
            }
        }

        //For EF Core
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Account()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Account(string name, string email, string hashedPassword)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Name = name;
            EmailAddress = email;
            HashedPassword = hashedPassword;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonConstructor]
        private Account(Guid id, string name, string emailAddress, string hashedPassword)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _id = id;
            Name = name;
            EmailAddress = emailAddress;
            HashedPassword = hashedPassword;
        }
    }
}