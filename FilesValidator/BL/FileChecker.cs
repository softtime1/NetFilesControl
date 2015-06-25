using FilesValidator.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesValidator.BL
{
    public sealed class FileChecker
    {
        private IEnumerable<IFileItemResult> _nonCheckFiles;

        public FileChecker(IEnumerable<IFileItemResult> nonCheckFiles)
        {
            _nonCheckFiles = nonCheckFiles;
        }

        public IFileItemResult Check(string filePath)
        {
            var selected = _nonCheckFiles.Where(elem =>
                0 == String.Compare(elem.FullFilename, filePath,
                StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (null != selected)
                return null;
            var result = new FileItemResult()
            {
                ResultFile = Path.GetFileName(filePath),
                DestinationFolder = Path.GetDirectoryName(filePath),
                CheckResult = "Not definded file was found"
            };
            return result;
        }
    }
}
