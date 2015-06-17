using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FilesValidator.Model
{
    sealed class FileItem : IFileItem
    {
        public String DestinationFolder { get; internal set; }
        public String ResultFile { get; internal set; }
        public String ResultFileMD5 { get; internal set; }
        public String SourceFile { get; internal set; }
    }
}
