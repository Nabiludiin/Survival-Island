using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NodeEditorFramework;

public class NodeCanvasManager
{
	public static Dictionary<Type, NodeCanvasTypeData> CanvasTypes;

	private static Action<NodeCanvasTypeData> _callBack;

	public static void GetAllCanvasTypes()
	{
		CanvasTypes = new Dictionary<Type, NodeCanvasTypeData>();
		foreach (Assembly item in from assembly in AppDomain.CurrentDomain.GetAssemblies()
			where assembly.FullName.Contains("Assembly")
			select assembly)
		{
			foreach (Type item2 in from T in item.GetTypes()
				where T.IsClass && !T.IsAbstract && T.GetCustomAttributes(typeof(NodeCanvasTypeAttribute), inherit: false).Length != 0
				select T)
			{
				NodeCanvasTypeAttribute nodeCanvasTypeAttribute = item2.GetCustomAttributes(typeof(NodeCanvasTypeAttribute), inherit: false)[0] as NodeCanvasTypeAttribute;
				CanvasTypes.Add(item2, new NodeCanvasTypeData
				{
					CanvasType = item2,
					DisplayString = nodeCanvasTypeAttribute.Name,
					WindowTitle = nodeCanvasTypeAttribute.WindowTitle,
					DefaultFileName = nodeCanvasTypeAttribute.DefaultFileName,
					SaveFileMessage = nodeCanvasTypeAttribute.SaveFileMessage
				});
			}
		}
	}

	private static void CreateNewCanvas(object userdata)
	{
		NodeCanvasTypeData obj = (NodeCanvasTypeData)userdata;
		_callBack(obj);
	}

	public static bool CheckCanvasCompability(Node node, Type canvasType)
	{
		NodeData nodeData = NodeTypes.getNodeData(node);
		if (nodeData.limitToCanvasTypes != null && nodeData.limitToCanvasTypes.Length != 0)
		{
			return nodeData.limitToCanvasTypes.Contains(canvasType);
		}
		return true;
	}
}
