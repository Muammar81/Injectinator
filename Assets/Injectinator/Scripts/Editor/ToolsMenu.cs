using UnityEditor;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;
using static UnityEngine.Application;
using UnityEngine;

public static class ToolsMenu
{
    //[MenuItem("Tools/Setup/Create Default Folders")]
    public static void CreateDefaultFolders()
    {
        string parentDir = "_Project";// Application.dataPath;
        string[] dirs = { parentDir, "Art", "Scripts", "Scenes", "Prefabs", "Audio","Animations" };
        CreateDirs(parentDir, dirs);
        Refresh();
    }

    private static void CreateDirs(string root, params string[] dirs)
    {
        string fullPath = Combine(dataPath, root);
        foreach (string newDir in dirs)
        {
            CreateDirectory(Combine(fullPath, newDir));
        }
    }
}
