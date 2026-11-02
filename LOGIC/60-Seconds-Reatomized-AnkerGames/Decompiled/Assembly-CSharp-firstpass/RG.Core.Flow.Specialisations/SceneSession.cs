using RG.Core.Base;
using RG.Core.Data.Abstractions;
using RG.Core.Data.DataTypes;
using RG.Core.Data.Specialisations;
using RG.Core.Flow.Abstractions;

namespace RG.Core.Flow.Specialisations;

public abstract class SceneSession : RGMonoBehaviour, ISession, IDataOwner<SceneSessionData>
{
	public abstract SceneSessionData Data { protected get; set; }

	public DualDictionary Values => Data.Values;

	public abstract void StartSession();

	public abstract void EndSession();

	public void ResetData()
	{
		Data.ResetData();
	}
}
