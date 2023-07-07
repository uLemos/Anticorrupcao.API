using Anticorrupção.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Anticorrupção.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepositoriesController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public RepositoriesController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRepository([FromBody] CreateRepositoryRequest request)
        {
            var repositoryId = await _gitHubService.CreateRepository(request);
            return Ok(repositoryId);
        }

        [HttpGet("{owner}/{repository}/branches")]
        public async Task<IActionResult> ListBranches(string owner, string repository)
        {
            var branches = await _gitHubService.GetBranches(owner, repository);
            return Ok(branches);
        }

        [HttpGet("{owner}/{repository}/webhooks")]
        public async Task<IActionResult> ListWebhooks(string owner, string repository)
        {
            var webhooks = await _gitHubService.GetWebhooks(owner, repository);
            return Ok(webhooks);
        }

        [HttpPost("{owner}/{repository}/webhooks")]
        public async Task<IActionResult> AddWebhook(string owner, string repository, [FromBody] AddWebhookRequest request)
        {
            var webhookId = await _gitHubService.AddWebhook(owner, repository, request.WebhookUrl, request.Events);
            return Ok(webhookId);
        }

        [HttpPatch("{owner}/{repository}/webhooks/{webhookId}")]
        public async Task<IActionResult> UpdateWebhook(string owner, string repository, int webhookId, [FromBody] UpdateWebhookRequest request)
        {
            var success = await _gitHubService.UpdateWebhook(owner, repository, webhookId, request.NewUrl);
            return Ok(success);
        }
    }
}
