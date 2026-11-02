using FMODUnity;
using UnityEngine;

public class Anchor : MonoBehaviour
{
	[SerializeField]
	private Transform reciever;

	[SerializeField]
	private WaterFloatSemih2 waterFloat;

	public EventReference er_waterSplash;

	private float previousY;

	private void Update()
	{
		float prevYPos = waterFloat.PrevYPos;
		float y = transform.position.y;
		if (y < prevYPos && previousY > prevYPos)
		{
			RuntimeManager.PlayOneShot(er_waterSplash, transform.position);
			ParticleManager.PlaySystem("WaterSplash_Hook", transform.position).transform.SetParent(SingletonGeneric<GameManager>.Singleton.lockedPivot);
		}
		previousY = y;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (reciever != null)
		{
			reciever.SendMessage("OnAnchorHit", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void SetReciever(Transform reciever)
	{
		this.reciever = reciever;
	}
}
