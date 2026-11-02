using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFOVManager : MonoBehaviour
{
	private List<Transform> fovChildren = new List<Transform>();

	private Network_Player player;

	public void Initialize()
	{
		transform.FindChildrenByLayer(ref fovChildren, LayerMasks.MASK_HandCamera.value);
		player = GetComponent<Network_Player>();
		if (player.IsLocalPlayer)
		{
			ThirdPerson thirdPersonSettings = player.currentModel.thirdPersonSettings;
			thirdPersonSettings.OnThirdpersonModelChange = (Action<bool>)Delegate.Combine(thirdPersonSettings.OnThirdpersonModelChange, new Action<bool>(OnThirdPersonModelChange));
		}
		else
		{
			SetFovChildrenLayer(LayerMasks.MASK_Default.value);
		}
	}

	private void OnDestroy()
	{
		if (player != null && player.IsLocalPlayer)
		{
			ThirdPerson thirdPersonSettings = player.currentModel.thirdPersonSettings;
			thirdPersonSettings.OnThirdpersonModelChange = (Action<bool>)Delegate.Remove(thirdPersonSettings.OnThirdpersonModelChange, new Action<bool>(OnThirdPersonModelChange));
		}
	}

	private void OnThirdPersonModelChange(bool thirdPerson)
	{
		if (thirdPerson)
		{
			SetFovChildrenLayer(LayerMasks.MASK_Default.value);
		}
		else
		{
			SetFovChildrenLayer(LayerMasks.MASK_HandCamera.value);
		}
	}

	private void SetFovChildrenLayer(int layer)
	{
		if (fovChildren == null)
		{
			return;
		}
		for (int i = 0; i < fovChildren.Count; i++)
		{
			if (fovChildren[i] == null)
			{
				fovChildren.RemoveAt(i);
				i--;
			}
			else
			{
				fovChildren[i].gameObject.layer = layer;
			}
		}
	}
}
