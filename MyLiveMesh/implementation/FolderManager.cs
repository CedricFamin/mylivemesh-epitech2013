using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MyLiveMesh.LinqToSQL;
using MyLiveMesh.Utils;
using System.IO;

namespace MyLiveMesh.implementation
{
    [Export(typeof(IFolderManager))]
    public class FolderManager : IFolderManager
    {
        MyLiveMeshDBDataContext db = new MyLiveMeshDBDataContext();

        public WebResult Create(int userId, string name)
        {
            var user = (from u in db.Users where u.id == userId select u).SingleOrDefault();

            if (user == default(User))
                return new WebResult(WebResult.ErrorCodeList.USER_NOT_FOUND);
            if (user.limit_folder != null)
            {
                if (this.DirList(userId).Value.Count >= user.limit_folder)
                    return new WebResult(WebResult.ErrorCodeList.CANNOT_CREATE_DIRECTORY);
            }
            string path = System.IO.Path.Combine(Config.ROOT_PATH, user.username, name);
            try   { System.IO.Directory.CreateDirectory(path); }
            catch { return new WebResult(WebResult.ErrorCodeList.CANNOT_CREATE_DIRECTORY); }
            return new WebResult();
        }

        public WebResult Delete(int userId, string name)
        {
            var user = (from u in db.Users where u.id == userId select u).SingleOrDefault();

            if (user == default(User))
                return new WebResult(WebResult.ErrorCodeList.USER_NOT_FOUND);
            string path = System.IO.Path.Combine(Config.ROOT_PATH, user.username, name);
            if (System.IO.Directory.Exists(path))
            {
                try   { System.IO.Directory.Delete(path, true); }
                catch { return new WebResult(WebResult.ErrorCodeList.CANNOT_DELETE_DIRECTORY); }
            }
            else 
                return new WebResult(WebResult.ErrorCodeList.DIRECTORY_NOT_FOUND);
            return new WebResult();
        }

        public WebResult Rename(int userId, string name, string newName)
        {
            var user = (from u in db.Users where u.id == userId select u).SingleOrDefault();

            if (user == default(User))
                return new WebResult(WebResult.ErrorCodeList.USER_NOT_FOUND);
            string path = System.IO.Path.Combine(Config.ROOT_PATH, user.username, name);
            string newPath = System.IO.Path.Combine(Config.ROOT_PATH, user.username, newName);
            if (System.IO.Directory.Exists(path))
            {
                try { System.IO.Directory.Move(path, newPath); }
                catch { return new WebResult(WebResult.ErrorCodeList.CANNOT_RENAME_DIRECTORY); }
            }
            else
                return new WebResult(WebResult.ErrorCodeList.DIRECTORY_NOT_FOUND);
            return new WebResult();
        }


        public WebResult<List<string>> DirList(int userId)
        {
            List<string> dirs = new List<string>();

            var user = (from u in db.Users where u.id == userId select u).SingleOrDefault();

            if (user == default(User))
                return new WebResult<List<string>>(WebResult.ErrorCodeList.USER_NOT_FOUND);

            string path = System.IO.Path.Combine(Config.ROOT_PATH, user.username);
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            foreach (DirectoryInfo directory in dirInfo.GetDirectories())
            {
                dirs.Add(directory.Name);
            }
            return new WebResult<List<string>>(dirs);
        }

        public WebResult<List<FileDefinition>> FileList(int userId, string folder)
        {
            List<FileDefinition> files = new List<FileDefinition>();

            var user = (from u in db.Users where u.id == userId select u).SingleOrDefault();

            if (user == default(User))
                return new WebResult<List<FileDefinition>>(WebResult.ErrorCodeList.USER_NOT_FOUND);

            string path = System.IO.Path.Combine(Config.ROOT_PATH, user.username, folder);
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                files.Add(new FileDefinition()
                    {
                        FileUri = HttpContext.Current.Request.Url.ToString() + "/../../upload_files/" + user.username + "/" + folder + "/" + file.Name,
                        Filename = file.Name
                    });
            }
            return new WebResult<List<FileDefinition>>(files);
        }

        public WebResult<FileDefinition> GetFileFrom(int userId, string folder, string file)
        {
            WebResult<FileDefinition> result = new WebResult<FileDefinition>();
            FileDefinition fd = new FileDefinition();
            result.Value = fd;
            var user = (from u in db.Users where u.id == userId select u).SingleOrDefault();

            if (user == default(User))
                return new WebResult<FileDefinition>(WebResult.ErrorCodeList.USER_NOT_FOUND);
            string path = System.IO.Path.Combine(Config.ROOT_PATH, user.username, folder, file);
            if (System.IO.File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                fd.RawData = new byte[stream.Length];
                stream.Read(fd.RawData, 0, (int)stream.Length);
                fd.Filename = file;
                fd.FileUri = HttpContext.Current.Request.Url.ToString() + "/../../upload_files/" + user.username + "/" + folder + "/" + file;
            }
            else
                return new WebResult<FileDefinition>(WebResult.ErrorCodeList.FILE_NOT_FOUND);

            return result;
        }
    }
}