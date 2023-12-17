using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Backend
{
    public class MyDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Account> Accounts { get; set; }
        private readonly string _connectionString = new SqliteConnectionStringBuilder()
        {
            Mode = SqliteOpenMode.ReadWriteCreate,
            DataSource = "WebApp.db"
        }.ToString();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
    }
}
