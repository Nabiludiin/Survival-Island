using System;
using UnityEngine;

[Serializable]
public class QuestInteractable_ComponentData_Animation : QuestInteractable_ComponentData
{
	public AnimatorPropertySetting_Legit animationSettings = new AnimatorPropertySetting_Legit();

	public Coroutine lerpFloatRoutine;
}
