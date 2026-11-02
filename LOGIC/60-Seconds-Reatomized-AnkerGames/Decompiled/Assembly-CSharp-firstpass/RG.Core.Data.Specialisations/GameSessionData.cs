using System;
using RG.Core.Data.Abstractions;
using RG.Core.Flow.Specialisations;

namespace RG.Core.Data.Specialisations;

[Serializable]
public abstract class GameSessionData : SessionData
{
	private PlaySession _playSession;

	public PlaySession CurrentPlaySession
	{
		get
		{
			return _playSession;
		}
		set
		{
			_playSession = value;
		}
	}
}
