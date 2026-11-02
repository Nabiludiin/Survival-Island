using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RG.Parsecs.Menu;

internal class ResolutionWithoutRefreshRate : IEquatable<ResolutionWithoutRefreshRate>
{
	public int width;

	public int height;

	public override string ToString()
	{
		return width + " x " + height;
	}

	public ResolutionWithoutRefreshRate(int widht, int height)
	{
		width = widht;
		this.height = height;
	}

	public ResolutionWithoutRefreshRate(Resolution r)
	{
		width = r.width;
		height = r.height;
	}

	public Resolution ToResolution()
	{
		List<Resolution> list = new List<Resolution>();
		Resolution[] resolutions = Screen.resolutions;
		foreach (Resolution resolution in resolutions)
		{
			if (Equals(resolution))
			{
				list.Add(resolution);
			}
		}
		list = list.OrderByDescending((Resolution resolution2) => resolution2.refreshRate).ToList();
		return list.FirstOrDefault();
	}

	public bool Equals(ResolutionWithoutRefreshRate other)
	{
		if (width == other.width && height == other.height)
		{
			return true;
		}
		return false;
	}

	public bool Equals(Resolution other)
	{
		if (width == other.width && height == other.height)
		{
			return true;
		}
		return false;
	}
}
