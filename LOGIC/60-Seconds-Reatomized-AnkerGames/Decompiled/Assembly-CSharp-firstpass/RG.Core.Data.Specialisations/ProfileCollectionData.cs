using System;
using System.Collections.Generic;
using RG.Core.Data.Abstractions;
using UnityEngine;

namespace RG.Core.Data.Specialisations;

[Serializable]
[CreateAssetMenu(fileName = "ProfileCollectionData", menuName = "RG/Data/Core/Profile/ProfileCollectionData", order = 1)]
public class ProfileCollectionData : DataObject
{
	[SerializeField]
	private List<ProfileData> _profiles = new List<ProfileData>();

	public bool AddProfile(ProfileData p)
	{
		return false;
	}

	public bool RemoveProfile(ProfileData p)
	{
		return false;
	}

	public ProfileData GetProfile(string playerId, string systemId = null)
	{
		for (int i = 0; i < _profiles.Count; i++)
		{
			bool num = systemId == null || systemId == _profiles[i].PlayerSystemId;
			bool flag = playerId == _profiles[i].PlayerGameId;
			if (num & flag)
			{
				return _profiles[i];
			}
		}
		return null;
	}

	public override void ResetData()
	{
		_profiles.Clear();
	}
}
