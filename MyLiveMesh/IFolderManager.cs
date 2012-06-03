using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLiveMesh.Utils;
using System.IO;
using MyLiveMesh.LinqToSQL;

namespace MyLiveMesh
{
    interface IFolderManager
    {
        WebResult Create(int userId, string name);
        WebResult Delete(int userId, string name);
        WebResult Rename(int userId, string name, string newName);
        WebResult<List<string>> DirList(int userId);
        WebResult<List<FileDefinition>> FileList(int userId, string folder);
        WebResult<FileDefinition> GetFileFrom(int userId, string folder, string file);
        WebResult AddSharing(int ownerId, int friendId, string folder);
        WebResult<List<Sharing>> GetSharing(int userId);
        WebResult<List<SharingDefinition>> GetReceiver(int userId, string folder);
        WebResult DeleteSharing(int sharingId);
    }
}
