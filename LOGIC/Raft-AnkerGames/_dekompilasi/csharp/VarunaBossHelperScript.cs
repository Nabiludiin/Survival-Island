using System.Collections.Generic;
using UnityEngine;

public class VarunaBossHelperScript : MonoBehaviour
{
	public List<QuestEventBase> floorOnePillars = new List<QuestEventBase>();

	public List<QuestEventBase> floorTwoPillars = new List<QuestEventBase>();

	public List<QuestEventBase> floorThreePillars = new List<QuestEventBase>();

	public WaypointHandler waypointHandlerFloorOne;

	public WaypointHandler waypointHandlerFloorTwo;

	public WaypointHandler waypointHandlerFloorThree;
}
