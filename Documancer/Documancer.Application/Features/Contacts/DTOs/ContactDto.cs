namespace Documancer.Application.Features.Contacts.DTOs
{
    [Description("Contacts")]
    public class ContactDto
    {
        #region Properties and Fields

        [Description("Id")]
        public int Id { get; set; }
        [Description("Name")]
        public string Name { get; set; }
        [Description("Description")]
        public string? Description { get; set; }
        [Description("Email")]
        public string? Email { get; set; }
        [Description("Phone number")]
        public string? PhoneNumber { get; set; }
        [Description("Country")]
        public string? Country { get; set; }

        #endregion

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Contact, ContactDto>().ReverseMap();
            }
        }
    }
}