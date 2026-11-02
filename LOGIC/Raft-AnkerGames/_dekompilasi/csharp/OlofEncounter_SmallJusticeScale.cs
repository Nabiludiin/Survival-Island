using UnityEngine;

public class OlofEncounter_SmallJusticeScale : MonoBehaviour
{
	public GameObject realBridge;

	public GameObject fakeBridge;

	public void OnOlofCloseBridge()
	{
		fakeBridge.SetActiveSafe(value: false);
		realBridge.SetActiveSafe(value: true);
	}
}
