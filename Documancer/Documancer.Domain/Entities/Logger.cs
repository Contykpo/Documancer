using Documancer.Domain.Common.Entities;

namespace Documancer.Domain.Entities
{
    public class Logger : IEntity<int>
    {
        public string? Message { get; set; }
        public string? MessageTemplate { get; set; }
        public string Level { get; set; } = default!;

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public string? Exception { get; set; }
        public string? UserName { get; set; }
        public string? ClientIP { get; set; }
        public string? ClientAgent { get; set; }
        public string? Properties { get; set; }
        public string? LogEvent { get; set; }
        public int Id { get; set; }
    }
}