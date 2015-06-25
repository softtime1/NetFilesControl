using FilesValidator.BL;
using FilesValidator.Model;
using FilesValidator.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace FilesValidator.VM
{
    public class MainModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// NotifyProperty Helper functions
        /// </summary>
        /// <param name="caller">property change caller</param>
        private void OnPropertyChanged([CallerMemberName]string caller = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }
        }

        private String _checkFolder;

        public String CheckFolder
        {
            get { return _checkFolder; }
            set
            {
                _checkFolder = value;
                Settings.Default.CheckFolder = _checkFolder;
                Settings.Default.Save();
                OnPropertyChanged();
                CheckStartStopCommandAvailability();
            }
        }

        private String _checklistFile;

        public String ChecklistFile
        {
            get { return _checklistFile; }
            set
            {
                _checklistFile = value;
                Settings.Default.ChecklistFile = _checklistFile;
                Settings.Default.Save();
                OnPropertyChanged();
                CheckStartStopCommandAvailability();
            }
        }

        private void CheckStartStopCommandAvailability()
        {
            StartStopCommand.CanExecute = false;
            if (String.IsNullOrEmpty(ChecklistFile))
                return;
            if (String.IsNullOrEmpty(CheckFolder))
                return;
            StartStopCommand.CanExecute = true;
        }

        private Command _startStopCommand;

        public Command StartStopCommand
        {
            get { return _startStopCommand; }
            set { _startStopCommand = value; }
        }

        private ObservableCollection<IFileItemResult> _fileItemsResults =
            new ObservableCollection<IFileItemResult>();

        public ObservableCollection<IFileItemResult> FileItemsResults
        {
            get { return _fileItemsResults; }
            set { _fileItemsResults = value; }
        }

        private bool _isProcessing;

        public bool IsProcessing
        {
            get { return _isProcessing; }
            set
            {
                _isProcessing = value;
                OnPropertyChanged();
            }
        }

        private bool _isValid = false;

        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                _isValid = value;
                OnPropertyChanged();
            }
        }


        public MainModel()
        {
            StartStopCommand = new Command(StartStopCommandExecute, false);
            _checkFolder = Settings.Default.CheckFolder;
            _checklistFile = Settings.Default.ChecklistFile;
            CheckStartStopCommandAvailability();
        }

        public MainModel(Dispatcher dispatcher)
            : this()
        {
            _dispatcher = dispatcher;
        }

        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private Dispatcher _dispatcher;

        private async void StartStopCommandExecute()
        {
            if (IsProcessing)
            {
                _cancellationToken.Cancel(false);
                return;
            }
            _cancellationToken.Dispose();
            _cancellationToken = new CancellationTokenSource();
            FileItemsResults.Clear();
            IsProcessing = true;
            var token = _cancellationToken.Token;
            await Task.Run(() =>
            {
                IsValid = true;
                var checker = new FileItemChecker(CheckFolder);
                using (var reader = new ExcelReader(ChecklistFile))
                {
                    var item = reader.ReadFileItem();
                    while (null != item && !token.IsCancellationRequested)
                    {
                        var result = checker.Check(item);
                        if (!String.IsNullOrEmpty(result.CheckResult))
                            IsValid = false;
                        _dispatcher.BeginInvoke(
                            new Action(() => { FileItemsResults.Add(result); }));
                        item = reader.ReadFileItem();
                    }
                }
                var fileChecker = new FileChecker(FileItemsResults);
                var files = GetAllFiles(CheckFolder, token);
                foreach(var file in files)
                {
                    if (token.IsCancellationRequested)
                        return;
                    var result = fileChecker.Check(file);
                    if (null == result)
                        continue;
                    IsValid = false;
                    _dispatcher.BeginInvoke(
                        new Action(() => { FileItemsResults.Add(result); }));
                }
            }, token);
            IsProcessing = false;
        }

        private IEnumerable<String> GetAllFiles(
            string checkFolder, CancellationToken token)
        {
            var listFiles = new List<String>();

            var di = new DirectoryInfo(checkFolder);
            var dirs = di.GetDirectories();
            foreach (var dir in dirs)
            {
                if (token.IsCancellationRequested)
                    return null;
                var res = GetAllFiles(dir.FullName, token);
                listFiles.AddRange(res);
            }
            var files = di.GetFiles();
            foreach (var file in files)
            {
                if (token.IsCancellationRequested)
                    return null;
                listFiles.Add(file.FullName);
            }
            return listFiles;
        }

    }
}
