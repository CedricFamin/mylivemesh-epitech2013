using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Liquid.Components;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace client_mesh.ViewModels
{
    public class FileUploaderViewControl : BindableObject
    {
        Uploader _uploader = new Uploader("http://localhost:1991/FileUploadService.asmx", "Upload");
        ViewModelLocator _locator = new ViewModelLocator();

        public Uploader Uploader { get { return _uploader; } }
        public ObservableCollection<UploaderItem> Items  { get { return _uploader.Items; } }
        public int ItemsTotal { get { return _uploader.ItemsTotal; } }
        public int ItemsUploaded { get { return _uploader.ItemsUploaded; } }

        public FileUploaderViewControl()
        {
            _uploader.UploadedItem += new UploadEventHandler(_uploader_UploadedItem);
            _uploader.UploadProgressChange += new UploadEventHandler(_uploader_UploadProgressChange);
            _uploader.UploadFinished += new UploadEventHandler(_uploader_UploadFinished);
        }

        void _uploader_UploadFinished(object sender, UploadEventArgs e)
        {
            _locator.LoggerViewModel.AddLog("Upload finished");
            RaisePropertyChange("ItemsTotal");
            RaisePropertyChange("ItemsUploaded");
            foreach (FolderDefinition folder in _locator.Folders)
            {
                folder.UpdateFileList.Execute(null);
            }
    }

        void _uploader_UploadProgressChange(object sender, UploadEventArgs e)
        {
            
            //_locator.LoggerViewModel.AddLog("Upload progress change");
        }

        void _uploader_UploadedItem(object sender, UploadEventArgs e)
        {
            _locator.LoggerViewModel.AddLog("Upload new file");
            RaisePropertyChange("ItemsTotal");
            RaisePropertyChange("ItemsUploaded");

        }

        public void AddFiles(IEnumerable<FileInfo> files, string dir)
        {
            _uploader.UploadFiles("upload", files, dir, true, "");
            RaisePropertyChange("ItemsTotal");
            RaisePropertyChange("ItemsUploaded");  
        }
    }
}
