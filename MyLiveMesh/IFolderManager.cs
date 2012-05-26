using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLiveMesh
{
    interface IFolderManager
    {
        bool Create(int userId, string name);
        bool Delete(int folderId);
        bool Rename(int folderId, string name);
    }
}
