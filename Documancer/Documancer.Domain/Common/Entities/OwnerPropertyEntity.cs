using System.ComponentModel.DataAnnotations.Schema;
using Documancer.Domain.Identity;

namespace Documancer.Domain.Common.Entities
{
    public abstract class OwnerPropertyEntity : BaseAuditableEntity
    {
        [ForeignKey("CreatedBy")]
        public virtual ApplicationUser? Owner { get; set; }

        [ForeignKey("LastModifiedBy")]
        public virtual ApplicationUser? LastModifier { get; set; }
    }
}