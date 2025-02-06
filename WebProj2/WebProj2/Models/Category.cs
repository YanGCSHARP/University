namespace WebProj2.Models;

public class Category: BaseDbItem
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<Product> Products { get; set; }
    public Category()
    {
        Products = new List<Product>();
    }
}