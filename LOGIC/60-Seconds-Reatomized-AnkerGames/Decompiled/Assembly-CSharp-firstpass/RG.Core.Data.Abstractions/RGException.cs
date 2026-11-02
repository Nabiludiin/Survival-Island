using System;
using UnityEngine;

namespace RG.Core.Data.Abstractions;

public class RGException : UnityException
{
	public RGException(string msg)
		: base(msg)
	{
	}

	public RGException(string msg, Exception innerException)
		: base(msg, innerException)
	{
	}
}
