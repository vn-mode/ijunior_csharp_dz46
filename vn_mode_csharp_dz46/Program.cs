using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Supermarket supermarket = new Supermarket();
        supermarket.Products.Add(new Product { Name = "Яблоко", Price = 1.2m });
        supermarket.Products.Add(new Product { Name = "Апельсин", Price = 1.1m });
        supermarket.Products.Add(new Product { Name = "Банан", Price = 0.9m });

        Customer customer1 = new Customer { Money = 5 };
        customer1.AddProductToBasket(supermarket.Products[0]);
        customer1.AddProductToBasket(supermarket.Products[2]);
        Console.WriteLine("Покупатель 1 добавил в корзину Яблоко и Банан.");

        supermarket.AddCustomerToQueue(customer1);
        Console.WriteLine("Покупатель 1 встал в очередь.");

        Customer customer2 = new Customer { Money = 3 };
        customer2.AddProductToBasket(supermarket.Products[1]);
        customer2.AddProductToBasket(supermarket.Products[2]);
        Console.WriteLine("Покупатель 2 добавил в корзину Апельсин и Банан.");

        supermarket.AddCustomerToQueue(customer2);
        Console.WriteLine("Покупатель 2 встал в очередь.");

        supermarket.ProcessCustomers();
        Console.WriteLine("Все покупатели в очереди были обслужены.");
    }
}

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class Customer
{
    public List<Product> Basket { get; set; } = new List<Product>();
    public decimal Money { get; set; }

    public decimal BasketTotal
    {
        get { return Basket.Sum(product => product.Price); }
    }

    public void AddProductToBasket(Product product)
    {
        Basket.Add(product);
    }

    public bool Pay()
    {
        if (BasketTotal <= Money)
        {
            Console.WriteLine($"Покупатель оплатил {BasketTotal:C2} и уходит с {(Money - BasketTotal):C2} в кармане.");
            Money -= BasketTotal;
            Basket.Clear();
            return true;
        }

        while (Basket.Count > 0 && BasketTotal > Money)
        {
            var removedProduct = Basket.ElementAt(new Random().Next(Basket.Count));
            Console.WriteLine($"Покупатель не может оплатить все товары. Удаляем {removedProduct.Name} из корзины.");
            Basket.Remove(removedProduct);
        }

        if (BasketTotal <= Money)
        {
            Console.WriteLine($"Покупатель оплатил {BasketTotal:C2} и уходит с {(Money - BasketTotal):C2} в кармане.");
            Money -= BasketTotal;
            Basket.Clear();
            return true;
        }

        return false;
    }
}

public class Supermarket
{
    public List<Product> Products { get; set; } = new List<Product>();
    public List<Customer> Queue { get; set; } = new List<Customer>();

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
