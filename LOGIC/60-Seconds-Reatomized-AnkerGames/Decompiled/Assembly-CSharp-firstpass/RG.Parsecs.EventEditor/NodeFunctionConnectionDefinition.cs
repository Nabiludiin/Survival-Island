using System;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Serializable]
public class NodeFunctionConnectionDefinition
{
	public enum EConnectionDefinitionMatchState
	{
		MATCHING,
		NOT_FOUND,
		DIFFERENT_TYPE
	}

	[SerializeField]
	[Tooltip("Name of the connection.")]
	private string _connectionName;

	[SerializeField]
	[Tooltip("Type of the connection..")]
	private string _connectionTypeId = "Int";

	public object RuntimeValue { get; set; }

	public string ConnectionName
	{
		get
		{
			return _connectionName;
		}
		set
		{
			_connectionName = value;
		}
	}

	public string ConnectionTypeId
	{
		get
		{
			return _connectionTypeId;
		}
		set
		{
			_connectionTypeId = value;
		}
	}
}
