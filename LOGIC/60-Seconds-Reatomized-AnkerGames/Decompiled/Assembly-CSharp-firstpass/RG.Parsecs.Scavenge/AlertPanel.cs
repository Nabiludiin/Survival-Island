using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class AlertPanel : MonoBehaviour
{
	public List<AlerPanelCorner> Corners;

	public void Stop()
	{
		foreach (AlerPanelCorner corner in Corners)
		{
			corner.Stop();
		}
	}
}
