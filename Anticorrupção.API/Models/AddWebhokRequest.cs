namespace Anticorrupção.API.Models
{
    public class AddWebhookRequest
    {
        public string WebhookUrl { get; set; }
        public List<string> Events { get; set; }
    }
}
