using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLiveMesh
{
    interface IFileUploadService
    {
        string Upload(string id, string mode, string path, string name, string filedata, bool overwrite, string tag, bool final);

    }
}
