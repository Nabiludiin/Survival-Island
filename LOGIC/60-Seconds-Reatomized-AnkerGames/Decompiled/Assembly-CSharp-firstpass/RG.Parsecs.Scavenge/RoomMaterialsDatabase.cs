using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[CreateAssetMenu(fileName = "RoomMaterialsDatabase", menuName = "Scavenge/Rooms Materials Database")]
public class RoomMaterialsDatabase : ScriptableObject
{
	public List<Material> CommandCentre = new List<Material>();

	public List<Material> Cafeteria = new List<Material>();

	public List<Material> Bathroom = new List<Material>();

	public List<Material> ScienceRoom = new List<Material>();

	public List<Material> SleepingQuaters = new List<Material>();

	public List<Material> EngineeringRoom = new List<Material>();

	public List<Material> Tops = new List<Material>();

	internal Material GetMaterial(RoomType roomType)
	{
		Material material = null;
		material = roomType switch
		{
			RoomType.CommandCentre => CommandCentre.RandomElement(), 
			RoomType.Cafeteria => Cafeteria.RandomElement(), 
			RoomType.Bathroom => Bathroom.RandomElement(), 
			RoomType.ScienceRoom => ScienceRoom.RandomElement(), 
			RoomType.SleepingQuaters => SleepingQuaters.RandomElement(), 
			RoomType.EngineeringRoom => EngineeringRoom.RandomElement(), 
			_ => throw new NotImplementedException(), 
		};
		if (material == null)
		{
			Debug.LogErrorFormat("No material found for {0}.", roomType);
			return null;
		}
		return material;
	}
}
