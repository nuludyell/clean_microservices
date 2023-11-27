namespace Basket.Application.Responses;

public class ShoppingCartResponse
{
    public string Username { get; set; }
    public List<ShoppingCartItemResponse> Items { get; set; } = new List<ShoppingCartItemResponse>();

    public ShoppingCartResponse()
    {
        
    }

    public ShoppingCartResponse(string username)
    {
        Username = username;
    }

    public double TotalPrice => Items.Sum(i => i.Price * i.Quantity);
}
