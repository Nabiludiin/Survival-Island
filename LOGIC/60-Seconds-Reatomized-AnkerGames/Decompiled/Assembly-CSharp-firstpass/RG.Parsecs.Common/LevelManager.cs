using UnityEngine;

namespace RG.Parsecs.Common;

public class LevelManager : MonoBehaviour, IBaseManager
{
	public static LevelManager Instance;

	private IBaseManager[] _managers;

	private void Awake()
	{
		Instance = this;
		_managers = GetComponentsInChildren<IBaseManager>();
	}

	public T GetManager<T>() where T : IBaseManager
	{
		for (int i = 0; i < _managers.Length; i++)
		{
			if (_managers[i] is T)
			{
				return (T)_managers[i];
			}
		}
		throw new ManagerNotFoundException(typeof(T), this);
	}
}
