using FilesValidator.Model;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FilesValidator.BL
{
    public sealed class ExcelReader : IDisposable
    {
        private String FileFullname { get; set; }

        private Application _excelApplication = new Application();

        private Workbook _workBook = null;

        private Worksheet _workSheet = null;

        private object _missingObj = System.Reflection.Missing.Value;

        private int _rowsCount = 0;

        private int _currentRow = 1;

        public ExcelReader(String fileFullname)
        {
            FileFullname = fileFullname;
            if (!File.Exists(FileFullname))
                throw new FileNotFoundException(FileFullname);
            _workBook = _excelApplication.Workbooks.Open(FileFullname);
            _workSheet = _workBook.Sheets[1];
            _rowsCount = _workSheet.Rows.Count;
        }

        public IFileItem ReadFileItem()
        {
            if (_currentRow > _rowsCount)
                return null;
            ++_currentRow;
            var result = new FileItem();
            result.SourceFile = _workSheet.Cells[_currentRow, 1].Value;
            if (String.IsNullOrEmpty(result.SourceFile))
                return null;
            result.DestinationFolder = _workSheet.Cells[_currentRow, 2].Value;
            result.ResultFile = _workSheet.Cells[_currentRow, 3].Value;
            result.ResultFileMD5 = _workSheet.Cells[_currentRow, 4].Value;
            return result;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                _workBook.Close(false, _missingObj, _missingObj);
                _excelApplication.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(
                    _excelApplication);
                _excelApplication = null;
                _workBook = null;
                _workSheet = null;
                System.GC.Collect();
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ExcelReader() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
