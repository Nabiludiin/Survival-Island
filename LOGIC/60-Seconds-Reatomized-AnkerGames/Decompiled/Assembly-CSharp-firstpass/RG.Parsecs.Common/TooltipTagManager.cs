using System;
using System.Collections.Generic;
using RG.Core.Base;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Common;

[Serializable]
public class TooltipTagManager : RGMonoBehaviour
{
	[SerializeField]
	private TooltipTagData _tagData;

	public List<SaveEvent> SaveEventList = new List<SaveEvent>();

	private static TooltipTagManager _instance;

	public static TooltipTagManager Instance
	{
		get
		{
			if (_instance == null)
			{
				FindInstanceInScene();
			}
			return _instance;
		}
	}

	public TooltipTagData TagData => _tagData;

	private static void FindInstanceInScene()
	{
		_instance = UnityEngine.Object.FindObjectOfType<TooltipTagManager>();
		if (_instance == null)
		{
			Debug.LogError("No TooltipTagManager in scene!");
		}
	}

	protected override void CustomAwake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else if (_instance != this)
		{
			Debug.LogFormat("Duplicate instance of TooltipTagManager detected! Removing {0}", gameObject.name);
			UnityEngine.Object.Destroy(this);
		}
		_tagData.ResetTags();
	}

	public void OnEnable()
	{
		RegisterDevToolsCommands();
	}

	public void OnDisable()
	{
		UnregisterDevToolsCommands();
	}

	private void RegisterDevToolsCommands()
	{
	}

	private void UnregisterDevToolsCommands()
	{
	}
}
