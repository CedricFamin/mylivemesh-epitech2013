using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MyLiveMesh.Utils;
using System.Collections.Generic;
using System.IO;
using MyLiveMesh.LinqToSQL;

namespace MyLiveMesh.services
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FolderManager : IFolderManager
    {
        #region Fields
        private CompositionContainer _container;

        [Import(typeof(IFolderManager))]
        private IFolderManager _manager = null;
        #endregion

        #region CTor
        public FolderManager()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(FolderManager).Assembly));

            _container = new CompositionContainer(catalog);
            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }
        #endregion

        #region OperationContract
        [OperationContract]
        public WebResult Create(int userId, string name)
        {
            return this._manager.Create(userId, name);
        }

        [OperationContract]
        public WebResult Delete(int userId, string name)
        {
            return this._manager.Delete(userId, name);
        }

        [OperationContract]
        public WebResult Rename(int userId, string name, string newName)
        {
            return this._manager.Rename(userId, name, newName);
        }

        [OperationContract]
        public WebResult<List<string>> DirList(int userId)
        {
            return this._manager.DirList(userId);
        }

        [OperationContract]
        public WebResult<List<FileDefinition>> FileList(int userId, string folder)
        {
            return this._manager.FileList(userId, folder);
        }

        [OperationContract]
        public WebResult<FileDefinition> GetFileFrom(int userId, string folder, string file)
        {
            return this._manager.GetFileFrom(userId, folder, file);
        }
        #endregion

        [OperationContract]
        public WebResult AddSharing(int ownerId, int friendId, string folder)
        {
            return this._manager.AddSharing(ownerId, friendId, folder);
        }

        [OperationContract]
        public WebResult<List<Sharing>> GetSharing(int userId)
        {
            return this._manager.GetSharing(userId);
        }

        [OperationContract]
        public WebResult<List<SharingDefinition>> GetReceiver(int userId, string folder)
        {
            return this._manager.GetReceiver(userId, folder);
        }

        [OperationContract]
        public WebResult DeleteSharing(int sharingId)
        {
            return this._manager.DeleteSharing(sharingId);
        }
    }
}
