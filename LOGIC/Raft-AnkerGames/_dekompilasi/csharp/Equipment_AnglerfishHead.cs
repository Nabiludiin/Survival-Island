using System;
using UnityEngine;

public class Equipment_AnglerfishHead : Equipment_Hat
{
	private Light lightSourceLight;

	protected override void Start()
	{
		base.Start();
		if (playerNetwork.NoteBookUI != null)
		{
			NoteBookUI noteBookUI = playerNetwork.NoteBookUI;
			noteBookUI.OnBookActiveChanged = (Action<bool>)Delegate.Combine(noteBookUI.OnBookActiveChanged, new Action<bool>(OnNoteBookToggle));
		}
	}

	private void OnNoteBookToggle(bool active)
	{
		if (lightSourceLight != null)
		{
			lightSourceLight.gameObject.SetActiveSafe(!active);
		}
	}

	public override void Initialize(Network_Player playerNetwork)
	{
		base.Initialize(playerNetwork);
		if (localModel != null)
		{
			lightSourceLight = localModel.GetComponentInChildren<Light>(includeInactive: true);
		}
		if (remoteModel != null && lightSourceLight == null)
		{
			lightSourceLight = remoteModel.GetComponentInChildren<Light>(includeInactive: true);
		}
		if (playerNetwork.IsLocalPlayer && lightSourceLight != null)
		{
			lightSourceLight.shadows = LightShadows.None;
		}
	}
}
