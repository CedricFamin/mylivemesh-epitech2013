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
                try   { System.IO.Directory.Delete(path); }
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

        public WebResult<List<string>> FileList(int userId, string folder)
        {
            List<string> files = new List<string>();

            var user = (from u in db.Users where u.id == userId select u).SingleOrDefault();

            if (user == default(User))
                return new WebResult<List<string>>(WebResult.ErrorCodeList.USER_NOT_FOUND);

            string path = System.IO.Path.Combine(Config.ROOT_PATH, user.username, folder);
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                files.Add(file.Name);
            }
            return new WebResult<List<string>>(files);
        }

        public WebResult<byte[]> GetFileFrom(int userId, string folder, string file)
        {
            WebResult<byte[]> result = new WebResult<byte[]>();

            var user = (from u in db.Users where u.id == userId select u).SingleOrDefault();

            if (user == default(User))
                return new WebResult<byte[]>(WebResult.ErrorCodeList.USER_NOT_FOUND);
            string path = System.IO.Path.Combine(Config.ROOT_PATH, user.username, folder, file);
            if (System.IO.File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                result.Value = new byte[stream.Length];
                stream.Read(result.Value, 0, (int)stream.Length);
            }
            else
                return new WebResult<byte[]>(WebResult.ErrorCodeList.FILE_NOT_FOUND);

            return result;
        }
    }
}