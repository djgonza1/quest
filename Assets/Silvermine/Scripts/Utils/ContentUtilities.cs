using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CommandTerminal;

public static class ContentUtilities
{
    public static string GetApplicationDataPath()
    {
        return Application.dataPath;
    }
    
    public static string GetAssetBundlesPath()
    {
        return Path.Combine(Application.dataPath, "AssetBundles");
    }
    
    public static string GetCardsBundlePath()
    {
        return Path.Combine(GetAssetBundlesPath(), "gameobjects/cards");
    }
}
