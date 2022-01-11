using UnityEngine;
using UnityEditor;
using System.IO;
public class BuildAsssetBundles 
{ 
    [MenuItem("Assets/Build AssetBundles/PC")] 
    static void BuildAndroidAssetBundles() 
    {
        FileUtil.DeleteFileOrDirectory("Assets/StreamingAssets/AssetBundles");
        if (!Directory.Exists("Assets/StreamingAssets/AssetBundles"))
        {
            Directory.CreateDirectory("Assets/StreamingAssets/AssetBundles");
        }
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows); 
    } 

    [MenuItem("Assets/Build AssetBundles/iOS")]
    static void BuildiOSAssetBundles()
    {
        FileUtil.DeleteFileOrDirectory("Assets/StreamingAssets/AssetBundles");
        if (!Directory.Exists("Assets/StreamingAssets/AssetBundles"))
        {
            Directory.CreateDirectory("Assets/StreamingAssets/AssetBundles");
        }
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
    }
}