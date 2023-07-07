using Anticorrupção.API.Models;
using Microsoft.Extensions.Options;
using Octokit;
using System.Text;
using System.Text.Json;

namespace Anticorrupção.API.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly HttpClient _httpClient;
        private readonly GitHubTokenOptions _githubTokenOptions;

        public GitHubService(HttpClient httpClient, IOptions<GitHubTokenOptions> options)
        {
            _httpClient = httpClient;
            _githubTokenOptions = options.Value;
        }

        public async Task<long> CreateRepository(CreateRepositoryRequest request)
        {
            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _githubTokenOptions.AccessToken);

            var response = await _httpClient.PostAsync("https://api.github.com/user/repos", content);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var repository = JsonSerializer.Deserialize<GitHubRepository>(jsonResponse);

            return repository.Id;
        }

        public async Task<List<Branch>> GetBranches(string owner, string repository)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _githubTokenOptions.AccessToken);

            var response = await _httpClient.GetAsync($"https://api.github.com/repos/{owner}/{repository}/branches");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var branches = JsonSerializer.Deserialize<List<Branch>>(jsonResponse);

            return branches;
        }

        public async Task<List<Webhook>> GetWebhooks(string owner, string repository)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _githubTokenOptions.AccessToken);

            var response = await _httpClient.GetAsync($"https://api.github.com/repos/{owner}/{repository}/hooks");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var webhooks = JsonSerializer.Deserialize<List<Webhook>>(jsonResponse);

            return webhooks;
        }

        public async Task<long> AddWebhook(string owner, string repository, string webhookUrl, List<string> events)
        {
            var request = new AddWebhookRequest
            {
                WebhookUrl = webhookUrl,
                Events = events
            };

            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _githubTokenOptions.AccessToken);

            var response = await _httpClient.PostAsync($"https://api.github.com/repos/{owner}/{repository}/hooks", content);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var webhook = JsonSerializer.Deserialize<Webhook>(jsonResponse);

            return webhook.Id;
        }

        public async Task<bool> UpdateWebhook(string owner, string repository, int webhookId, string newUrl)
        {
            var request = new UpdateWebhookRequest
            {
                NewUrl = newUrl
            };

            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _githubTokenOptions.AccessToken);

            var response = await _httpClient.PatchAsync($"https://api.github.com/repos/{owner}/{repository}/hooks/{webhookId}", content);

            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }
    }
}

