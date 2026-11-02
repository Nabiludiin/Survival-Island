using UnityEngine;
using UnityEngine.SceneManagement;

namespace AssetBundles.Manager;

public class AssetBundleLoadLevelOperation : AssetBundleLoadOperation
{
	protected string m_AssetBundleName;

	protected string m_LevelName;

	protected bool m_IsAdditive;

	protected string m_DownloadingError;

	protected AsyncOperation m_Request;

	public AssetBundleLoadLevelOperation(string assetbundleName, string levelName, bool isAdditive)
	{
		m_AssetBundleName = assetbundleName;
		m_LevelName = levelName;
		m_IsAdditive = isAdditive;
	}

	public override bool Update()
	{
		if (m_Request != null)
		{
			return false;
		}
		if (AssetBundleManager.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError) != null)
		{
			LoadSceneMode mode = LoadSceneMode.Single;
			if (m_IsAdditive)
			{
				mode = LoadSceneMode.Additive;
			}
			m_Request = SceneManager.LoadSceneAsync(m_LevelName, mode);
			return false;
		}
		return true;
	}

	public override bool IsDone()
	{
		if (m_Request == null && !string.IsNullOrEmpty(m_DownloadingError))
		{
			Debug.LogError(m_DownloadingError);
			return true;
		}
		if (m_Request != null)
		{
			return m_Request.isDone;
		}
		return false;
	}

	public override bool IsError()
	{
		return !string.IsNullOrEmpty(m_DownloadingError);
	}
}
