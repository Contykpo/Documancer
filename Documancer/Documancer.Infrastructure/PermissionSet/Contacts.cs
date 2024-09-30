using System.ComponentModel;

namespace Documancer.Infrastructure.PermissionSet
{
    public static partial class Permissions
    {

        [DisplayName("Contact Permissions")]
        [Description("Set permissions for contact operations.")]
        public static class Contacts
        {
            public const string View = "Permissions.Contacts.View";
            public const string Create = "Permissions.Contacts.Create";
            public const string Edit = "Permissions.Contacts.Edit";
            public const string Delete = "Permissions.Contacts.Delete";
            public const string Print = "Permissions.Contacts.Print";
            public const string Search = "Permissions.Contacts.Search";
            public const string Export = "Permissions.Contacts.Export";
            public const string Import = "Permissions.Contacts.Import";
        }
    }
}