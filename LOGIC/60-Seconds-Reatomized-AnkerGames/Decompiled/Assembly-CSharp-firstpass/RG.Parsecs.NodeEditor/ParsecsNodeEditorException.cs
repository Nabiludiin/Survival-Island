using UnityEngine;

namespace RG.Parsecs.NodeEditor;

public abstract class ParsecsNodeEditorException : UnityException
{
	protected ParsecsNodeEditorException(string message)
		: base(message)
	{
	}
}
