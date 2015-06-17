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

        public String Check(IFileItem fileItem)
        {
            var destinationFoler = Path.Combine(Folder, fileItem.DestinationFolder);
            if(!Directory.Exists(destinationFoler))
                return String.Format("Destination folder \"{0}\" not found",
                destinationFoler);

            var resultFile = Path.Combine(destinationFoler, fileItem.ResultFile);
            if (!File.Exists(resultFile))
                return String.Format("Result file \"{0}\" not found", resultFile);

            if (String.IsNullOrEmpty(fileItem.ResultFileMD5))
                return null;
            byte[] fileBytes = null;
            try
            {
                fileBytes = File.ReadAllBytes(resultFile);
            }
            catch (IOException ex)
            {
                return ex.Message;
            }
            var md5Bytes = MD5.Create().ComputeHash(File.ReadAllBytes(resultFile));
            var md5 = new StringBuilder();
            for (int i = 0; i < md5Bytes.Length; i++)
                md5.Append(md5Bytes[i].ToString("x2"));
            var comparer = StringComparer.OrdinalIgnoreCase;
            if (0 != comparer.Compare(fileItem.ResultFileMD5, md5.ToString()))
                return "Hash is wrong";
            return null;
        }
    }
}
