using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace InventoryRecordsSystem
{
    // (a) Immutable Inventory Record
    public record InventoryItem(
        int Id,
        string Name,
        int Quantity,
        DateTime DateAdded) : IInventoryEntity;

    // (b) Marker Interface
    public interface IInventoryEntity
    {
        int Id { get; }
    }

    // (c) Generic Inventory Logger
    public class InventoryLogger<T>
        where T : IInventoryEntity
    {
        private List<T> _log = new List<T>();
        private string _filePath;

        public InventoryLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Add(T item)
        {
            _log.Add(item);
        }

        public List<T> GetAll()
        {
            return new List<T>(_log);
        }

        public void SaveToFile()
        {
            try
            {
                string json = JsonSerializer.Serialize(
                    _log,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                using (StreamWriter writer =
                    new StreamWriter(_filePath))
                {
                    writer.Write(json);
                }

                Console.WriteLine(
                    "Inventory data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error saving inventory data: {ex.Message}");
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine(
                        "Inventory file was not found.");
                    return;
                }

                using (StreamReader reader =
                    new StreamReader(_filePath))
                {
                    string json = reader.ReadToEnd();

                    List<T>? loadedItems =
                        JsonSerializer.Deserialize<List<T>>(json);

                    if (loadedItems != null)
                    {
                        _log = loadedItems;
                    }
                }

                Console.WriteLine(
                    "Inventory data loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error loading inventory data: {ex.Message}");
            }
        }

        public void Clear()
        {
            _log.Clear();
        }
    }

    // (f) Integration Layer
    public class InventoryApp
    {
        private InventoryLogger<InventoryItem> _logger;

        public InventoryApp()
        {
            _logger = new InventoryLogger<InventoryItem>(
                "inventory.json");
        }

        public void SeedSampleData()
        {
            _logger.Add(
                new InventoryItem(
                    1,
                    "Laptop",
                    10,
                    DateTime.Now));

            _logger.Add(
                new InventoryItem(
                    2,
                    "Keyboard",
                    25,
                    DateTime.Now));

            _logger.Add(
                new InventoryItem(
                    3,
                    "Mouse",
                    30,
                    DateTime.Now));

            _logger.Add(
                new InventoryItem(
                    4,
                    "Monitor",
                    15,
                    DateTime.Now));
        }

        public void SaveData()
        {
            _logger.SaveToFile();
        }

        public void LoadData()
        {
            _logger.LoadFromFile();
        }

        public void ClearMemory()
        {
            _logger.Clear();
        }

        public void PrintAllItems()
        {
            Console.WriteLine(
                "===== INVENTORY ITEMS =====");

            foreach (InventoryItem item in _logger.GetAll())
            {
                Console.WriteLine(
                    $"ID: {item.Id} | " +
                    $"Name: {item.Name} | " +
                    $"Quantity: {item.Quantity} | " +
                    $"Date Added: {item.DateAdded}");
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            InventoryApp app =
                new InventoryApp();

            Console.WriteLine(
                "===== SEEDING INVENTORY =====");

            app.SeedSampleData();

            Console.WriteLine();

            Console.WriteLine(
                "===== SAVING DATA =====");

            app.SaveData();

            Console.WriteLine();

            Console.WriteLine(
                "===== NEW SESSION =====");

            app.ClearMemory();

            app.LoadData();

            Console.WriteLine();

            app.PrintAllItems();
        }
    }
}
