using RG.Parsecs.Common;
using RG.Parsecs.Survival;
using Rewired;
using Rewired.Integration.UnityUI;
using UnityEngine;

public sealed class GamepadPointerInputModule : RewiredStandaloneInputModule
{
	private bool _wasPointer;

	public static GamepadPointerInputModule I { get; protected set; }

	protected override void Awake()
	{
		if (I != null)
		{
			Debug.LogError("More than one GamepadPointerInputModule in the scene.");
			return;
		}
		I = this;
		base.Awake();
	}

	protected override void OnDestroy()
	{
		if (I == this)
		{
			I = null;
		}
		base.OnDestroy();
	}

	public override void Process()
	{
		if (!ReInput.isReady || (!m_HasFocus && ShouldIgnoreEventsOnNoFocus()) || !enabled || !gameObject.activeInHierarchy)
		{
			return;
		}
		bool flag = SendUpdateEventToSelectedObject();
		if (eventSystem.sendNavigationEvents)
		{
			if (!flag)
			{
				flag |= SendMoveEventToSelectedObject();
			}
			if (!flag)
			{
				SendSubmitEventToSelectedObject();
			}
		}
		if (Singleton<HandCursor>.Instance.isGamepadMode && !Singleton<HandCursor>.Instance.isGamepadPointerMode)
		{
			if (_wasPointer)
			{
				_wasPointer = false;
				ForceMouseExit();
			}
		}
		else
		{
			_wasPointer = true;
			if (!ProcessTouchEvents() && isMouseSupported)
			{
				ProcessMouseEvents();
			}
		}
	}

	private void HandleMouseExit(int playerId, int pointerIndex)
	{
		MouseState mousePointerEventData = GetMousePointerEventData(playerId, pointerIndex);
		if (mousePointerEventData != null)
		{
			MouseButtonEventData eventData = mousePointerEventData.GetButtonState(0).eventData;
			HandlePointerExitAndEnter(eventData.buttonData, null);
		}
	}

	public void ForceMouseExit()
	{
		for (int i = 0; i < playerIds.Length; i++)
		{
			Player player = ReInput.players.GetPlayer(playerIds[i]);
			if (player != null && (!UsePlayingPlayersOnly || player.isPlaying))
			{
				int mouseInputSourceCount = GetMouseInputSourceCount(playerIds[i]);
				for (int j = 0; j < mouseInputSourceCount; j++)
				{
					HandleMouseExit(playerIds[i], j);
				}
			}
		}
	}
}
