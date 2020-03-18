using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpResponseListener
{
    class PresentationFileDetails
    {
        public int PresentationFileId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int FileSize { get; set; }
        public bool IsHandOutFile { get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsStartupFile { get; set; }
        public bool IsLocked { get; set; }
        public string LockedBy { get; set; }
        public string ClientSessionId { get; set; }
        public string RoomName { get; set; }
        public int SubSessionId { get; set; }
        public DateTime SessionDate { get; set; }
    }

    class ConvertedFileCreateDto
    {
        public int PresentationFileId { get; set; }
        public int SubSessionId { get; set; }
        public int ConvertedFileTypeId { get; set; }
        public string FileName { get; set; }
        public string InternalFilePath { get; set; }
        public string ExternalFilePath { get; set; }
        public bool ConversionError { get; set; }
        public string ErrorText { get; set; }
    }
}
