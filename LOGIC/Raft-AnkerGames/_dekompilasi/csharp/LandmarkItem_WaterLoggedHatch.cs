using System;
using UnityEngine;

public class LandmarkItem_WaterLoggedHatch : LandmarkItem_Quest
{
	public Collider ladderCollider;

	public FillContainerComponent fillComponent;

	[SerializeField]
	private GameObject waterPlane;

	[SerializeField]
	private ParticleSystem particles;

	public ObjectEnabler[] objEnablersToToggle;

	[SerializeField]
	private StudioEventEmitterSustain emitter_Hatch;

	[SerializeField]
	private StudioEventEmitterSustain emitter_WaterHitFloor;

	[HideInInspector]
	public Vector3 defaultLadderPos;

	private void Awake()
	{
		FillContainerComponent fillContainerComponent = fillComponent;
		fillContainerComponent.OnFilledCompletely = (Action)Delegate.Combine(fillContainerComponent.OnFilledCompletely, new Action(OnWaterFilled));
	}

	private void Start()
	{
		defaultLadderPos = ladderCollider.transform.position;
	}

	private void OnWaterFilled()
	{
		SimpleMonoBehaviourSingleton<HapticManager>.Instance.StopAtennuated(HapticEvent.TangaroaLidOpen);
		emitter_Hatch?.Stop();
		emitter_WaterHitFloor.Stop();
		StopParticles();
	}

	public override RGD_LandmarkItem GetSave()
	{
		return new RGD_LandmarkItem_WaterLoggedHatch(this, this);
	}

	public void HardStopEffectsViaRestore()
	{
		SimpleMonoBehaviourSingleton<HapticManager>.Instance.StopAtennuated(HapticEvent.TangaroaLidOpen);
		emitter_Hatch?.StopHard();
		emitter_WaterHitFloor?.StopHard();
		Invoke("StopParticles", 1f);
	}

	public void StopParticles()
	{
		particles?.Stop();
	}

	public void StartFilling()
	{
		fillComponent.SetNormalizedValue(1f, instant: false);
		waterPlane.SetActiveSafe(value: true);
	}

	public void HatchInteract()
	{
		StartFilling();
		if (objEnablersToToggle != null)
		{
			ObjectEnabler[] array = objEnablersToToggle;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DeactivateAllObject();
			}
		}
		SimpleMonoBehaviourSingleton<HapticManager>.Instance.StartAttenuated(HapticEvent.TangaroaLidOpen, emitter_Hatch.eventEmitter.transform.position);
		emitter_Hatch?.Play();
		emitter_WaterHitFloor?.Play();
		HideLadderClimbColliderViaPosition();
	}

	public void HideLadderClimbColliderViaPosition()
	{
		ladderCollider.transform.position = defaultLadderPos - Vector3.forward * 5f;
	}
}
