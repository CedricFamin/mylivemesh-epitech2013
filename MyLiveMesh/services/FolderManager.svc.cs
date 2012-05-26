using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
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
        public bool Create(int userId, string name)
        {
            return this._manager.Create(userId, name);
        }

        [OperationContract]
        public bool Delete(int folderId)
        {
            return this._manager.Delete(folderId);
        }

        [OperationContract]
        public bool Rename(int folderId, string name)
        {
            return this._manager.Rename(folderId, name);
        }
        #endregion
    }
}
