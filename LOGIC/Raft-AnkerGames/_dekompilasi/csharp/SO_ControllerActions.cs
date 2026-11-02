using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_ControllerActions", menuName = "SO_ControllerActions")]
public class SO_ControllerActions : ScriptableObject
{
	[Serializable]
	public struct Action
	{
		public string bindingString;

		public Sprite sprite;
	}

	private Dictionary<string, Sprite> dictionary;

	[SerializeField]
	private Action[] ActionSprites;

	public bool GetSprite(string name, out Sprite sprite)
	{
		switch (name)
		{
		case "Cross":
			name = "A";
			break;
		case "Circle":
			name = "B";
			break;
		case "L2":
			name = "LT";
			break;
		case "R2":
			name = "RT";
			break;
		case "Square":
			name = "X";
			break;
		case "Triangle":
			name = "Y";
			break;
		case "R3":
			name = "Right Stick Press";
			break;
		}
		if (dictionary == null)
		{
			dictionary = ActionSprites.ToDictionary((Action x) => x.bindingString, (Action x) => x.sprite);
		}
		return dictionary.TryGetValue(name, out sprite);
	}
}
