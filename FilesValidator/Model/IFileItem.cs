using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesValidator.Model
{
    public interface IFileItem
    {
        String DestinationFolder { get; }
        String ResultFile { get; }
        String ResultFileMD5 { get; }
        String SourceFile { get; }
    }
}
