using UnityEngine;

public class Network_Entity_Redirect : MonoBehaviour
{
	public Network_Entity entity;

	public void OnValidate()
	{
		if (entity == null)
		{
			entity = GetComponentInParent<Network_Entity>();
		}
	}
}
