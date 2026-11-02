using System;
using UnityEngine;

namespace RG.Parsecs.Common;

public class ManagerNotFoundException : RGException
{
	public Type ManagerType { get; set; }

	public UnityEngine.Object ParentObject { get; set; }

	public ManagerNotFoundException(Type managerType, UnityEngine.Object parentObject)
	{
		ManagerType = managerType;
		ParentObject = parentObject;
	}
}
