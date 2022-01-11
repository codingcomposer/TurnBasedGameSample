using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadedData
{
    public static AssetBundle stageBundle;
    public static AssetBundle unitBundle;
    public static bool assetsLoaded = false;
    public static void Load(MonoBehaviour loader)
    {
        loader.StartCoroutine(LoadCoroutine());
    }

    private static IEnumerator LoadCoroutine()
    {
        AssetBundleCreateRequest cr;
        cr = AssetBundle.LoadFromFileAsync(Path.Combine(Path.Combine(Application.streamingAssetsPath, "AssetBundles"), "stagebundle"));
        yield return cr;
        stageBundle = cr.assetBundle;

        cr = null;
        cr = AssetBundle.LoadFromFileAsync(Path.Combine(Path.Combine(Application.streamingAssetsPath, "AssetBundles"), "unitbundle"));
        yield return cr;
        unitBundle = cr.assetBundle;
        assetsLoaded = true;
    }
}
