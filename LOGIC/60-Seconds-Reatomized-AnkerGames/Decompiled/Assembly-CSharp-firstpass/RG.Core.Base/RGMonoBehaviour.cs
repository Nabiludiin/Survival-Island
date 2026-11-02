using UnityEngine;

namespace RG.Core.Base;

public class RGMonoBehaviour : MonoBehaviour
{
	[SerializeField]
	private bool _persist;

	public bool Persist
	{
		get
		{
			return _persist;
		}
		protected set
		{
			_persist = value;
		}
	}

	private void Awake()
	{
		CustomAwake();
		if (_persist)
		{
			Object.DontDestroyOnLoad(gameObject);
		}
	}

	protected virtual void CustomAwake()
	{
	}

	private void Start()
	{
		CustomStart();
	}

	protected virtual void CustomStart()
	{
	}

	private void Update()
	{
		CustomUpdate();
	}

	protected virtual void CustomUpdate()
	{
	}

	private void OnDestroy()
	{
		CustomOnDestroy();
	}

	protected virtual void CustomOnDestroy()
	{
	}

	private void Reset()
	{
		CustomReset();
	}

	protected virtual void CustomReset()
	{
	}
}
