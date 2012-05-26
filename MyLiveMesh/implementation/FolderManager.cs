using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace MyLiveMesh.implementation
{
    [Export(typeof(IFolderManager))]
    public class FolderManager : IFolderManager
    {
        MyLiveMeshDBDataContext db = new MyLiveMeshDBDataContext();

        public bool Create(int userId, string name)
        {
            try
            {
                Folder folder = new Folder()
                {
                    name = name,
                    User1 = (from u in db.Users where u.id == userId select u).Single()
                };

                db.Folders.InsertOnSubmit(folder);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int folderId)
        {
            try
            {
                Folder entity = (from f in db.Folders where f.id == folderId select f).Single();
                db.Folders.DeleteOnSubmit(entity);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Rename(int folderId, string name)
        {
            throw new NotImplementedException();
        }
    }
}