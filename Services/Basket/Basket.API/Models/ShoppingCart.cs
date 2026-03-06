
public class ShoppingCart
{
    public string UserName { get; set; } = default!;
    public List<ShoppingCartItem> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

    // Constructor to initialize the shopping cart with a user name
    public ShoppingCart(string userName)
    {
        UserName = userName;
    }

    // Parameterless constructor for deserialization purposes
    public ShoppingCart() 
    {
            
    }
}

