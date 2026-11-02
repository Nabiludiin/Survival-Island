using UnityEngine;

public class WaterFloatDomesticRigidbody : WaterFloatSemih_Rigidbody
{
	[SerializeField]
	private AI_NetworkBehaviour_Domestic domestic;

	protected override bool CanBeSetToSurface => domestic.transform.parent != domestic.lockedPivot;

	private void OnValidate()
	{
		if (domestic == null)
		{
			domestic = GetComponent<AI_NetworkBehaviour_Domestic>();
		}
	}

	protected override void Start()
	{
		base.Start();
		if (domestic == null)
		{
			domestic = GetComponent<AI_NetworkBehaviour_Domestic>();
		}
	}
}
