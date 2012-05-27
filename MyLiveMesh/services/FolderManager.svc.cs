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

namespace MyLiveMesh.services
{
    [ServiceContract(Namespace = "")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FolderManager : IFolderManager
    {
        #region Fields
        private CompositionContainer _container;

        [Import(typeof(IFolderManager))]
        private IFolderManager _manager = null;
        #endregion

        #region CTor
        FolderManager()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Account).Assembly));

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
        public WebResult<List<string>> FileList(int userId, string folder)
        {
            return this._manager.FileList(userId, folder);
        }
        #endregion
    }
}
