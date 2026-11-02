using UnityEngine;

[CreateAssetMenu(fileName = "SO_Data_ReflectiveRaycast", menuName = "Raft/Linecast/ReflectiveRayCastData")]
public class SO_Data_ReflectiveRaycast : ScriptableObject
{
	public enum ReflectionMode
	{
		OBJECTNORMAL,
		REFLECT
	}

	public float raySegmentMaxLength = 15f;

	public int maxBounces = 10;

	public float hitPointOffset = -0.05f;

	public ReflectionMode reflectionMode = ReflectionMode.REFLECT;

	public LayerMask layer_Hit;

	public LayerMask layer_Bounce;

	public LayerMask layer_Interact;

	public LayerMask layer_dealDamage;

	public bool isActiveDefault;

	public bool shouldInteract;

	public bool storeHits;

	public bool dealsDamage = true;

	public float dealDamageCooldown = 1f;

	public int damage = 3;
}
