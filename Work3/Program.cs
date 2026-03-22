using Library.EfCore;

using var context = new ShopContext();


//Task 1.1
var newProduct1 = new Product { Name = "Laptop", Price = 1500m, Category = "Electronics", InStock = true };
var newProduct2 = new Product { Name = "Phone", Price = 800m, Category = "Electronics", InStock = true };
var newProduct3 = new Product { Name = "Desk", Price = 350m, Category = "Furniture", InStock = false };
var newProduct4 = new Product { Name = "Headphones", Price = 120m, Category = "Electronics", InStock = true };
var newProduct5 = new Product { Name = "Chair",      Price = 200m,  Category = "Furniture",   InStock = true  };

context.Products.AddRange(newProduct1, newProduct2, newProduct3, newProduct4, newProduct5);
context.SaveChanges();

var newProduct = new Product { Name = "New Product", Price = 1.99m, Category = "Some Category", InStock = true };
context.Products.Add(newProduct);
context.SaveChanges();
Console.WriteLine($"Added: {newProduct.Name}");

var foundProduct = context.Products.Find(newProduct.Id);
if (foundProduct is not null)
    Console.WriteLine($"Found: {foundProduct.Name}");

newProduct.Category = "Not some category";
context.SaveChanges(); // ✅ обов'язково
Console.WriteLine($"New category: {newProduct.Category}");

context.Products.Remove(newProduct);
context.SaveChanges(); // ✅ обов'язково
Console.WriteLine($"Deleted: {newProduct.Name}");

int count = context.Products.Count();
Console.WriteLine($"Count of products: {count}");
