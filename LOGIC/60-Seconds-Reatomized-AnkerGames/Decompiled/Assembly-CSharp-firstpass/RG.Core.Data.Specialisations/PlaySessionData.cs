using System;
using System.Collections.Generic;
using RG.Core.Data.Abstractions;
using RG.Core.Flow.Specialisations;

namespace RG.Core.Data.Specialisations;

[Serializable]
public class PlaySessionData : SessionData
{
	private List<SceneSession> _sceneSessions = new List<SceneSession>();

	private SceneSession _currentSceneSession;

	public SceneSession CurrentSceneSession
	{
		get
		{
			return _currentSceneSession;
		}
		set
		{
			_currentSceneSession = value;
		}
	}
}
