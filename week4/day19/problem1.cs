using System;

class Product
{
    // Private variables (data members)
    private int productId;
    private string productName;
    private double unitPrice;
    private int qty;

    // Constructor (accepts productId)
    public Product(int id)
    {
        productId = id;
    }

    // Readonly Property for ProductId
    public int ProductId
    {
        get { return productId; }
    }

    // Property for ProductName
    public string ProductName
    {
        get { return productName; }
        set { productName = value; }
    }

    // Property for UnitPrice
    public double UnitPrice
    {
        get { return unitPrice; }
        set { unitPrice = value; }
    }

    // Property for Quantity
    public int Quantity
    {
        get { return qty; }
        set { qty = value; }
    }

    // Method to show product details
    public void ShowDetails()
    {
        double totalAmount = unitPrice * qty;

        Console.WriteLine("Product Id : " + ProductId);
        Console.WriteLine("Product Name : " + ProductName);
        Console.WriteLine("Unit Price : " + UnitPrice);
        Console.WriteLine("Quantity : " + Quantity);
        Console.WriteLine("Total Amount : " + totalAmount);
    }
}

class Program
{
    static void Main()
    {
        // Create object
        Product p = new Product(101);

        // Assign values using properties
        p.ProductName = "Laptop";
        p.UnitPrice = 50000;
        p.Quantity = 2;

        // Display details
        p.ShowDetails();

        Console.ReadLine();
    }
}