//ref project https://github.com/xwbash/UDT/tree/main/DropboxTool

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DropboxTool
{
    public class DropboxUploader : EditorWindow
    {
        public string dropboxAppName;
        
        public static string Err;
        public static string Inf;
        
        [Header("Folder Names")]
        public List<string> folderNames = new List<string>();

        private void OnEnable()
        {
            string[] s = Application.dataPath.Split('/');
            dropboxAppName = s[s.Length - 2];
            Err = "";
            Inf = "";
        }

        private void UploadFiles()
        {
            DropboxUpload.StartProcess(dropboxAppName, folderNames);
        }
        
        private void CheckFiles()
        {
            string[] files = AssetDatabase.GetSubFolders("Assets");
            foreach (var file in files)
            {
                var replace = file.Replace("Assets/", "");
                folderNames.Add(replace);
            }
        }

        [MenuItem("Window/Dropbox Tool")]
        public static void ShowWindow()
        {
            GetWindow<DropboxUploader>("Dropbox Upload Tool");
        }
        
        private void OnGUI()
        {
            EditorGUILayout.Space(10);
            GUIStyle appNameLabel = new GUIStyle(GUI.skin.textField)
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            GUIStyle appNameField = new GUIStyle(GUI.skin.textField)
            {
                fontSize = 13,
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.Label("Dropbox App Name", appNameLabel);
            EditorGUILayout.TextField(dropboxAppName, appNameField);
            
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(new SerializedObject(this).FindProperty("folderNames"), true); // True means show children
            
            EditorGUILayout.Space();
            if (folderNames.Count > 0)
            {
                if (GUILayout.Button("Upload Folders"))
                    UploadFiles();
            }
            else
            {
                EditorGUILayout.HelpBox("No folders selected", MessageType.Warning, true);
                if (GUILayout.Button("Check Folders"))
                    CheckFiles();
            }

            if (Err != "")
                EditorGUILayout.HelpBox(Err, MessageType.Error, true);

            if (Inf != "")
                EditorGUILayout.HelpBox(Inf, MessageType.Info, true);
        }
    }
}