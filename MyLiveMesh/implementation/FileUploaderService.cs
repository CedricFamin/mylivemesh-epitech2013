using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.Composition;

namespace MyLiveMesh.implementation
{
    [Export(typeof(IFileUploadService))]
    public class FileUploaderService : IFileUploadService
    {
        public string Upload(string id, string mode, string path, string name, string filedata, bool overwrite, string tag, bool final)
        {
            string filename = string.Empty;

            try
            {
                filename = System.IO.Path.Combine(Config.ROOT_PATH, path, name);
                if (mode == "new")
                {
                    if (File.Exists(filename) == true)
                    {
                        if (overwrite)
                        {
                            File.Delete(filename);
                        }
                        else
                        {
                            return "File Already Exists";
                        }
                    }

                    WriteFile(filename, Convert.FromBase64String(filedata), FileMode.Create);
                }
                else
                {
                    WriteFile(filename, Convert.FromBase64String(filedata), FileMode.Append);
                }
            }
            catch (Exception ex)
            {
                File.Delete(filename);
                return "File Write Error: " + ex.Message;
            }

            return "ok";
        }

        private void WriteFile(string filename, byte[] content, FileMode fileMode)
        {
            Stream target = null;
            BinaryWriter writer = null;

            try
            {
                target = File.Open(filename, fileMode);
                writer = new BinaryWriter(target);

                writer.Write(content);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not write to file: " + filename + " - " + ex.Message);
            }
            finally
            {
                if (target != null)
                {
                    target.Close();
                }
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }
    }
}