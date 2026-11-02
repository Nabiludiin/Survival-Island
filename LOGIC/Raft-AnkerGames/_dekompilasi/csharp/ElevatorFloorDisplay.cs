using TMPro;
using UnityEngine;

public class ElevatorFloorDisplay : MonoBehaviour
{
	private TextMeshPro display;

	public void SetFloorIndex(int floorIndex)
	{
		if (display == null)
		{
			display = GetComponent<TextMeshPro>();
		}
		display.text = floorIndex.ToString();
	}
}
