using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Common;

[CreateAssetMenu(menuName = "60 Parsecs!/New Available Content")]
public class AvailableContentsPacks : ScriptableObject
{
	[SerializeField]
	private List<ContentPack> _contentPacks = new List<ContentPack>();

	public List<ContentPack> ContentPacks => _contentPacks;

	public void AddEntry(ContentPack contentPack, bool overrideEntry = false)
	{
		if (overrideEntry)
		{
			if (RemovePack(contentPack.Name))
			{
				Debug.Log($"Successfully removed {contentPack.Name} from list of Contents.");
			}
		}
		else if (DoesContainPack(contentPack.Name))
		{
			throw new UnityException($"There already is an entry {contentPack.Name} in {name} object.");
		}
		_contentPacks.Add(contentPack);
	}

	private bool DoesContainPack(string gameContentPackName)
	{
		for (int i = 0; i < _contentPacks.Count; i++)
		{
			if (_contentPacks[i].Name.Equals(gameContentPackName))
			{
				return true;
			}
		}
		return false;
	}

	private bool RemovePack(string gameContentPackName)
	{
		if (_contentPacks == null)
		{
			return false;
		}
		for (int i = 0; i < _contentPacks.Count; i++)
		{
			if (_contentPacks[i].Name.Equals(gameContentPackName))
			{
				_contentPacks.Remove(_contentPacks[i]);
				return true;
			}
		}
		return false;
	}
}
