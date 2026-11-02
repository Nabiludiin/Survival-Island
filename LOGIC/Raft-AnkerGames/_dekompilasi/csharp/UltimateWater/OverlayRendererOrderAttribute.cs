using System;

namespace UltimateWater;

public class OverlayRendererOrderAttribute : Attribute
{
	private readonly int _Priority;

	public int Priority => _Priority;

	public OverlayRendererOrderAttribute(int priority)
	{
		_Priority = priority;
	}
}
