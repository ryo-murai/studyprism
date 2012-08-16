using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using WindowsExplorerish.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace WindowsExplorerish.ViewModels
{
    public class FileListViewModel : NotificationObject, INavigationAware, IConfirmNavigationRequest
    {
        private string path = null;

        private FileViewModel selectedItem;

        private readonly InteractionRequest<Confirmation> confirmNavigationInteractionRequest =
            new InteractionRequest<Confirmation>();
        
        public FileListViewModel()
        {
            // constructing commands

            // Open
            this.OpenCommand = new DelegateCommand<object>((o) => { }, (o) => { return true; });
            HomeCommands.Open.RegisterCommand(this.OpenCommand);
        }

        public string Path
        {
            get
            {
                return this.path;
            }

            set
            {
                if (this.path != value)
                {
                    this.path = value;

                    // raise property changed notifications
                    this.RaisePropertyChanged(() => this.Files);
                    this.RaisePropertyChanged(() => this.ContentTitle);
                }
            }
        }

        public IInteractionRequest ConfirmNavigationInteractionRequest
        {
            get
            {
                return this.confirmNavigationInteractionRequest;
            }
        }

        public IEnumerable<FileViewModel> Files
        {
            get
            {
                try
                {
                    return Directory
                            .EnumerateFiles(this.Path)
                            .Select(s => new FileViewModel(s));
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public FileViewModel SelectedItem
        {
            get
            {
                return this.selectedItem;
            }

            set
            {
                this.selectedItem = value;
                this.RaisePropertyChanged(() => this.SelectedItem);
                this.OpenCommand.RaiseCanExecuteChanged();
            }
        }

        public string ContentTitle
        {
            get
            {
                if (this.Path == null)
                {
                    return null;
                } 
                
                return new DirectoryInfo(this.Path).Name;
            }
        }

        public DelegateCommand<object> OpenCommand { get; private set; }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var targetPath = navigationContext.Parameters["path"];
            return this.Path == targetPath;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // do nothing
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (this.Path == null)
            {
                var newPath = navigationContext.Parameters["path"];

                this.Path = newPath ?? Directory.GetCurrentDirectory();
            }
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            if (true)
            {
                this.confirmNavigationInteractionRequest.Raise(
                            new Confirmation() { Title = "Confirm", Content = "Navigationがロックされていますが、強制的に開きますか？" },
                            c => continuationCallback(c.Confirmed));
            }
            else
            {
                continuationCallback(true);
            }
        }

        #region inner classes
        public class FileViewModel
        {
            private readonly FileInfo fileInfo;

            public FileViewModel(string path)
            {
                this.fileInfo = new FileInfo(path);
            }

            public ImageSource Icon
            {
                get
                {
                    var icon = System.Drawing.Icon.ExtractAssociatedIcon(this.fileInfo.FullName);
                    var stream = new MemoryStream();
                    icon.Save(stream);

                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();

                    return bitmapImage;
                }
            }

            public string Name 
            { 
                get
                {
                    return this.fileInfo.Name;
                }
            }

            public string LastUpdateDate
            { 
                get
                {
                    return this.fileInfo.LastWriteTime.ToShortDateString();
                }
            }

            public string Type
            { 
                get
                {
                    return this.fileInfo.Extension;
                }
            }
            public string Size
            {
                get
                {
                    return (this.fileInfo.Length / 1000).ToString("#,0K");
                }
            }
        }
        #endregion
    }
}
