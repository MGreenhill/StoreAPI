using Microsoft.EntityFrameworkCore;
using StoreSales.API.Entities;

namespace StoreSales.API.DbContexts
{
    public class StoreSalesContext : DbContext
    {
        public DbSet<Person> Persons { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Inventory> Inventory { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;

        public StoreSalesContext(DbContextOptions<StoreSalesContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new Person("Bob", "Smith")
                {
                    Id = 1
                },
                new Person("Mary", "Doe")
                {
                    Id = 2
                },
                new Person("Jim", "Bo")
                {
                    Id = 3
                });
            modelBuilder.Entity<Item>().HasData(
                new Item("Chips")
                {
                    Id = 1,
                    Type = "Food",
                    Price = 1.50M,
                    Description = "BBQ flavored potato chips."
                },
                new Item("Cup")
                {
                    Id = 2,
                    Type = "Misc",
                    Price = 1.00M,
                    Description = "Colorful cup with a mascot printed on it."
                },
                new Item("Gas")
                {
                    Id = 3,
                    Type = "Fuel",
                    Price = 3.25M,
                    Description = "Unleaded Gasoline"
                });
            modelBuilder.Entity<Inventory>().HasData(
                new Inventory()
                {
                    Id = 1,
                    ItemId = 1,
                    Quantity = 4
                },
                new Inventory()
                {
                    Id = 2,
                    ItemId = 2,
                    Quantity = 3
                },
                new Inventory()
                {
                    Id = 3,
                    ItemId = 3,
                    Quantity = 50
                });
            modelBuilder.Entity<Transaction>().HasData(
                new Transaction()
                {
                    Id = 1,
                    PersonId = 2,
                    TimeOccurred = new DateTime(2023, 5, 10, 12, 0, 0)
                },
                new Transaction()
                {
                    Id = 2,
                    PersonId = 1,
                    TimeOccurred = new DateTime(2023, 5, 11, 15, 30, 15)
                },
                new Transaction()
                {
                    Id = 3,
                    PersonId = 1,
                    TimeOccurred = new DateTime(2023, 5, 13, 9, 15, 0)
                },
                new Transaction()
                {
                    Id = 4,
                    PersonId = 3,
                    TimeOccurred = new DateTime(2023, 5, 18, 20, 30, 30)
                });
            modelBuilder.Entity<Order>().HasData(
                new Order()
                {
                    Id = 1,
                    TransactionId = 1,
                    ItemId = 1,
                    Quantity = 2
                },
                new Order()
                {
                    Id = 2,
                    TransactionId = 1,
                    ItemId = 2,
                    Quantity = 1
                },
                new Order()
                {
                    Id = 3,
                    TransactionId = 2,
                    ItemId = 3,
                    Quantity = 5
                },
                new Order()
                {
                    Id = 4,
                    TransactionId = 3,
                    ItemId = 3,
                    Quantity = 3
                },
                new Order()
                {
                    Id = 5,
                    TransactionId = 3,
                    ItemId = 1,
                    Quantity = 1
                },
                new Order()
                {
                    Id = 6,
                    TransactionId = 4,
                    ItemId = 1,
                    Quantity = 1
                },
                new Order()
                {
                    Id = 7,
                    TransactionId = 4,
                    ItemId = 2,
                    Quantity = 1
                },
                new Order()
                {
                    Id = 8,
                    TransactionId = 4,
                    ItemId = 3,
                    Quantity = 10
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}
