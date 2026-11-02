using RG.Parsecs.EventEditor;
using Steamworks;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
	public static bool IS_DEMO_VERSION = false;

	public const int DEMO_SURVIVAL_DAYS = 11;

	public const int DEMO_FORCE_BACK_FROM_EXPEDITION_DAY = 9;

	public const string DEMO_SCAVENGE_LEVEL_NAME = "level_scavenge_11";

	public static uint REATOMIZED_FULL_VERSION_APP_ID = 1012880u;

	[SerializeField]
	private GlobalBoolVariable _isDemoVariable;

	private AppId_t _fullVersionAppId;

	private void Start()
	{
		Object.DontDestroyOnLoad(this);
		_fullVersionAppId = new AppId_t(REATOMIZED_FULL_VERSION_APP_ID);
		Debug.Log("Reatomized manual version " + _fullVersionAppId.ToString() + " and automatic from steam" + SteamUtils.GetAppID().ToString());
		CheckIfGameIsDemo();
	}

	public void CheckIfGameIsDemo()
	{
		if (!(_isDemoVariable == null))
		{
			bool flag = SteamApps.BIsSubscribedApp(_fullVersionAppId);
			_isDemoVariable.Value = !flag;
			IS_DEMO_VERSION = _isDemoVariable.Value;
		}
	}
}
