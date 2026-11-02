namespace PlayFab.Party;

public class PlayFabLocalPlayer : PlayFabPlayer
{
	internal string _preferredLanguageCode;

	public bool IsChatControlAvailable => _chatControlHandle != null;

	public string LanguageCode
	{
		get
		{
			return PlayFabMultiplayerManager.Get()._GetLanguageCode(EntityKey, isLocal: true);
		}
		set
		{
			if (!IsChatControlAvailable)
			{
				_preferredLanguageCode = value;
			}
		}
	}

	public string PlatformSpecificUserId => _platformSpecificUserId;

	public PlayFabLocalPlayer()
	{
		_isLocal = true;
	}
}
