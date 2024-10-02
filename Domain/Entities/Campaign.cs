using Domain.Common;
using System;

namespace Domain.Entities
{
    public class Campaign : BaseEntity
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        #endregion

        #region Constructors

        // Parameterless constructor for EF Core.
        private Campaign() { }

        public Campaign(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
        }

        #endregion
    }
}
