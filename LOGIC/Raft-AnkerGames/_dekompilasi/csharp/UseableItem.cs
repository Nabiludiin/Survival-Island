using System;
using UnityEngine;

public class UseableItem : UsableTool
{
	protected Action OnChannelFinished;

	protected bool isChanellingItem;

	[SerializeField]
	private float originItemChannelTime = 1f;

	[SerializeField]
	private bool useCanvasLoadCircle = true;

	[SerializeField]
	private bool useDelayBetweenChannels;

	[SerializeField]
	private float channelDelay = 0.5f;

	protected CanvasHelper canvas;

	protected float currentItemChannelTime;

	private bool canChannel = true;

	protected bool IsChannelFinished => currentItemChannelTime >= originItemChannelTime;

	protected override void Start()
	{
		base.Start();
		if (playerNetwork.IsLocalPlayer)
		{
			canvas = ComponentManager<CanvasHelper>.Value;
			OnReleaseUseButton = (Action)Delegate.Combine(OnReleaseUseButton, new Action(ResetItemChannel));
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		OnReleaseUseButton = (Action)Delegate.Remove(OnReleaseUseButton, new Action(ResetItemChannel));
	}

	protected override bool Update()
	{
		isChanellingItem = false;
		if (base.Update() && isHoldingUseButton && canChannel)
		{
			ChannelItem();
		}
		return true;
	}

	protected override void OnToolSelect()
	{
		base.OnToolSelect();
		canChannel = true;
	}

	protected override void OnToolDeSelect()
	{
		base.OnToolDeSelect();
		isChanellingItem = false;
		canChannel = true;
		currentItemChannelTime = 0f;
		if (canvas != null)
		{
			canvas.SetLoadCircle(state: false);
		}
		if (IsInvoking("HandleChannelDelay"))
		{
			CancelInvoke("HandleChannelDelay");
		}
	}

	public virtual bool ChannelItem()
	{
		if (currentItemChannelTime <= 0f)
		{
			OnStartChanneling();
		}
		currentItemChannelTime += Time.deltaTime;
		isChanellingItem = true;
		UpdateLoadCircle();
		if (IsChannelFinished)
		{
			if (useDelayBetweenChannels)
			{
				canChannel = false;
				Invoke("HandleChannelDelay", channelDelay);
			}
			OnChannelFinished.CallSafe();
			ResetItemChannel();
		}
		return true;
	}

	protected virtual void OnStartChanneling()
	{
	}

	protected virtual void ResetItemChannel()
	{
		currentItemChannelTime = 0f;
		UpdateLoadCircle();
		HideLoadCircle();
	}

	protected virtual void HandleChannelDelay()
	{
		canChannel = true;
	}

	private void UpdateLoadCircle()
	{
		if (useCanvasLoadCircle && canvas != null)
		{
			canvas.SetLoadCircle(Helper.NormalizeValue(currentItemChannelTime, 0f, originItemChannelTime));
		}
	}

	private void HideLoadCircle()
	{
		if (useCanvasLoadCircle && canvas != null)
		{
			canvas.SetLoadCircle(state: false);
		}
	}
}
