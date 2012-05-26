using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace MyLiveMesh.implementation
{
    [Export(typeof(IAccount))]
    public class Account : IAccount
    {
        MyLiveMeshDBDataContext db = new MyLiveMeshDBDataContext();

        public bool Register(string username, string email, string password)
        {
            var users = from u in db.Users where u.username == username || u.email == email select u;
            if (password == "" || users.Count() > 0)
                return false;
            User user = new User()
            {
                username = username,
                email = email,
                password = password,
                superuser = false,
                root_path = username
            };
            // TODO: decommenter
            // System.IO.Directory.CreateDirectory(System.IO.Path.Combine("D:\\Work\\Epitech\\Tech4\\mylivemesh\\MyLiveMesh", "upload_files", user.root_path));
            db.Users.InsertOnSubmit(user);
            db.SubmitChanges();
            return true;
        }

        public User Login(string username, string password)
        {
            try
            {
                return (from u in db.Users where u.username == username && u.password == password select u).Single();
            }
            catch
            {
                return null;
            }
        }

        public bool Update(User updateUser)
        {
            try
            {
                var user = (from u in db.Users where u.id == updateUser.id select u).Single();
                user.password = updateUser.password;
                user.email = updateUser.email;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var user = (from u in db.Users where u.id == id select u).Single();
                db.Users.DeleteOnSubmit(user);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}