using System;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

public class ParsecsNodeExecutionException : UnityException
{
	public ParsecsNodeExecutionException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public ParsecsNodeExecutionException(string message)
		: base(message)
	{
	}
}
