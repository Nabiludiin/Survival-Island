using RG.Core.Base;
using RG.Core.Data.Abstractions;
using RG.Core.Data.DataTypes;
using RG.Core.Data.Specialisations;
using RG.Core.Flow.Abstractions;

namespace RG.Core.Flow.Specialisations;

public abstract class PlaySession : RGMonoBehaviour, ISession, IDataOwner<PlaySessionData>
{
	public abstract PlaySessionData Data { protected get; set; }

	public DualDictionary Values => Data.Values;

	protected override void CustomAwake()
	{
		base.CustomAwake();
		Persist = true;
	}

	protected override void CustomStart()
	{
		base.CustomStart();
		RGMonoBehaviourSingleton<TimeManager>.TheInstance.AddTimer("PlayTimer", false, 0f, 1f);
	}

	public abstract void StartSession();

	public virtual void EndSession()
	{
		if (Data.CurrentSceneSession != null)
		{
			Data.CurrentSceneSession.EndSession();
		}
	}

	public void ResetData()
	{
		Data.ResetData();
	}
}
