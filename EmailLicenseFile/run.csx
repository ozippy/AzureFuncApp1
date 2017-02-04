#r "SendGrid"
 
using SendGrid.Helpers.Mail;
using System.Text.RegularExpressions;


public class Order
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    
    public string OrderId { get; set;}
    public string ProductId { get; set;}
    public string Email { get; set;}
    public decimal Price { get; set; }
}

public static void Run(string myBlob, string filename, Order ordersRow, TraceWriter log, out Mail message)
{
    var email = ordersRow.Email;
    
    log.Info($"Got order from {email}\n License file Name:{filename}");

    message = new Mail();
    var personalization = new Personalization();
    personalization.AddTo(new Email(email));
    message.AddPersonalization(personalization);
    
    Attachment attachment = new Attachment();
    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(myBlob);
    attachment.Content =  System.Convert.ToBase64String(plainTextBytes);
    attachment.Type = "text/plain";
    attachment.Filename = "license.txt";
    attachment.Disposition = "attachment";
    attachment.ContentId = "License File";
    message.AddAttachment(attachment);

    var messageContent = new Content("text/html", "Your license file is attached");
    message.AddContent(messageContent);
    message.Subject = "Thanks for your order";
    message.From = new Email("jeremy@sanitariumdev.onmicrosoft.com");
}