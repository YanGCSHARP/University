﻿namespace WebProj2.Models;

public class Product:BaseDbItem 
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
