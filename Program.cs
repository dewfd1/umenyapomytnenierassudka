using System;
using static Program;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

class Program
{
    public interface ICRUD
    {
        void Create();
        void Read();
        void Update();
        void Delete();
        void Search(string keyword);
    }

    public class User
    {
        public int ID { get; set; }
        public string Логин { get; set; }
        public string Пароль { get; set; }
        public string Роль { get; set; }
        public string Email { get; internal set; }
        public string Name { get; internal set; }
    }

    public class Employee
    {
        public int ID { get; set; }
        public object Id { get; internal set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public DateTime Дата_рождения { get; set; }
        public string Серия_номер_паспорта { get; set; }
        public string Должность { get; set; }
        public decimal Зарплата { get; set; }
        public int UserID { get; set; }
        public object Name { get; internal set; }
        public object Position { get; internal set; }
    }

    public class Product
    {
        public int ID { get; set; }
        public object Id { get; internal set; }
        public string Название { get; set; }
        public decimal Цена_за_штуку { get; set; }
        public int Количество_на_складе { get; set; }
        public object Name { get; internal set; }
        public object Category { get; internal set; }
        public object Price { get; internal set; }
        public object Quantity { get; internal set; }
    }

    public class SelectedProduct : Product
    {
        public int Выбранное_количество { get; set; }
    }

    public class AccountingRecord
    {
        public int ID { get; set; }
        public string Название { get; set; }
        public decimal Сумма_денег { get; set; }
        public DateTime Дата_записи { get; set; }
        public bool Приход { get; set; }
    }
    public static void Main(string[] args)
    {
        bool isAuthenticated = false;
        string username = string.Empty;

        while (!isAuthenticated)
        {
            Console.WriteLine("Авторизация");
            Console.Write("Логин: ");
            string inputUsername = Console.ReadLine();
            Console.Write("Пароль: ");
            string inputPassword = GetHiddenPassword();

            isAuthenticated = Authenticate(inputUsername, inputPassword, out username);

            if (!isAuthenticated)
            {
                Console.WriteLine("Неправильный логин или пароль");
                Console.WriteLine("Повторите попытку");
            }
        }

        Console.WriteLine($"Вы авторизованы как: {username}");
        Console.WriteLine("Добро пожаловать в информационную систему!");

        Console.ReadKey();
    }

    static string GetHiddenPassword()
    {
        string password = string.Empty;
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else
            {
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, (password.Length - 1));
                    Console.Write("\b \b");
                }
            }
        }
        while (key.Key != ConsoleKey.Enter);

        return password;
    }

    static bool Authenticate(string username, string password, out string authenticatedUsername)
    {
        if (username == "admin" && password == "admin")
        {
            authenticatedUsername = "Администратор";
            return true;
        }
        else if (username == "cashier" && password == "cashier")
        {
            authenticatedUsername = "Кассир";
            return true;
        }
        else if (username == "hrmanager" && password == "hrmanager")
        {
            authenticatedUsername = "Менеджер персонала";
            return true;
        }
        else if (username == "warehousemanager" && password == "warehousemanager")
        {
            authenticatedUsername = "Склад-менеджер";
            return true;
        }
        else if (username == "accountant" && password == "accountant")
        {
            authenticatedUsername = "Бухгалтер";
            return true;
        }

        authenticatedUsername = string.Empty;
        return false;
    }

    public class Administrator
    {
        public object users { get; private set; }

        public void ShowMenu()
    {
        Console.WriteLine("Меню администратора:");
        Console.WriteLine("1. Просмотреть всех пользователей");
        Console.WriteLine("2. Добавить пользователя");
        int choice = Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case 1:
                DisplayAllUsers();
                break;
            case 2:
                AddUser();
                break;
        }
    }
    public void DisplayAllUsers()
    {

    }

    public void AddUser()
    {
        User newUser = new User();
        Console.Write("Введите имя пользователя: ");
        newUser.Name = Console.ReadLine();
        Console.Write("Введите email пользователя: ");
        newUser.Email = Console.ReadLine();
        SaveUsers();
    }
    public void SaveUsers()
    {
        string json = JsonConvert.SerializeObject(users);
        File.WriteAllText("users.json", json);
    }

    public void LoadUsers()
    {
        if (File.Exists("users.json"))
        {
            string json = File.ReadAllText("users.json");
            users = JsonConvert.DeserializeObject<List<User>>(json);
        }
    }
}

    public class Manager
    {
        private List<Employee> employees;


        public void LoadEmployeesFromJson(string jsonFilePath)
        {
            string json = File.ReadAllText(jsonFilePath);
        }


        public void DisplayAllEmployees()
        {
            foreach (var employee in employees)
            {
                Console.WriteLine($"ID: {employee.Id}, Name: {employee.Name}, Position: {employee.Position}");
            }
        }

        public void AddEmployee(Employee employee)
        {
            employees.Add(employee);
        }

        public void UpdateEmployee(int id, Employee updatedEmployee)
        {
            var employee = employees.Find(e => (bool)(e.Id = id));
            if (employee != null)
            {
                employee.Name = updatedEmployee.Name;
                employee.Position = updatedEmployee.Position;
            }
        }

        public void DeleteEmployee(int id)
        {
            var employee = employees.Find(e => (bool)(e.Id = id));
            if (employee != null)
            {
                employees.Remove(employee);
            }
        }

        public void SearchEmployee(string attribute, string value)
        {
            foreach (var employee in employees)
            {
                if (attribute == "ID" && employee.Id.ToString() == value)
                {
                    Console.WriteLine($"ID: {employee.Id}, Name: {employee.Name}, Position: {employee.Position}");
                }
            }
        }

        public void BindEmployeeToUser(int employeeId, int userId)
        {
        }
    }

    public class Consultant
    {
        private List<Product> products;



        public void LoadProductsFromJson(string jsonFilePath)
        {
            string json = File.ReadAllText(jsonFilePath);
            products = JsonSerializer.Deserialize<List<Product>>(json);
        }

        public void DisplayAllProducts()
        {
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Category: {product.Category}, Price: {product.Price}, Quantity: {product.Quantity}");
            }
        }

        public void DisplayProductDetails(int productId)
        {
            var product = products.Find(p => (bool)(p.Id = productId));
            if (product != null)
            {
                Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Category: {product.Category}, Price: {product.Price}, Quantity: {product.Quantity}");
            }
            else
            {
                Console.WriteLine("Product not found");
            }
        }

        public void AddProduct(Product product)
        {
            products.Add(product);
        }

        public void UpdateProduct(int productId, Product updatedProduct)
        {
            var index = products.FindIndex(p => (bool)(p.Id = productId));
            if (index != -1)
            {
                products[index] = updatedProduct;
            }
            else
            {
                Console.WriteLine("Product not found");
            }
        }

        public void DeleteProduct(int productId)
        {
            var product = products.Find(p => p.Id == productId);
            if (product != null)
            {
                products.Remove(product);
            }
            else
            {
                Console.WriteLine("Product not found");
            }
        }

        public List<Product> SearchProducts(string attribute, string value)
        {
            List<Product> searchResult = new List<Product>();
            switch (attribute.ToLower())
            {
                case "id":
                    searchResult = products.FindAll(p => p.Id.ToString().Contains(value));
                    break;
                case "name":
                    searchResult = products.FindAll(p => p.Name.Contains(value));
                    break;
                case "category":
                    searchResult = products.FindAll(p => p.Category.Contains(value));
                    break;
                // Другие атрибуты для поиска
                default:
                    Console.WriteLine("Invalid attribute for search");
                    break;
            }
            return searchResult;
        }
    }

    public class Cashier
    {
        private WarehouseManager warehouseManager;
        private List<Product> selectedProducts;

        public Cashier(WarehouseManager manager)
        {
            warehouseManager = manager;
            selectedProducts = new List<Product>();
        }

        public void OpenCashierMenu()
        {
            Console.WriteLine("Список товаров на складе:");
            foreach (Product product in warehouseManager.GetProducts())
            {
                Console.WriteLine($"ID: {product.Id}, Наименование: {product.Name}, Категория: {product.Category}, Цена: {product.Price}, На складе: {product.Quantity}");
            }

            warehouseManager.SaveProductsToJson("warehouse.json");

            UpdateAccounting();
        }

        private void UpdateAccounting()
        {
        }
    }

    public class Accountant
    {
        class Transaction
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public string Description { get; set; }
            public double Amount { get; set; }
            public DateTime Date { get; set; }
        }

        class Accountants
        {
            private List<Transaction> transactions;

            public Accountants()
            {
                transactions = new List<Transaction>();
            }

            public void ShowMenu()
            {
                foreach (var transaction in transactions)
                {
                    Console.WriteLine($"{transaction.Id}. {transaction.Description} - {transaction.Amount}");
                }


                int selectedId = 1; 
                var selectedTransaction = transactions.Find(t => t.Id == selectedId);
                Console.WriteLine($"ID: {selectedTransaction.Id}");
                Console.WriteLine($"Type: {selectedTransaction.Type}");
                Console.WriteLine($"Description: {selectedTransaction.Description}");
                Console.WriteLine($"Amount: {selectedTransaction.Amount}");
                Console.WriteLine($"Date: {selectedTransaction.Date}");
            }

            public void AddTransaction(Transaction transaction)
            {
                transactions.Add(transaction);
            }

            public void UpdateTransaction(int id, Transaction updatedTransaction)
            {
                var index = transactions.FindIndex(t => t.Id == id);
                if (index != -1)
                {
                    transactions[index] = updatedTransaction;
                }
            }

            public void DeleteTransaction(int id)
            {
                transactions.RemoveAll(t => t.Id == id);
            }

            public void Search(string attribute, string value)
            {

            }


            private void LoadFromJson()
            {
                if (File.Exists("transactions.json"))
                {
                    string json = File.ReadAllText("transactions.json");
                    transactions = JsonSerializer.Deserialize<List<Transaction>>(json);
                }
            }
        }
    }
}

internal class WarehouseManager
{
    internal IEnumerable<Product> GetProducts()
    {
        throw new NotImplementedException();
    }

    internal void SaveProductsToJson(string v)
    {
        throw new NotImplementedException();
    }
}