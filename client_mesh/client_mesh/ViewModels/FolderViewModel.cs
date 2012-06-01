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
using client_mesh.ServiceReference1;
using System.Collections.ObjectModel;
using client_mesh.Utils;
using Liquid.Components;
using System.IO;

namespace client_mesh.ViewModels
{
    public class FolderDefinition : BindableObject
    {
        #region Ppties
        
        public ICommand UploadFiles { get; private set; }
        public ICommand UpdateFileList { get; private set; }
        public ICommand DownloadFile { get; private set; }
        public string Name { get; private set; }
        public ObservableCollection<FileDefinition> Files { get; private set; }
        FolderManagerClient folderManagerService = new FolderManagerClient();
        ViewModelLocator Locator = new ViewModelLocator();
        #endregion

        #region Ctor
        public FolderDefinition(string name)
        {
            Name = name;
            UploadFiles = new RelayCommand((param) => UploadFilesBody());
            UpdateFileList = new RelayCommand((param) => UpdateFileListBody());
            DownloadFile = new RelayCommand((param) => DownloadFileBody(param as string));
            folderManagerService.FileListCompleted += new EventHandler<FileListCompletedEventArgs>(folderManagerService_FileListCompleted);
            folderManagerService.GetFileFromCompleted +=new EventHandler<GetFileFromCompletedEventArgs>(folderManagerService_GetFileFromCompleted);
            Files = new ObservableCollection<FileDefinition>();
            Locator.Folders.Add(this);
        }

        ~FolderDefinition()
        {
            Locator.Folders.Remove(this);
        }
        #endregion

        #region event
        void folderManagerService_FileListCompleted(object sender, FileListCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result.ErrorCode == WebResult.ErrorCodeList.SUCCESS)
                {
                    var dirs = e.Result.Value;
                    Files.Clear();
                    foreach (FileDefinition folder in e.Result.Value)
                    {
                        Files.Add(folder);
                    }
                    RaisePropertyChange("Files");
                }
                Locator.LoggerViewModel.AddLog("Get files in " + Name + ": " + e.Result.ErrorCode);
            }
        }

        void folderManagerService_GetFileFromCompleted(object sender, GetFileFromCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result.ErrorCode == WebResult.ErrorCodeList.SUCCESS)
                {
                    
                  /*  if (fd.SafeFileName != null)
                    {
                        Stream stream = fd.OpenFile();
                        stream.Write(e.Result.Value, 0, e.Result.Value.Length);
                    }*/
                }
                Locator.LoggerViewModel.AddLog("Download file: " + e.Result.ErrorCode);
            }
        }
        #endregion

        #region Command body
        private void UpdateFileListBody()
        {
            folderManagerService.FileListAsync(Locator.LoginPageViewModel.User.id, Name);
        }

        private void UploadFilesBody()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Multiselect = true;
            fd.ShowDialog();
            Locator.FileUploaderViewModel.AddFiles(fd.Files, Locator.LoginPageViewModel.User.username+"/"+Name+"/");
        }

        private void DownloadFileBody(string filename)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.ShowDialog();
            folderManagerService.GetFileFromAsync(Locator.LoginPageViewModel.User.id, Name, filename);
        }
        #endregion
    }

    public class FolderViewModel : BindableObject
    {
        #region Ppties
        public ICommand DeleteDirectory { get; private set; }
        public ICommand CreateDirectory { get; private set; }
        public ICommand UpdateDirList { get; private set; }
        public ObservableCollection<FolderDefinition> Folders { get; private set; }
        FolderManagerClient _folderManagerService = new FolderManagerClient();
        ViewModelLocator Locator = new ViewModelLocator();
        #endregion

        #region CTor
        public FolderViewModel()
        {
            Folders = new ObservableCollection<FolderDefinition>();
            UpdateDirList = new RelayCommand((param) => UpdateDirListBody());
            CreateDirectory = new RelayCommand((param) => CreateDirectoryBody(param as string));
            DeleteDirectory = new RelayCommand((param) => DeleteDirectoryBody(param as FolderDefinition));
            _folderManagerService.DirListCompleted += new EventHandler<DirListCompletedEventArgs>(_folderManagerService_DirListCompleted);
            _folderManagerService.CreateCompleted += new EventHandler<CreateCompletedEventArgs>(_folderManagerService_CreateCompleted);
            _folderManagerService.DeleteCompleted += new EventHandler<DeleteCompletedEventArgs>(_folderManagerService_DeleteCompleted);
        }
        #endregion

        #region Event
        void _folderManagerService_CreateCompleted(object sender, CreateCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result.ErrorCode == WebResult.ErrorCodeList.SUCCESS)
                {
                    UpdateDirListBody();
                }
                Locator.LoggerViewModel.AddLog("Create new directory: " + e.Result.ErrorCode);
            }
        }

        void _folderManagerService_DeleteCompleted(object sender, DeleteCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Locator.LoggerViewModel.AddLog("Delete directory: " + e.Result.ErrorCode);
            }
        }

        void _folderManagerService_DirListCompleted(object sender, DirListCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result.ErrorCode == WebResult.ErrorCodeList.SUCCESS)
                {
                    var dirs = e.Result.Value;
                    foreach (string dirName in e.Result.Value)
                    {
                        bool exist = false;

                        FolderDefinition folder = new FolderDefinition(dirName);
                        foreach (FolderDefinition f in Folders)
                        {
                            if (f.Name == dirName)
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (!exist)
                        {
                            folder.UpdateFileList.Execute(null);
                            Folders.Add(folder);
                        }
                    }
                    RaisePropertyChange("Folders");
                }
                Locator.LoggerViewModel.AddLog("Get folder list: " + e.Result.ErrorCode);
            }
        }
        #endregion

        #region Command body
        private void UpdateDirListBody()
        {
            _folderManagerService.DirListAsync(Locator.LoginPageViewModel.User.id);
        }

        private void CreateDirectoryBody(string p)
        {
            if (p == null)
                return;
            _folderManagerService.CreateAsync(Locator.LoginPageViewModel.User.id, p);
        }

        private void DeleteDirectoryBody(FolderDefinition folderDefinition)
        {
            if (Folders.Contains(folderDefinition))
            {
                Folders.Remove(folderDefinition);
                RaisePropertyChange("Folders");
                _folderManagerService.DeleteAsync(Locator.LoginPageViewModel.User.id, folderDefinition.Name);
            }
        }
        #endregion
    }
}
