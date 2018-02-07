using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetBundles;

public class GameController : MonoBehaviour {

    string workingText;

    IEnumerator Start()
    {
        yield return GetTextFromBundle();
        Debug.Log(workingText.Length);
    }

    IEnumerator GetTextFromBundle()
    {
        yield return StartCoroutine(Initialize());
        yield return StartCoroutine(InstantiateGameObjectAsync("textfile", "alice30"));
    }

    protected IEnumerator Initialize()
    {
        AssetBundleManager.SetDevelopmentAssetBundleServer();
        var request = AssetBundleManager.Initialize();
        if (request != null)
            yield return StartCoroutine(request);
    }

    protected IEnumerator InstantiateGameObjectAsync(string assetBundleName, string assetName)
    {
        // Load asset from assetBundle.
        AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(TextAsset));
        if (request == null)
            yield break;
        yield return StartCoroutine(request);

        // Get the asset.
        TextAsset prefab = request.GetAsset<TextAsset>();
        workingText = prefab.text;
    }
}
