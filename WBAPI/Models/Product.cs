namespace WBAPI.Models;
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Product()
    {
        Id = default;
        Name = String.Empty;
        Price = default;
    }

    public Product(int id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}

