using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.ServiceLocation;

namespace WindowsExplorerish.ViewModels
{
    public class FolderTreeViewModel
    {
        private readonly IRegionManager regionManager;
        private readonly IEnumerable<TreeNode> drives;

        public FolderTreeViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            this.drives = DriveInfo.GetDrives().Select(d => new DirectoryTreeNode(d.Name));
        }

        public IEnumerable<TreeNode> TreeContent
        {
            get
            {
                return this.drives;
            }
        }

        #region inner classes

        public abstract class TreeNode : NotificationObject
        {
            protected readonly FileSystemInfo fileSystemInfo;
            private bool isSelected = false;

            public TreeNode(FileSystemInfo fileSystemInfo)
            {
                this.fileSystemInfo = fileSystemInfo;
            }

            //public Stream ImageStream
            //{
            //    get
            //    {
            //        var icon = SystemIcons.WinLogo;
            //        var stream = new MemoryStream();
            //        icon.Save(stream);

            //        return stream;
            //    }
            //}

            public string Icon
            {
                get
                {
                    return null;
                }
            }

            //public string FolderIconSource
            //{
            //    get
            //    {
            //        return "/WindowsExplorerishModule;component/Images/Folder.jpg";
            //    }
            //}

            //public ImageSource Icon
            //{
            //    get
            //    {
            //        var icon = SystemIcons.Asterisk;
            //        //var icon = System.Drawing.Icon.ExtractAssociatedIcon(this.fileSystemInfo.FullName);
            //        var stream = new MemoryStream();
            //        icon.Save(stream);

            //        var bI = new BitmapImage();
            //        bI.BeginInit();
            //        bI.StreamSource = stream;
            //        bI.EndInit();

            //        return bI;
            //    }
            //}

            public string Label
            {
                get
                {
                    return this.fileSystemInfo.Name;
                }
            }

            public abstract IEnumerable<TreeNode> Children { get; }

            public bool IsSelected
            {
                get
                {
                    return this.isSelected;
                }

                set
                {
                    if (this.isSelected != value)
                    {
                        this.isSelected = value;

                        if (this.isSelected)
                        {
                            var regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
                            regionManager.RequestNavigate("MainRegion", "FileListView?path=" + this.fileSystemInfo.FullName);
                        }

                        this.RaisePropertyChanged(() => this.IsSelected);
                    }
                }
            }
        }

        class DirectoryTreeNode : TreeNode
        {
            public DirectoryTreeNode(string path) : base(new DirectoryInfo(path)) { }

            public override IEnumerable<TreeNode> Children
            {
                get
                {
                    try
                    {
                        var directories = Directory
                                            .EnumerateDirectories(this.fileSystemInfo.FullName)
                                            .Select(s => new DirectoryTreeNode(s) as TreeNode);

                        return directories;
                        //var files = Directory
                        //                .EnumerateFiles(this.fileSystemInfo.FullName)
                        //                .Select(s => new FileTreeNode(s) as TreeNode);

                        //return directories.Union(files);
                    }
                    catch (Exception)
                    {
                        // folder permission is handled by catching exception
                        return null;
                    }
                }
            }
        }

        class FileTreeNode : TreeNode
        {
            public FileTreeNode(string path) : base(new FileInfo(path)) { }

            public override IEnumerable<TreeNode> Children
            {
                get
                {
                    return null;
                }
            }
        }

        #endregion

    }
}
