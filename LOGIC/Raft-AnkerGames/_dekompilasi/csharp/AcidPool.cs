using FMODUnity;
using UnityEngine;

public class AcidPool : MonoBehaviour
{
	public ShrinkAndDestroy damageAreaShrink;

	public ShrinkAndDestroy particleShrink;

	[SerializeField]
	private StudioEventEmitter emitter;

	private bool stoppedSound;

	public void Update()
	{
		if (!stoppedSound && emitter != null && damageAreaShrink != null && damageAreaShrink.LifeTime < 1f)
		{
			stoppedSound = true;
			emitter.EventInstance.keyOff();
		}
	}
}
