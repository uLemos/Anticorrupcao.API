namespace Anticorrupção.API.Models
{
    public class CreateRepositoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Private { get; set; }
    }
}
