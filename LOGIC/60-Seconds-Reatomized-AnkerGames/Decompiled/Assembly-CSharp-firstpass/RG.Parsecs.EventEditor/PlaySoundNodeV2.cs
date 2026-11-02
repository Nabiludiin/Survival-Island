using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Utility Nodes/Play Sound Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ReportEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class PlaySoundNodeV2 : FlowNode
{
	private const int INTPUT_IN_INDEX = 0;

	private const int OUTTPUT_IN_INDEX = 0;

	private const string NODE_NAME = "Play Sound";

	[EventRef]
	[SerializeField]
	private string _event;

	[SerializeField]
	[Range(0f, 1f)]
	private float _volume = 0.5f;

	[SerializeField]
	[Range(-1f, 1f)]
	private float _pan;

	[SerializeField]
	[Range(-1f, 1f)]
	private float _pitch;

	[SerializeField]
	private int _offset;

	[SerializeField]
	private bool _offsetCheck;

	private const string ERROR_INFO = "No event selected.";

	private const string WARNING_INFO = "No offeset time was set.";

	private const string VALUE_PITCH = "Pitch";

	private const string VALUE_PAN = "Pan";

	public const string ID = "EE_PlaySoundNodeV2";

	public override string GetID => "EE_PlaySoundNodeV2";

	public override Node Create(Vector2 pos)
	{
		PlaySoundNodeV2 playSoundNodeV = ScriptableObject.CreateInstance<PlaySoundNodeV2>();
		playSoundNodeV.rect = new Rect(pos.x, pos.y, 310f, 270f);
		playSoundNodeV.name = "Play Sound";
		playSoundNodeV.CreateMutliInput("In", "Flow");
		playSoundNodeV.CreateOutput("Out", "Flow");
		return playSoundNodeV;
	}

	public override Node Duplicate(Vector2 pos)
	{
		PlaySoundNodeV2 playSoundNodeV = (PlaySoundNodeV2)Create(rect.position + new Vector2(20f, 20f));
		playSoundNodeV._event = _event;
		playSoundNodeV._volume = _volume;
		playSoundNodeV._pan = _pan;
		playSoundNodeV._pitch = _pitch;
		playSoundNodeV._offset = _offset;
		return playSoundNodeV;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	public IEnumerator PlaySound()
	{
		EventInstance playEvent = RuntimeManager.CreateInstance(_event);
		playEvent.setParameterByName("Pitch", _pitch);
		playEvent.setParameterByName("Pan", _pan);
		playEvent.setVolume(_volume);
		if (_offsetCheck)
		{
			yield return new WaitForSeconds(_offset);
			playEvent.start();
			yield return null;
		}
		else
		{
			playEvent.start();
		}
	}

	public override void Execute(NodeCanvas canvas)
	{
		Singleton<GameManager>.Instance.PlaySoundInvoke(PlaySound());
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
