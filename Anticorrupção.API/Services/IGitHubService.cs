using Anticorrupção.API.Models;
using Octokit;

public interface IGitHubService
{
    Task<long> CreateRepository(CreateRepositoryRequest request);
    Task<List<Branch>> GetBranches(string owner, string repository);
    Task<List<Webhook>> GetWebhooks(string owner, string repository);
    Task<long> AddWebhook(string owner, string repository, string webhookUrl, List<string> events);
    Task<bool> UpdateWebhook(string owner, string repository, int webhookId, string newUrl);
}
