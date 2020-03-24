//using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class GameBuildPipeline
{
    [MenuItem("Silvermine/Builds/BuildClient")]
    public static void BuildClient ()
    {
        string executableName = "quest";

        string[] levels = new string[] 
        {
            "Assets/Silvermine/Scenes/start_scene.unity", 
            "Assets/Silvermine/Scenes/board_scene.unity"
        };

        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");

        BuildAddressables();

        // Build player.
        BuildPipeline.BuildPlayer(levels, path + "/" + executableName + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);

        // Copy a file from the project folder to the build folder, alongside the built game.
        //FileUtil.CopyFileOrDirectory("Assets/Templates/Readme.txt", path + "Readme.txt");

        // Run the game (Process class from System.Diagnostics).
        // Process proc = new Process();
        // proc.StartInfo.FileName = path + "/BuiltGame.exe";
        // proc.Start();
    }
    
    private static void BuildAddressables()
    {
        UnityEditor.AddressableAssets.Settings.AddressableAssetSettings.CleanPlayerContent();
        UnityEditor.AddressableAssets.Settings.AddressableAssetSettings.BuildPlayerContent();
    }
}

