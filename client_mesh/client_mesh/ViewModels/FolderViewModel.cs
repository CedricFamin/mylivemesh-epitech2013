using System;
using System.Linq;
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
using client_mesh;
using System.Collections.Generic;

namespace client_mesh.ViewModels
{
    public class FolderDefinition : BindableObject
    {
        #region Ppties
        public ICommand UploadFiles { get; private set; }
        public ICommand UpdateFileList { get; private set; }
        public ICommand DownloadFile { get; private set; }
        public ICommand RefreshUserList { get; private set; }
        public ICommand AddSharing { get; private set; }
        public ICommand GetReceiver { get; private set; }
        public ICommand DeleteSharing { get; private set; }

        public ObservableCollection<SharingDefinition> Receivers { get; private set; }
        public ObservableCollection<ServiceReference.User> Users { get; private set; }
        public string Name { get; private set; }
        public ObservableCollection<FileDefinition> Files { get; private set; }
        public ServiceReference.User Owner { get; set; }

        FolderManagerClient folderManagerService = new FolderManagerClient();
        ServiceReference.AccountClient accountService = new ServiceReference.AccountClient();
        ViewModelLocator Locator = new ViewModelLocator();
        #endregion

        #region Ctor
        public FolderDefinition(string name)
        {
            Name = name;
            UploadFiles = new RelayCommand((param) => UploadFilesBody());
            UpdateFileList = new RelayCommand((param) => UpdateFileListBody());
            DownloadFile = new RelayCommand((param) => DownloadFileBody(param as string));
            RefreshUserList = new RelayCommand((param) => RefreshUserListBody());
            AddSharing = new RelayCommand((param) => AddSharingBody(param as ServiceReference.User));
            GetReceiver = new RelayCommand((param) => GetReceiverBody());
            DeleteSharing = new RelayCommand((param) => DeleteSharingBody(param as SharingDefinition));

            folderManagerService.FileListCompleted += new EventHandler<FileListCompletedEventArgs>(folderManagerService_FileListCompleted);
            folderManagerService.GetFileFromCompleted +=new EventHandler<GetFileFromCompletedEventArgs>(folderManagerService_GetFileFromCompleted);
            folderManagerService.AddSharingCompleted += new EventHandler<AddSharingCompletedEventArgs>(folderManagerService_AddSharingCompleted);
            accountService.UserListCompleted += new EventHandler<ServiceReference.UserListCompletedEventArgs>(accountService_UserListCompleted);
            folderManagerService.GetReceiverCompleted += new EventHandler<GetReceiverCompletedEventArgs>(folderManagerService_GetReceiverCompleted);
            folderManagerService.DeleteSharingCompleted += new EventHandler<DeleteSharingCompletedEventArgs>(folderManagerService_DeleteSharingCompleted);
            Files = new ObservableCollection<FileDefinition>();
            Locator.Folders.Add(this);
        }

        ~FolderDefinition()
        {
            Locator.Folders.Remove(this);
        }
        #endregion

        #region event
        void folderManagerService_DeleteSharingCompleted(object sender, DeleteSharingCompletedEventArgs e)
        {
            if (e.Error != null)
                return;
            GetReceiverBody();
            Locator.LoggerViewModel.AddLog("Get receiver list: " + e.Result.ErrorCode);
        }

        void folderManagerService_GetReceiverCompleted(object sender, GetReceiverCompletedEventArgs e)
        {
            if (e.Error != null)
                return;
            if (e.Result.ErrorCode == WebResult.ErrorCodeList.SUCCESS)
            {
                Receivers = e.Result.Value;
                RaisePropertyChange("Receivers");
            }
            Locator.LoggerViewModel.AddLog("Get receiver list: " + e.Result.ErrorCode);
        }

        void folderManagerService_AddSharingCompleted(object sender, AddSharingCompletedEventArgs e)
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
                Locator.LoggerViewModel.AddLog("Add sharing: " + e.Result.ErrorCode);
            }
        }

        void accountService_UserListCompleted(object sender, ServiceReference.UserListCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Users = e.Result.Value;
                RaisePropertyChange("Users");
            }
        }

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
            folderManagerService.FileListAsync(Owner.id, Name);
        }

        private void UploadFilesBody()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Multiselect = true;
            fd.ShowDialog();
            Locator.FileUploaderViewModel.AddFiles(fd.Files, Owner.username+"/"+Name+"/");
        }

        private void DownloadFileBody(string filename)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.ShowDialog();
            folderManagerService.GetFileFromAsync(Owner.id, Name, filename);
        }

        private void RefreshUserListBody()
        {
            if (Owner.id == Locator.LoginPageViewModel.User.id)
                accountService.UserListAsync();
        }

        private void AddSharingBody(ServiceReference.User user)
        {
            if (user != null && Owner.id == Locator.LoginPageViewModel.User.id)
            {
                folderManagerService.AddSharingAsync(Owner.id, user.id, Name);
            }   
        }

        private void GetReceiverBody()
        {
            folderManagerService.GetReceiverAsync(Owner.id, Name);
        }

        private void DeleteSharingBody(SharingDefinition sharingDefinition)
        {
            if (sharingDefinition != null)
                folderManagerService.DeleteSharingAsync(sharingDefinition.SharingId);
        }

        #endregion
    }

    public class FolderViewModel : BindableObject
    {
        #region Ppties
        public ICommand DeleteDirectory { get; private set; }
        public ICommand CreateDirectory { get; private set; }
        public ICommand UpdateDirList { get; private set; }
        public ObservableCollection<KeyValuePair<string, ObservableCollection<FolderDefinition>>> Folders {get; private set;}
        FolderManagerClient _folderManagerService = new FolderManagerClient();
        ViewModelLocator Locator = new ViewModelLocator();
        #endregion

        #region CTor
        public FolderViewModel()
        {
            Folders = new ObservableCollection<KeyValuePair<string, ObservableCollection<FolderDefinition>>>();
            UpdateDirList = new RelayCommand((param) => UpdateDirListBody());
            CreateDirectory = new RelayCommand((param) => CreateDirectoryBody(param as string));
            DeleteDirectory = new RelayCommand((param) => DeleteDirectoryBody(param as FolderDefinition));
            _folderManagerService.DirListCompleted += new EventHandler<DirListCompletedEventArgs>(_folderManagerService_DirListCompleted);
            _folderManagerService.CreateCompleted += new EventHandler<CreateCompletedEventArgs>(_folderManagerService_CreateCompleted);
            _folderManagerService.DeleteCompleted += new EventHandler<DeleteCompletedEventArgs>(_folderManagerService_DeleteCompleted);
            _folderManagerService.GetSharingCompleted += new EventHandler<GetSharingCompletedEventArgs>(_folderManagerService_GetSharingCompleted);
        }
        #endregion

        #region Event
        void _folderManagerService_GetSharingCompleted(object sender, GetSharingCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result.ErrorCode == WebResult.ErrorCodeList.SUCCESS)
                {
                    foreach (Sharing share in e.Result.Value)
                    {
                        KeyValuePair<string, ObservableCollection<FolderDefinition>> folderContainer;
                        folderContainer = (from f in Folders where f.Key == share.user select f).SingleOrDefault();
                        if (folderContainer.Key == null && folderContainer.Value == null)
                        {
                            folderContainer = new KeyValuePair<string, ObservableCollection<FolderDefinition>>(share.user, new ObservableCollection<FolderDefinition>());
                            Folders.Add(folderContainer);
                        }
                        FolderDefinition fd = new FolderDefinition(share.folder)
                        {
                            Owner = new ServiceReference.User()
                            {
                                id = share.idOwner,
                                username = share.user,
                            }
                        };
                        folderContainer.Value.Add(fd);
                        fd.UpdateFileList.Execute(null);
                    }
                    RaisePropertyChange("Folders");
                }
                Locator.LoggerViewModel.AddLog("Get sharing directory: " + e.Result.ErrorCode);
            }
        }

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
                    Folders.Clear();
                    KeyValuePair<string, ObservableCollection<FolderDefinition>> folderContainer = new KeyValuePair<string, ObservableCollection<FolderDefinition>>("~", new ObservableCollection<FolderDefinition>());
                    Folders.Add(folderContainer);
                    foreach (string dirName in e.Result.Value)
                    {
                        FolderDefinition folder = new FolderDefinition(dirName);
                        folder.Owner = Locator.LoginPageViewModel.User;
                        folder.UpdateFileList.Execute(null);
                        folderContainer.Value.Add(folder);
                    }
                    RaisePropertyChange("Folders");
                    _folderManagerService.GetSharingAsync(Locator.LoginPageViewModel.User.id);
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
            foreach (KeyValuePair<string, ObservableCollection<FolderDefinition>> uf in Folders)
            {
                if (uf.Value.Contains(folderDefinition))
                {
                    if (uf.Key == "~")
                    {
                        uf.Value.Remove(folderDefinition);
                        RaisePropertyChange("Folders");
                        _folderManagerService.DeleteAsync(Locator.LoginPageViewModel.User.id, folderDefinition.Name);
                    }
                }
                    
            }     
        }
        #endregion
    }
}
