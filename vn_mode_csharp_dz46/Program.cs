using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Supermarket supermarket = new Supermarket();
        supermarket.AddProduct("Яблоко", 1.2m);
        supermarket.AddProduct("Апельсин", 1.1m);
        supermarket.AddProduct("Банан", 0.9m);

        Customer customer1 = new Customer { Money = 5 };
        customer1.AddProductToBasket(supermarket.GetProduct(0));
        customer1.AddProductToBasket(supermarket.GetProduct(2));
        Console.WriteLine("Покупатель 1 добавил в корзину Яблоко и Банан.");

        supermarket.AddCustomerToQueue(customer1);
        Console.WriteLine("Покупатель 1 встал в очередь.");

        Customer customer2 = new Customer { Money = 3 };
        customer2.AddProductToBasket(supermarket.GetProduct(1));
        customer2.AddProductToBasket(supermarket.GetProduct(2));
        Console.WriteLine("Покупатель 2 добавил в корзину Апельсин и Банан.");

        supermarket.AddCustomerToQueue(customer2);
        Console.WriteLine("Покупатель 2 встал в очередь.");

        supermarket.ProcessCustomers();
        Console.WriteLine("Все покупатели в очереди были обслужены.");
    }
}

public class Product
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}

public class Customer
{
    private List<Product> Basket { get; } = new List<Product>();
    public decimal Money { get; set; }

    public decimal BasketTotal
    {
        get { return Basket.Sum(product => product.Price); }
    }

    public void AddProductToBasket(Product product)
    {
        Basket.Add(product);
    }

    public void Pay()
    {
        while (!CanPay())
        {
            RemoveRandomProduct();
        }

        PerformPayment();
    }

    private bool CanPay()
    {
        return BasketTotal <= Money;
    }

    private void RemoveRandomProduct()
    {
        var removedProduct = Basket.ElementAt(new Random().Next(Basket.Count));
        Console.WriteLine($"Покупатель не может оплатить все товары. Удаляем {removedProduct.Name} из корзины.");
        Basket.Remove(removedProduct);
    }

    private void PerformPayment()
    {
        Console.WriteLine($"Покупатель оплатил {BasketTotal:C2} и уходит с {(Money - BasketTotal):C2} в кармане.");
        Money -= BasketTotal;
        Basket.Clear();
    }
}

public class Supermarket
{
    private List<Product> Products { get; } = new List<Product>();
    private List<Customer> Queue { get; } = new List<Customer>();

    public void AddProduct(string name, decimal price)
    {
        Products.Add(new Product(name, price));
    }

    public Product GetProduct(int index)
    {
        return Products[index];
    }

    public void AddCustomerToQueue(Customer customer)
    {
        Queue.Add(customer);
    }

    public void ProcessCustomers()
    {
        foreach (var customer in Queue)
        {
            customer.Pay();
        }
        Queue.Clear();
    }
}
