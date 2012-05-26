using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace MyLiveMesh
{
    interface IAccount
    {
        bool Register(string username, string email, string password);
        User Login(string username, string password);
        bool Update(User updateUser);
        bool Delete(int id);
    }
}
