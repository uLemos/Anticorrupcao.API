namespace Anticorrupção.API.Models
{
    public class Webhook
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Events { get; set; }
    }
}
