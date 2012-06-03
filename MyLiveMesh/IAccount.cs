using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using MyLiveMesh.Utils;
using MyLiveMesh.LinqToSQL;

namespace MyLiveMesh
{
    public interface IAccount
    {
        WebResult Register(string username, string email, string password);
        WebResult<User> Login(string username, string password);
        WebResult Update(User updateUser);
        WebResult Delete(int id);
        WebResult<List<User>> UserList();
    }
}
