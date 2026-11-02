using System;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[Serializable]
[CreateAssetMenu(fileName = "NewRoomData", menuName = "Scavenge/Room Data")]
public class RoomData : ScriptableObject
{
	public RoomShape RoomShape;

	public SceneField Scene;
}
