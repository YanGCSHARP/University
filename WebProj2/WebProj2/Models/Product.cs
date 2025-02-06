namespace WebProj2.Models;

public class Product : BaseDbItem
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Instructions { get; set; }
    public decimal Price { get; set; } = 0;
    public List<string>? Images { get; set; }
    public Category Category { get; set; }
}