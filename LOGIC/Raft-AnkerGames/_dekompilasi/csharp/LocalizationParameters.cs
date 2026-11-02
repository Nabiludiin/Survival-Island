using I2.Loc;
using UnityEngine;

public class LocalizationParameters : MonoBehaviour, ILocalizationParamsManager
{
	public static int itemCount;

	public static string itemX;

	public static string mainMenuRaftBuiltByName;

	public static Network_UserId remoteSteamID;

	private Network_Player localPlayer;

	private PlayerInventory playerInventory;

	private Raft_Network network;

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	private void OnEnable()
	{
		if (!LocalizationManager.ParamManagers.Contains(this))
		{
			LocalizationManager.ParamManagers.Add(this);
			LocalizationManager.LocalizeAll(Force: true);
		}
	}

	private void OnDestroy()
	{
		LocalizationManager.ParamManagers.Remove(this);
	}

	public virtual string GetParameterValue(string parameter)
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (playerInventory == null && localPlayer != null)
		{
			playerInventory = localPlayer.Inventory;
		}
		switch (parameter)
		{
		case "RotateKey":
			return MyInput.Keybinds["Rotate"].MainKey.ToShortString();
		case "RMBKey":
			return MyInput.Keybinds["RMB"].MainKey.ToShortString();
		case "CurrentItem":
			if (localPlayer != null)
			{
				ItemInstance selectedHotbarItem = playerInventory.GetSelectedHotbarItem();
				if (selectedHotbarItem != null && selectedHotbarItem.Valid)
				{
					return selectedHotbarItem.baseItem.settings_Inventory.DisplayName;
				}
			}
			break;
		case "ItemX":
			return itemX;
		case "ItemCount":
			return itemCount.ToString();
		case "RemoteSteamID":
		{
			Network_Player playerFromID = network.GetPlayerFromID(remoteSteamID);
			if (playerFromID != null)
			{
				return playerFromID.visualName;
			}
			return null;
		}
		case "CarriedPlayer":
			if (localPlayer != null)
			{
				return localPlayer.RessurectComponent.CarriedPlayer.visualName;
			}
			break;
		case "IncapacitatedPlayer":
			if (localPlayer != null)
			{
				return localPlayer.RessurectComponent.IncapacitatedPlayerAtCursor.visualName;
			}
			break;
		case "RaftBuiltByName":
			return mainMenuRaftBuiltByName;
		}
		return null;
	}
}
