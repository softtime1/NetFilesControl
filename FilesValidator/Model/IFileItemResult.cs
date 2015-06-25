using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesValidator.Model
{
    public interface IFileItemResult
    {
        String DestinationFolder { get; }
        bool DestinationFolderValid { get; }
        String ResultFile { get; }
        bool ResultFileValid { get; }
        String ResultFileMD5 { get; }
        bool ResultFileMD5Valid { get; }
        String SourceFile { get; }
        String CheckResult { get; }
        String FullFilename { get; }
    }
}