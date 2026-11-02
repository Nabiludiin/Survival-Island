public class AnimatorStorage
{
	public AnimatorParameterStorage[] storedParamaters;

	public AnimatorStorage(uint parameterCount)
	{
		if (parameterCount != 0)
		{
			storedParamaters = new AnimatorParameterStorage[parameterCount];
		}
	}
}
