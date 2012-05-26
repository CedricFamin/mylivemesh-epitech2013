using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace MyLiveMesh
{

    [ServiceContract(Namespace = "")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Account : IAccount
    {
        #region Fields
        private CompositionContainer _container;
        
        [Import(typeof(IAccount))]
        private IAccount _account = null;
        #endregion

        #region CTor
        Account()
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
        public bool Register(string username, string email, string password)
        {
            return this._account.Register(username, email, password);
        }

        [OperationContract]
        public User Login(string username, string password)
        {
            return this._account.Login(username, password);
        }

        [OperationContract]
        public void Update(User updateUser)
        {
            this._account.Update(updateUser);
        }
        #endregion
    }
}
