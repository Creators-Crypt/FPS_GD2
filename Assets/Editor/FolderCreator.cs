using System.IO;
using UnityEditor;
using UnityEngine;

public static class FolderCreator {

    [MenuItem("Tools/Create Project Folders")]
    public static void CreatePredefinedFolders() {

        string rootName = "_Project";
        string rootPath = Path.Combine("Assets", rootName);

        string[] subFolders = new[] {
            "Animations",
            "Audio",
            "Materials",
            "Models",
            "Prefabs",
            "Scripts",
            "Textures"
        };

        CheckCreateFolder("Assets", rootName);

        foreach (string folder in subFolders) CheckCreateFolder(rootPath, folder);

        AssetDatabase.Refresh();
        Debug.Log($"Successfully initialized project folder structure under {rootPath}!");
    }

    private static void CheckCreateFolder(string parentPath, string folderName) {
        
        string fullPath = Path.Combine(parentPath, folderName);

        if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
    }
}