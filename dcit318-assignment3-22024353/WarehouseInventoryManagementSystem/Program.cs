using System;
using System.Collections.Generic;

namespace WarehouseInventoryManagement
{
    // (a) Marker Interface
    public interface IInventoryItem
    {
        int Id { get; }
        string Name { get; }
        int Quantity { get; set; }
    }

    // (b) ElectronicItem
    public class ElectronicItem : IInventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Brand { get; set; }
        public int WarrantyMonths { get; set; }

        public ElectronicItem(
            int id,
            string name,
            int quantity,
            string brand,
            int warrantyMonths)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }
    }

    // (c) GroceryItem
    public class GroceryItem : IInventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }

        public GroceryItem(
            int id,
            string name,
            int quantity,
            DateTime expiryDate)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            ExpiryDate = expiryDate;
        }
    }

    // (e) Custom Exceptions
    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string message)
            : base(message)
        {
        }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message)
            : base(message)
        {
        }
    }

    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string message)
            : base(message)
        {
        }
    }

    // (d) Generic Inventory Repository
    public class InventoryRepository<T>
        where T : IInventoryItem
    {
        private Dictionary<int, T> _items =
            new Dictionary<int, T>();

        public void AddItem(T item)
        {
            if (_items.ContainsKey(item.Id))
            {
                throw new DuplicateItemException(
                    $"Item with ID {item.Id} already exists.");
            }

            _items.Add(item.Id, item);
        }

        public T GetItemById(int id)
        {
            if (!_items.ContainsKey(id))
            {
                throw new ItemNotFoundException(
                    $"Item with ID {id} was not found.");
            }

            return _items[id];
        }

        public void RemoveItem(int id)
        {
            if (!_items.ContainsKey(id))
            {
                throw new ItemNotFoundException(
                    $"Item with ID {id} was not found.");
            }

            _items.Remove(id);
        }

        public List<T> GetAllItems()
        {
            return new List<T>(_items.Values);
        }

        public void UpdateQuantity(int id, int newQuantity)
        {
            if (newQuantity < 0)
            {
                throw new InvalidQuantityException(
                    "Quantity cannot be negative.");
            }

            T item = GetItemById(id);
            item.Quantity = newQuantity;
        }
    }

    // (f) WareHouseManager
    public class WareHouseManager
    {
        private InventoryRepository<ElectronicItem> _electronics =
            new InventoryRepository<ElectronicItem>();

        private InventoryRepository<GroceryItem> _groceries =
            new InventoryRepository<GroceryItem>();

        // Properties to access the existing repositories
        public InventoryRepository<ElectronicItem> Electronics
        {
            get { return _electronics; }
        }

        public InventoryRepository<GroceryItem> Groceries
        {
            get { return _groceries; }
        }

        public void SeedData()
        {
            _electronics.AddItem(
                new ElectronicItem(
                    1,
                    "Laptop",
                    10,
                    "HP",
                    24));

            _electronics.AddItem(
                new ElectronicItem(
                    2,
                    "Smartphone",
                    15,
                    "Samsung",
                    12));

            _electronics.AddItem(
                new ElectronicItem(
                    3,
                    "Tablet",
                    8,
                    "Lenovo",
                    18));

            _groceries.AddItem(
                new GroceryItem(
                    101,
                    "Rice",
                    20,
                    new DateTime(2027, 5, 10)));

            _groceries.AddItem(
                new GroceryItem(
                    102,
                    "Milk",
                    12,
                    new DateTime(2026, 12, 15)));

            _groceries.AddItem(
                new GroceryItem(
                    103,
                    "Bread",
                    25,
                    new DateTime(2026, 10, 20)));
        }

        public void PrintAllItems<T>(
            InventoryRepository<T> repo)
            where T : IInventoryItem
        {
            foreach (T item in repo.GetAllItems())
            {
                Console.WriteLine(
                    $"ID: {item.Id} | " +
                    $"Name: {item.Name} | " +
                    $"Quantity: {item.Quantity}");
            }
        }

        public void IncreaseStock<T>(
            InventoryRepository<T> repo,
            int id,
            int quantity)
            where T : IInventoryItem
        {
            try
            {
                T item = repo.GetItemById(id);

                if (quantity < 0)
                {
                    throw new InvalidQuantityException(
                        "Quantity to add cannot be negative.");
                }

                repo.UpdateQuantity(
                    id,
                    item.Quantity + quantity);

                Console.WriteLine(
                    $"Stock increased. New quantity for " +
                    $"{item.Name}: {item.Quantity}");
            }
            catch (ItemNotFoundException ex)
            {
                Console.WriteLine(
                    $"Not Found Error: {ex.Message}");
            }
            catch (InvalidQuantityException ex)
            {
                Console.WriteLine(
                    $"Quantity Error: {ex.Message}");
            }
        }

        public void RemoveItemById<T>(
            InventoryRepository<T> repo,
            int id)
            where T : IInventoryItem
        {
            try
            {
                repo.RemoveItem(id);

                Console.WriteLine(
                    $"Item with ID {id} removed successfully.");
            }
            catch (ItemNotFoundException ex)
            {
                Console.WriteLine(
                    $"Not Found Error: {ex.Message}");
            }
        }

        public void TestDuplicateItem()
        {
            try
            {
                _electronics.AddItem(
                    new ElectronicItem(
                        1,
                        "Duplicate Laptop",
                        5,
                        "Dell",
                        12));
            }
            catch (DuplicateItemException ex)
            {
                Console.WriteLine(
                    $"Duplicate Error: {ex.Message}");
            }
        }

        public void TestInvalidQuantity()
        {
            try
            {
                _electronics.UpdateQuantity(1, -5);
            }
            catch (InvalidQuantityException ex)
            {
                Console.WriteLine(
                    $"Quantity Error: {ex.Message}");
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            WareHouseManager manager =
                new WareHouseManager();

            manager.SeedData();

            Console.WriteLine(
                "===== GROCERY ITEMS =====");

            manager.PrintAllItems(
                manager.Groceries);

            Console.WriteLine();

            Console.WriteLine(
                "===== ELECTRONIC ITEMS =====");

            manager.PrintAllItems(
                manager.Electronics);

            Console.WriteLine();

            Console.WriteLine(
                "===== INCREASE STOCK =====");

            manager.IncreaseStock(
                manager.Electronics,
                1,
                5);

            Console.WriteLine();

            Console.WriteLine(
                "===== DUPLICATE ITEM TEST =====");

            manager.TestDuplicateItem();

            Console.WriteLine();

            Console.WriteLine(
                "===== ITEM NOT FOUND TEST =====");

            manager.RemoveItemById(
                manager.Groceries,
                999);

            Console.WriteLine();

            Console.WriteLine(
                "===== INVALID QUANTITY TEST =====");

            manager.TestInvalidQuantity();
        }
    }
}


