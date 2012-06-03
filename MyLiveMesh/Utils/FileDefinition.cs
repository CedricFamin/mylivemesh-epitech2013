using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLiveMesh.Utils
{
    public class FileDefinition
    {
        public byte[] RawData { get; set; }
        public string Filename { get; set; }
        public string FileUri { get; set; }
        public string MimeType { get; set; }
    }
}