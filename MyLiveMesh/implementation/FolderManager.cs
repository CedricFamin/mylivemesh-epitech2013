using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MyLiveMesh.LinqToSQL;
using MyLiveMesh.Utils;

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
    }
}