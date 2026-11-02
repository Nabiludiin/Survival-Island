using System.Collections;
using UnityEngine;

namespace AssetBundles.Manager;

public class AssetLoader : MonoBehaviour
{
	public const string AssetBundlesOutputPath = "/AssetBundles/";

	public string assetBundleName;

	public string assetName;

	private IEnumerator Start()
	{
		yield return StartCoroutine(Initialize());
		yield return StartCoroutine(InstantiateGameObjectAsync(assetBundleName, assetName));
	}

	protected IEnumerator Initialize()
	{
		Object.DontDestroyOnLoad(gameObject);
		AssetBundleLoadManifestOperation assetBundleLoadManifestOperation = AssetBundleManager.Initialize();
		if (assetBundleLoadManifestOperation != null)
		{
			yield return StartCoroutine(assetBundleLoadManifestOperation);
		}
	}

	protected IEnumerator InstantiateGameObjectAsync(string assetBundleName, string assetName)
	{
		float startTime = Time.realtimeSinceStartup;
		AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(GameObject));
		if (request != null)
		{
			yield return StartCoroutine(request);
			GameObject asset = request.GetAsset<GameObject>();
			if (asset != null)
			{
				Object.Instantiate(asset);
			}
			float num = Time.realtimeSinceStartup - startTime;
			Debug.Log(assetName + ((asset == null) ? " was not" : " was") + " loaded successfully in " + num + " seconds");
		}
	}
}
