using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadedData
{
    public static AssetBundle stageBundle;
    public static AssetBundle unitBundle;
    public static AssetBundle itemBundle;
    public static bool assetsLoaded = false;
    private static string bundlesPath = Path.Combine(Application.streamingAssetsPath, "AssetBundles");
    public static void Load(MonoBehaviour loader)
    {
        loader.StartCoroutine(LoadCoroutine());
    }

    private static IEnumerator LoadCoroutine()
    {
        AssetBundleCreateRequest cr;
        cr = AssetBundle.LoadFromFileAsync(Path.Combine(bundlesPath, "stagebundle"));
        yield return cr;
        stageBundle = cr.assetBundle;

        cr = null;
        cr = AssetBundle.LoadFromFileAsync(Path.Combine(bundlesPath, "unitbundle"));
        yield return cr;
        unitBundle = cr.assetBundle;

        cr = null;
        cr = AssetBundle.LoadFromFileAsync(Path.Combine(bundlesPath, "itembundle"));
        yield return cr;
        itemBundle = cr.assetBundle;
        assetsLoaded = true;
    }
}
