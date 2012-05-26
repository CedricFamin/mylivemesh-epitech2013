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
            return new WebResult();
        }

        public WebResult Delete(int folderId)
        {
            return new WebResult();
        }

        public WebResult Rename(int folderId, string name)
        {
            return new WebResult();
        }
    }
}