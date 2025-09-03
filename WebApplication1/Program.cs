using System.Text.Json.Serialization;
using WebApplication1.Models;
using WebApplication1.Service;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddSingleton<ProductService>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

var productApi = app.MapGroup("/product");
productApi.MapGet("/", (ProductService service) => service.GetAll());
productApi.MapGet("/{id}", (Guid id, ProductService service) =>
     service.GetById(id) is Product product
        ? Results.Ok(product)
        : Results.NotFound());
productApi.MapPost("/", (Product product, ProductService service) => {
    Product productCreated = service.Create(product);
    return Results.Created($"/product/{productCreated.Id}", productCreated);
});
productApi.MapPut("/{id}", (Guid id, Product product, ProductService service) => {
    _ = service.Updated(id, product) ? Results.NoContent() : Results.NotFound();
});

productApi.MapDelete("/{id}", (Guid id, ProductService service) => {
    _ = service.Delete(id) ? Results.NoContent() : Results.NotFound();
});


app.Run();

[JsonSerializable(typeof(Product))]
[JsonSerializable(typeof(IEnumerable<Product>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
