//ref project https://github.com/xwbash/UDT/tree/main/DropboxTool

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DropboxTool
{
    public class DropboxUpload : MonoBehaviour
    {
        public static void StartProcess(string dropboxAppName, List<string> folderNames)
        {
            foreach (var folderName in folderNames)
            {
                var sourcePath = Application.dataPath + "/" + folderName;
                var destPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Dropbox/" + dropboxAppName + "/" + folderName;
                CopyDirectory(sourcePath, destPath);
            }
        }

        private static void CopyDirectory(string sourcePath, string destPath)
        {
            try
            {
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }

                var files = Directory.GetFiles(sourcePath);
                foreach (var file in files)
                {
                    var name = Path.GetFileName(file);
                    if (name.Contains(".meta")) continue;
                    var dest = Path.Combine(destPath, name);
                    if (File.Exists(destPath + "/" + file)) continue;
                    Debug.Log("File Name: " + destPath + "/" + name + " " + File.Exists(destPath + "/" + file));
                    File.Copy(file, dest);
                }

                var folders = Directory.GetDirectories(sourcePath);
                foreach (var folder in folders)
                {
                    var name = Path.GetFileName(folder);
                    var dest = Path.Combine(destPath, name);
                    CopyDirectory(folder, dest);
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("already exists"))
                {
                    DropboxUploader.Inf = e.Message;
                }
                else
                {
                    DropboxUploader.Err = e.Message;
                }
            }
        }
    }
}
