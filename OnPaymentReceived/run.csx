#r "Newtonsoft.Json"

using System;
using System.Net;
using Newtonsoft.Json;

public class Order
{
    public string PartitionKey {get; set;}
    public string RowKey {get; set;}
    public string OrderId {get; set;}
    public string ProductId {get; set;}
    public string Email {get; set;}
    public decimal Price {get; set;}
}

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log, IAsyncCollector<Order> outputQueueItem, IAsyncCollector<Order> outputTable)
{
    log.Info($"Order recieved");

    string jsonContent = await req.Content.ReadAsStringAsync();
    var order = JsonConvert.DeserializeObject<Order>(jsonContent);

    log.Info($"Order {order.OrderId} recieved from {order.Email} for product {order.ProductId}");

    order.PartitionKey = "Orders";
    order.RowKey = order.OrderId;

    await outputTable.AddAsync(order);
    await outputQueueItem.AddAsync(order);

    return req.CreateResponse(HttpStatusCode.OK, new {
        message = $"Thank you for your order!"
    });
}
