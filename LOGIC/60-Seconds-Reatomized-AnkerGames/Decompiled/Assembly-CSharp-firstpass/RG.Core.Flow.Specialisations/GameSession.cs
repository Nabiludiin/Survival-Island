using RG.Core.Base;
using RG.Core.Data.Abstractions;
using RG.Core.Data.DataTypes;
using RG.Core.Data.Specialisations;
using RG.Core.Flow.Abstractions;
using UnityEngine;

namespace RG.Core.Flow.Specialisations;

public class GameSession : RGMonoBehaviourSingleton<GameSession>, ISession, IDataOwner<GameSessionData>
{
	public virtual GameSessionData Data { protected get; set; }

	public DualDictionary Values => Data.Values;

	protected override void CustomAwake()
	{
		base.CustomAwake();
		Persist = true;
	}

	protected override void CustomStart()
	{
		base.CustomStart();
		RGMonoBehaviourSingleton<TimeManager>.TheInstance.AddTimer("GameTimer", false, 0f, 1f);
		StartSession();
	}

	private void OnApplicationQuit()
	{
		EndSession();
	}

	public virtual void StartSession()
	{
	}

	public virtual void EndSession()
	{
	}

	public virtual void EndGame()
	{
		if (Data.CurrentPlaySession != null)
		{
			EndPlaySession();
		}
		EndSession();
	}

	public bool SpawnPlaySession(GameObject sessionTemplate)
	{
		GameObject gameObject = Object.Instantiate(sessionTemplate, transform);
		bool flag = gameObject != null;
		if (flag)
		{
			Data.CurrentPlaySession = gameObject.GetComponent<PlaySession>();
		}
		return flag;
	}

	public bool StartPlaySession()
	{
		if (Data.CurrentPlaySession == null)
		{
			DebugManager.LogError("Play session to start not found.", this);
			return false;
		}
		Data.CurrentPlaySession.StartSession();
		return true;
	}

	public bool EndPlaySession()
	{
		if (Data.CurrentPlaySession == null)
		{
			DebugManager.LogError("Play session to end not found.", this);
			return false;
		}
		Data.CurrentPlaySession.EndSession();
		return true;
	}

	public void ResetData()
	{
		Data.ResetData();
	}
}
