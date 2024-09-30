namespace Documancer.Application.Common.Security
{
#nullable disable
    /// <summary>
    ///     Specifies the class this attribute is applied to requires authorization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequestAuthorizeAttribute : Attribute
    {
        #region Properties and Fields

        /// <summary>
        ///     Gets or sets a comma delimited list of roles that are allowed to access the resource.
        /// </summary>
        public string Roles { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the policy name that determines access to the resource.
        /// </summary>
        public string Policy { get; set; } = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthorizeAttribute" /> class.
        /// </summary>
        public RequestAuthorizeAttribute()
        {
        }

        #endregion
    }
}