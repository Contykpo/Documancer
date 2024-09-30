using System.ComponentModel;

namespace Documancer.Domain.Common.Enums
{
    public enum UploadType : byte
    {
        [Description(@"Products")] Product,
        [Description(@"ProfilePictures")] ProfilePicture,
        [Description(@"Documents")] Document
    }
}