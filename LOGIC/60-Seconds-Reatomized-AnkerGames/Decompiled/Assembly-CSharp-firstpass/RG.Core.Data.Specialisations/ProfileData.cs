using System;
using System.Collections.Generic;
using RG.Core.Data.Abstractions;
using UnityEngine;

namespace RG.Core.Data.Specialisations;

[Serializable]
[CreateAssetMenu(fileName = "ProfileData", menuName = "RG/Data/Core/Profile/ProfileData", order = 1)]
public class ProfileData : DataObject, ISerializable
{
	[SerializeField]
	private string _playerGameId = string.Empty;

	[SerializeField]
	private string _playerSystemId = string.Empty;

	[SerializeField]
	private PlayerStatsData _stats;

	[SerializeField]
	private Dictionary<string, object> _values = new Dictionary<string, object>();

	[SerializeField]
	private List<UnlockData> _unlocks = new List<UnlockData>();

	public string PlayerGameId => _playerGameId;

	public string PlayerSystemId => _playerSystemId;

	public override void ResetData()
	{
		_playerGameId = string.Empty;
		_playerSystemId = string.Empty;
		_stats.ResetData();
		_unlocks.Clear();
		_values.Clear();
	}

	public void Load()
	{
		throw new NotImplementedException();
	}

	public void Save()
	{
		throw new NotImplementedException();
	}
}
