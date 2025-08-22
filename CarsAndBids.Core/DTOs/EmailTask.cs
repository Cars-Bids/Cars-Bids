namespace CarsAndBids.Core.Models;

public class EmailTask
{
    public string MailTo { get; set; }
    public string Subject { get; set; }
    public string HtmlBody { get; set; }
    public string FromName { get; set; }
    public string FromEmail { get; set; }
}