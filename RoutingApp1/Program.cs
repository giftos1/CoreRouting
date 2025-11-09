using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//builder.Services.AddHealthChecks(); // Add services required by the health-check middleware
//builder.Services.AddRazorPages(); // Add services required by Razor Pages

app.MapGet("/", () => "Hello World!");
//app.MapHealthChecks("/health"); // Register a Health-check endpoint
//app.MapRazorPages(); // Register All the Razor Pages as endpoints


app.MapGet("/product/{name}", (string name) =>
{
    return $"The product is {name}"; //❶
}).WithName("product"); // ❷

app.MapGet("/links", (LinkGenerator links) => //❸
{
    string? link = links.GetPathByName("product", new { name = "big-widget" }); //❹
    // links.GetUriByName("product", new { name = "my-product" }, "https", new HostString("localhost"));
    return $"View the product at {link}"; // ❺
});

app.Run();

/*
❶ The endpoint echoes the name it receives in the route template.
❷ Gives the endpoint a name by adding metadata to it
❸ References the LinkGenerator class in the endpoint handler
❹ Creates a link using the route name “product” and provides a value for the route
parameter
❺ Returns the value “View the product at /product/big-widget”


The WithName() method adds metadata to your endpoints so that they can be referenced by other parts of your application. In this case, 
we’re adding a name to the endpoint so we can refer to it later

The LinkGenerator is a service available anywhere in ASP.NET Core. You can access it from your endpoints by including it 
as a parameter in the handler. It's registered with the dependency injection system by default.

Endpoint names are case-sensitive (unlike the route templates themselves) and must be globally unique within the application.

The GetPathByName() method takes the name of a route and, optionally, route data. The route data is packaged as
key-value pairs into a single C# anonymous object. If you need to pass more than one route value, you can add more
properties to the anonymous object. Then the helper will generate a path based on the referenced endpoint’s route template.

Warning: Be careful when using the GetUriByName() method. It's possible to expose vulnerabilities in your app if you use unvalidated host values.
*/