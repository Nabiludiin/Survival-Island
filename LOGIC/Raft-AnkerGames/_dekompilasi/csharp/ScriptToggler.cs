using UnityEngine;

public class ScriptToggler : MonoBehaviour
{
	public MonoBehaviour[] scriptsToToggle;

	public GameObject[] gameObjectsToToggle;

	public KeyCode toggleKey;

	public bool active = true;

	private void Update()
	{
		if (Input.GetKeyDown(toggleKey))
		{
			active = !active;
			MonoBehaviour[] array = scriptsToToggle;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = active;
			}
			GameObject[] array2 = gameObjectsToToggle;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].SetActive(active);
			}
		}
	}
}
