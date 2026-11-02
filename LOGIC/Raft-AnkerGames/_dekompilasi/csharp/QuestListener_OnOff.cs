using UnityEngine;

public class QuestListener_OnOff : QuestListener
{
	public bool onState;

	public GameObject[] objs;

	protected override void OnQuestRestore()
	{
		base.OnQuestRestore();
		SetObjStates();
	}

	protected override void OnQuestComplete()
	{
		base.OnQuestComplete();
		SetObjStates();
	}

	public void SetObjStates()
	{
		if (objs != null && objs.Length != 0)
		{
			GameObject[] array = objs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActiveSafe(onState);
			}
		}
	}
}
