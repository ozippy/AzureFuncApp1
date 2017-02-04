#r "Newtonsoft.Json"

using System;
using Newtonsoft.Json;

public class Order
{
    public string OrderId {get; set;}
    public string ProductId {get; set;}
    public string Email {get; set;}
    public decimal Price {get; set;}
}

public static void Run(Order myQueueItem, TraceWriter log, IBinder binder)
{
    log.Info($"Received an order: Order {myQueueItem.OrderId}, Product {myQueueItem.ProductId}, Email {myQueueItem.Email}");
 using (var outputBlob = binder.Bind<TextWriter>(
                  new BlobAttribute($"licenses/{myQueueItem.OrderId}.lic")))
    {
        outputBlob.WriteLine($"OrderId: {myQueueItem.OrderId}");
        outputBlob.WriteLine($"Email: {myQueueItem.Email}");
        outputBlob.WriteLine($"ProductId: {myQueueItem.OrderId}");
        outputBlob.WriteLine($"PurchaseDate: {DateTime.UtcNow}");
        var md5 = System.Security.Cryptography.MD5.Create();
        var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(myQueueItem.Email + "secret"));    
        outputBlob.WriteLine($"SecretCode: {BitConverter.ToString(hash).Replace("-","")}");
    }
}