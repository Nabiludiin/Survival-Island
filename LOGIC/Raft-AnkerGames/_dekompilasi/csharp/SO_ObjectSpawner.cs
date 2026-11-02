using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSpawner", menuName = "Raft/Miscellaneous/ObjectSpawner", order = 1)]
public class SO_ObjectSpawner : ScriptableObject
{
	[SerializeField]
	private ObjectSpawnerAssetSettings[] settings;

	public ObjectSpawnerAssetSettings GetSettings(int playerCount)
	{
		ObjectSpawnerAssetSettings objectSpawnerAssetSettings = settings.FirstOrDefault((ObjectSpawnerAssetSettings s) => !s.fallback && s.PlayerCountWithinRange(playerCount));
		if (objectSpawnerAssetSettings == null)
		{
			objectSpawnerAssetSettings = settings.FirstOrDefault((ObjectSpawnerAssetSettings s) => s.fallback);
		}
		return objectSpawnerAssetSettings;
	}
}
