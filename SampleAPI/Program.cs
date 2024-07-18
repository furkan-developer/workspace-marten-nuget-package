using System.Formats.Asn1;
using Marten;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMarten(options =>{
    options.Connection(builder.Configuration.GetConnectionString("PostgreSQL")!);
});

var app = builder.Build();

app.MapGet("/", async (IQuerySession querySession) => {
    
    var products =   await querySession.Query<Product>().ToListAsync();
    
    return Results.Ok(products);

});

app.MapGet("/create-product", async (IDocumentSession documentSession) => {

    var product = new Product("Product-1",10);
    documentSession.Store(product);
    await documentSession.SaveChangesAsync();
    
    return Results.Ok(new {id = product.Id});
});

app.Run();


public record Product(string ProductName, int quantity){
    public Guid Id { get; set; }
}