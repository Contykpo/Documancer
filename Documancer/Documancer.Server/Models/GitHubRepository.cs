using System.Text.Json.Serialization;

namespace Documancer.Server.Models
{
    public class GitHubRepository
    {
        [JsonPropertyName("stargazers_count")] public int StargazersCount { get; set; }
    }
}