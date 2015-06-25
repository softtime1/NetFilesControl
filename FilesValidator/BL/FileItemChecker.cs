using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilesValidator.Model;
using System.IO;
using System.Security.Cryptography;

namespace FilesValidator.BL
{
    public sealed class FileItemChecker
    {
        public String Folder { get; private set; }

        public FileItemChecker(String folder)
        {
            Folder = folder;
        }

        public IFileItemResult Check(IFileItem fileItem)
        {
            var result = new FileItemResult(fileItem);
            var destinationFoler = Path.Combine(Folder, fileItem.DestinationFolder);
            result.DestinationFolderValid = Directory.Exists(destinationFoler);
            if (!result.DestinationFolderValid)
            {
                result.CheckResult = String.Format("Destination folder \"{0}\" not found",
                    destinationFoler);
                return result;
            }

            result.FullFilename = Path.Combine(destinationFoler, fileItem.ResultFile);
            result.ResultFileValid = File.Exists(result.FullFilename);
            if (!result.ResultFileValid)
            {
                result.CheckResult = String.Format(
                    "Result file \"{0}\" not found", result.FullFilename);
                return result;
            }

            byte[] fileBytes = null;
            try
            {
                fileBytes = File.ReadAllBytes(result.FullFilename);
            }
            catch (IOException ex)
            {
                result.CheckResult = ex.Message;
                return result;
            }
            var md5Bytes = MD5.Create().ComputeHash(File.ReadAllBytes(result.FullFilename));
            var md5 = new StringBuilder();
            for (int i = 0; i < md5Bytes.Length; i++)
                md5.Append(md5Bytes[i].ToString("x2"));
            result.ResultFileMD5 = md5.ToString();
            var comparer = StringComparer.OrdinalIgnoreCase;
            if (!String.IsNullOrEmpty(fileItem.ResultFileMD5) &&
                0 != comparer.Compare(fileItem.ResultFileMD5, result.ResultFileMD5))
            {
                result.ResultFileMD5Valid = false;
                result.CheckResult = "Hash is wrong";
                return result;
            }
            result.ResultFileMD5Valid = true;
            return result;
        }
    }
}
