namespace Documancer.Application.Common.Models
{
    public class UploadRequest
    {
        #region Properties

        public string FileName { get; set; }
        public string? Extension { get; set; }
        public UploadType UploadType { get; set; }
        public bool Overwrite { get; set; }
        public byte[] Data { get; set; }

        #endregion

        #region Constructors

        public UploadRequest(string fileName, UploadType uploadType, byte[] data, bool overwrite = false)
        {
            FileName = fileName;
            UploadType = uploadType;
            Data = data;
            Overwrite = overwrite;
        }

        #endregion
    }
}