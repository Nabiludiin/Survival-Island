using UnityEngine;

namespace RG.Parsecs.Common;

public class MonoBehaviourExtended : MonoBehaviour
{
	public T GetManager<T>() where T : IBaseManager
	{
		if (LevelManager.Instance != null)
		{
			return LevelManager.Instance.GetManager<T>();
		}
		throw new ManagerNotFoundException(typeof(T), this);
	}
}
