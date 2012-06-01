using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;

namespace MyLiveMesh.services
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FileUploadService : IFileUploadService
    {
        #region Fields
        private CompositionContainer _container;

        [Import(typeof(IFileUploadService))]
        private IFileUploadService _fileuploader = null;
        #endregion
        
        #region CTor
        public FileUploadService()
        { 
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(FileUploadService).Assembly));

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

        [OperationContract]
        public string Upload(string id, string mode, string path, string name, string filedata, bool overwrite, string tag, bool final)
        {
            return this._fileuploader.Upload(id, mode, path, name, filedata, overwrite, tag, final);
        }
    }
}
