using RG.Core.Base;
using RG.Core.Data.Abstractions;
using RG.Core.Data.Specialisations;
using RG.Core.Flow.Abstractions;
using UnityEngine;

namespace RG.Core.Flow.Specialisations;

public class ProfileManager : RGMonoBehaviourSingleton<ProfileManager>, IManager, IDataOwner<ProfileCollectionData>
{
	private ProfileData _currentProfile;

	[SerializeField]
	private ProfileCollectionData _data;

	public ProfileData CurrentProfile => _currentProfile;

	public ProfileCollectionData Data
	{
		protected get
		{
			return _data;
		}
		set
		{
			_data = value;
		}
	}

	public bool SelectProfile(string playerId, string systemId = null)
	{
		ProfileData profile = GetProfile(playerId, systemId);
		if (profile != null)
		{
			_currentProfile = profile;
			return true;
		}
		return false;
	}

	public ProfileData GetProfile(string playerId, string systemId = null)
	{
		return _data.GetProfile(playerId, systemId);
	}

	public bool AddProfile(ProfileData profile)
	{
		if (profile != null && GetProfile(profile.PlayerGameId, profile.PlayerSystemId) == null)
		{
			return _data.AddProfile(profile);
		}
		return false;
	}

	public bool RemoveProfile(string playerId, string systemId = null)
	{
		ProfileData profile = GetProfile(playerId, systemId);
		if (profile != null)
		{
			return _data.RemoveProfile(profile);
		}
		return false;
	}

	public void ResetData()
	{
		_data.ResetData();
	}
}
