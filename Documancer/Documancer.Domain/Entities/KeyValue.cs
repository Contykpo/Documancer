using System.ComponentModel;
using Documancer.Domain.Common.Entities;

namespace Documancer.Domain.Entities
{
    public class KeyValue : BaseAuditableEntity, IAuditTrial
    {
        public Picklist Name { get; set; } = Picklist.Brand;
        public string? Value { get; set; }
        public string? Text { get; set; }
        public string? Description { get; set; }
    }

    public enum Picklist
    {
        [Description("Status")] Status,
        [Description("Unit")] Unit,
        [Description("Brand")] Brand
    }
}