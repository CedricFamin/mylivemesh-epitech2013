using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MyLiveMesh.Utils;
using MyLiveMesh.LinqToSQL;
using System.Collections.Generic;

namespace MyLiveMesh
{

    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Account : IAccount
    {
        #region Fields
        private CompositionContainer _container;
        
        [Import(typeof(IAccount))]
        private IAccount _account = null;
        #endregion

        #region CTor
        public Account()
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
        public WebResult Register(string username, string email, string password)
        {
            return this._account.Register(username, email, password);
        }

        [OperationContract]
        public WebResult<User> Login(string username, string password)
        {
            return this._account.Login(username, password);
        }

        [OperationContract]
        public WebResult Update(User updateUser)
        {
            return this._account.Update(updateUser);
        }

        [OperationContract]
        public WebResult Delete(int id)
        {
            return this._account.Delete(id);
        }

        [OperationContract]
        public WebResult<List<User>> UserList()
        {
            return this._account.UserList();
        }
        #endregion
    }
}
