using UnityEngine;

public class GizmoBoxConverter : MonoBehaviour
{
	public GameObject[] prefabsToFix;

	private void Start()
	{
	}

	public void Apply()
	{
		GameObject[] array = prefabsToFix;
		for (int i = 0; i < array.Length; i++)
		{
			_ = array[i];
		}
	}

	public void Convert()
	{
		GameObject[] array = prefabsToFix;
		for (int i = 0; i < array.Length; i++)
		{
			_ = array[i];
		}
	}
}
