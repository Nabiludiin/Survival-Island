using UnityEngine;

public struct AnimatorParameterStorage
{
	public AnimatorControllerParameterType type;

	public string paramaterName;

	public bool boolValue;

	public float floatValue;

	public int intValue;

	public void SaveParameter(AnimatorControllerParameter param, Animator anim)
	{
		paramaterName = param.name;
		type = param.type;
		switch (param.type)
		{
		case AnimatorControllerParameterType.Float:
			floatValue = anim.GetFloat(param.name);
			break;
		case AnimatorControllerParameterType.Int:
			intValue = anim.GetInteger(param.name);
			break;
		case AnimatorControllerParameterType.Bool:
			boolValue = anim.GetBool(param.name);
			break;
		case (AnimatorControllerParameterType)2:
			break;
		}
	}

	public void LoadParameter(Animator anim)
	{
		switch (type)
		{
		case AnimatorControllerParameterType.Float:
			anim.SetFloat(paramaterName, floatValue);
			break;
		case AnimatorControllerParameterType.Int:
			anim.SetInteger(paramaterName, intValue);
			break;
		case AnimatorControllerParameterType.Bool:
			anim.SetBool(paramaterName, boolValue);
			break;
		case (AnimatorControllerParameterType)2:
			break;
		}
	}
}
