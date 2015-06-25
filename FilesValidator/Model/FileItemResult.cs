using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesValidator.Model
{
    class FileItemResult : IFileItemResult
    {
        public string CheckResult { get; set; }

        public string DestinationFolder { get; set; }

        public bool DestinationFolderValid { get; set; } = false;

        public string ResultFile { get; set; }

        public string ResultFileMD5 { get; set; }

        public bool ResultFileMD5Valid { get; set; } = false;

        public bool ResultFileValid { get; set; } = false;

        public string SourceFile { get; set; }

        public string FullFilename { get; set; }

        public FileItemResult()
        {
        }

        public FileItemResult(IFileItem item)
        {
            DestinationFolder = item.DestinationFolder;
            ResultFile = item.ResultFile;
            ResultFileMD5 = item.ResultFileMD5;
            SourceFile = item.SourceFile;
        }
    }
}
