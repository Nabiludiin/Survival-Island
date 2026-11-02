using System.Collections.Generic;

namespace RG_GameCamera.Config;

public class EmptyConfig : Config
{
	public override void LoadDefault()
	{
		Dictionary<string, Param> value = new Dictionary<string, Param> { 
		{
			"Orthographic",
			new BoolParam
			{
				value = false
			}
		} };
		Params = new Dictionary<string, Dictionary<string, Param>> { { "Default", value } };
		Transitions = new Dictionary<string, float>();
		foreach (KeyValuePair<string, Dictionary<string, Param>> item in Params)
		{
			Transitions.Add(item.Key, 0.25f);
		}
		Deserialize(DefaultConfigPath);
		base.LoadDefault();
	}

	protected override void Awake()
	{
		base.Awake();
		LoadDefault();
	}
}
