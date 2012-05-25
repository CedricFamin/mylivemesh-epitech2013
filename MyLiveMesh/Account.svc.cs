using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace MyLiveMesh
{
    [ServiceContract(Namespace = "")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Account
    {
        MyLiveMeshDBDataContext db = new MyLiveMeshDBDataContext();

        [OperationContract]
        public bool Register(string username, string email, string password)
        {
            var users = from u in db.Users where u.username == username || u.email == email select u;
            if (password == "" || users.Count() > 0)
                return false;
            User user = new User()
            {
                username=username,
                email=email,
                password=password,
                superuser=false,
                root_path=username
            };

            System.IO.Directory.CreateDirectory(System.IO.Path.Combine("D:\\Work\\Epitech\\Tech4\\mylivemesh\\MyLiveMesh", "upload_files", user.root_path));
            db.Users.InsertOnSubmit(user);
            db.SubmitChanges();
            return true;
        }

        [OperationContract]
        public User Login(string username, string password)
        {
            var users = from u in db.Users where u.username == username && u.password == password select u;
            if (users.Count() != 1)
                return null;
            return users.First();
        }

        // Add more operations here and mark them with [OperationContract]
    }
}
