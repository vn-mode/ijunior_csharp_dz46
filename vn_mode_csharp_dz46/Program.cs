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

        Customer customer1 = new Customer(5);
        customer1.AddProductToBasket(supermarket.GetProduct(0));
        customer1.AddProductToBasket(supermarket.GetProduct(2));

        supermarket.AddCustomerToQueue(customer1);

        Customer customer2 = new Customer(3);
        customer2.AddProductToBasket(supermarket.GetProduct(1));
        customer2.AddProductToBasket(supermarket.GetProduct(2));

        supermarket.AddCustomerToQueue(customer2);

        supermarket.ProcessCustomers();
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
    private List<Product> basket = new List<Product>();
    public decimal Money { get; private set; }

    public Customer(decimal money)
    {
        Money = money;
    }

    public decimal BasketTotal
    {
        get { return basket.Sum(product => product.Price); }
    }

    public void AddProductToBasket(Product product)
    {
        basket.Add(product);
        Console.WriteLine($"Покупатель добавил в корзину {product.Name}.");
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
        var removedProduct = basket.ElementAt(new Random().Next(basket.Count));
        Console.WriteLine($"Покупатель не может оплатить все товары. Удаляем {removedProduct.Name} из корзины.");
        basket.Remove(removedProduct);
    }

    private void PerformPayment()
    {
        Console.WriteLine($"Покупатель оплатил {BasketTotal:C2} и уходит с {(Money - BasketTotal):C2} в кармане.");
        Money -= BasketTotal;
        basket.Clear();
    }
}

public class Supermarket
{
    private List<Product> products = new List<Product>();
    private List<Customer> queue = new List<Customer>();

    public void AddProduct(string name, decimal price)
    {
        products.Add(new Product(name, price));
    }

    public Product GetProduct(int index)
    {
        return products[index];
    }

    public void AddCustomerToQueue(Customer customer)
    {
        queue.Add(customer);
        Console.WriteLine($"Покупатель встал в очередь. У него {customer.BasketTotal:C2} товаров на {customer.Money:C2}.");
    }

    public void ProcessCustomers()
    {
        foreach (var customer in queue)
        {
            customer.Pay();
        }

        Console.WriteLine("Все покупатели в очереди были обслужены.");
        queue.Clear();
    }
}
