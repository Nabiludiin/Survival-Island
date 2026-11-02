using System;

namespace UnityEngine.AzureSky;

[Serializable]
public class GradientOutput
{
	public int gradientIndex;

	public Gradient[] gradientOutput = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};
}
