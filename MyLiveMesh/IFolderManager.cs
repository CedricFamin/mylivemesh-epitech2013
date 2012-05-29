using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLiveMesh.Utils;
using System.IO;

namespace MyLiveMesh
{
    interface IFolderManager
    {
        WebResult Create(int userId, string name);
        WebResult Delete(int userId, string name);
        WebResult Rename(int userId, string name, string newName);
        WebResult<List<string>> DirList(int userId);
        WebResult<List<string>> FileList(int userId, string folder);
        WebResult<byte[]> GetFileFrom(int userId, string folder, string file);
    }
}
