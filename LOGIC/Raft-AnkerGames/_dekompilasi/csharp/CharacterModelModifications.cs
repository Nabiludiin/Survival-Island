using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterModelModifications : MonoBehaviour
{
	public VoiceType voiceType = VoiceType.Maya;

	public RuntimeAnimatorController thirdPersonController;

	public RuntimeAnimatorController firstPersonController;

	[SerializeField]
	private Avatar avatar;

	public SkinnedMeshRenderer fullBodyMesh;

	[SerializeField]
	private GameObject fullBodyMeshHazmat;

	[SerializeField]
	private SkinnedMeshRenderer armMesh;

	[SerializeField]
	private GameObject armMeshHazmat;

	public SkinnedMeshRenderer[] hairMeshes;

	[FormerlySerializedAs("colorSchemes")]
	public Outfit[] outfits;

	public HairStyleConnection[] hairStyles;

	private HairStyle currentHairStyle = HairStyle.Full;

	private FOVStyle currentFOVStyle;

	public ThirdPerson thirdPersonSettings;

	public Transform skeletonBone;

	public Transform headBone;

	public Transform leftHandItemHolder;

	public Transform rightHandItemHolder;

	public Transform cameraHolder;

	public Transform ropeConnectTransform;

	public Transform hatParent;

	public BoxCollider incapacitatedCollider;

	public BoxCollider[] entityColliders;

	private Vector3 originalSkeletonPos = Vector3.zero;

	private Vector3 originalCameraHolderPos = Vector3.zero;

	private Network_Player playerNetwork;

	private Settings settings;

	public HairStyle CurrentHairStyle => currentHairStyle;

	private void CopyBodyMaterialToArm()
	{
		Outfit[] array = outfits;
		foreach (Outfit obj in array)
		{
			obj.fpArmMaterial = obj.bodyMaterial;
		}
	}

	private void Start()
	{
		settings = ComponentManager<Settings>.Value;
		playerNetwork = GetComponentInParent<Network_Player>();
		Animator componentInParent = GetComponentInParent<Animator>();
		if (playerNetwork != null && componentInParent != null)
		{
			if (!playerNetwork.IsLocalPlayer)
			{
				componentInParent.runtimeAnimatorController = thirdPersonController;
			}
			componentInParent.avatar = avatar;
			if (playerNetwork.IsLocalPlayer)
			{
				ThirdPerson thirdPerson = playerNetwork.currentModel.thirdPersonSettings;
				thirdPerson.OnThirdpersonModelChange = (Action<bool>)Delegate.Combine(thirdPerson.OnThirdpersonModelChange, new Action<bool>(ThirdPersonStateChange));
				armMesh.gameObject.SetActive(value: true);
				fullBodyMesh.gameObject.SetActive(value: false);
			}
			else
			{
				fullBodyMesh.gameObject.SetActive(value: true);
				armMesh.gameObject.SetActive(value: false);
			}
			if (playerNetwork != null && playerNetwork.IsLocalPlayer)
			{
				ActivateHairStyle(HairStyle.Full, useAsCurrentHairStyle: true);
			}
		}
		originalCameraHolderPos = cameraHolder.localPosition;
		originalSkeletonPos = skeletonBone.localPosition;
	}

	private void OnDestroy()
	{
		playerNetwork = null;
	}

	private void Update()
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (!(settings != null) || !(playerNetwork != null) || !playerNetwork.IsLocalPlayer)
		{
			return;
		}
		switch (currentFOVStyle)
		{
		case FOVStyle.CurrentSetting:
		{
			float value = settings.graphicsBox.FOVSlider.value;
			if (value != playerNetwork.Camera.fieldOfView)
			{
				playerNetwork.Camera.fieldOfView = value;
			}
			break;
		}
		case FOVStyle.Default:
			if (playerNetwork.Camera.fieldOfView != 50f)
			{
				playerNetwork.Camera.fieldOfView = 50f;
			}
			break;
		}
	}

	private void ThirdPersonStateChange(bool newThirdPersonState)
	{
		if (!playerNetwork.HazmatSuit.Equipped)
		{
			if (!newThirdPersonState)
			{
				ActivateHairStyle(HairStyle.Bald, useAsCurrentHairStyle: false);
			}
			else
			{
				ActivateHairStyle(currentHairStyle, useAsCurrentHairStyle: false);
			}
		}
	}

	public bool ApplyOutfit(int outfitIndex)
	{
		if (outfitIndex < 0 || outfitIndex >= outfits.Length)
		{
			return false;
		}
		Outfit outfit = outfits[outfitIndex];
		if (outfit == null)
		{
			return false;
		}
		outfit.ApplyOutfit(fullBodyMesh, armMesh, hairMeshes);
		return true;
	}

	public void ActivateHairStyle(HairStyle style, bool useAsCurrentHairStyle)
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		HairStyleConnection hairStyleConnection = null;
		HairStyleConnection[] array = hairStyles;
		foreach (HairStyleConnection hairStyleConnection2 in array)
		{
			if (hairStyleConnection2.hairStyle == style)
			{
				hairStyleConnection = hairStyleConnection2;
			}
			hairStyleConnection2.SetState(state: false);
		}
		if (!playerNetwork.IsLocalPlayer || playerNetwork.currentModel.thirdPersonSettings.ThirdPersonModel)
		{
			hairStyleConnection?.SetState(state: true);
		}
		if (useAsCurrentHairStyle && hairStyleConnection != null)
		{
			currentHairStyle = hairStyleConnection.hairStyle;
		}
	}

	public void SetFOVStyle(FOVStyle style)
	{
		if (currentFOVStyle != style)
		{
			currentFOVStyle = style;
		}
	}

	public void UpdateArmAndBodyMesh()
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (!playerNetwork.IsLocalPlayer)
		{
			SetFullBodyMeshState(state: true);
			SetArmMeshState(state: false);
		}
		else if (thirdPersonSettings.ThirdPersonModel)
		{
			SetFullBodyMeshState(state: true);
			SetArmMeshState(state: false);
		}
		else
		{
			SetFullBodyMeshState(state: false);
			SetArmMeshState(state: true);
		}
	}

	public void SetFullBodyMeshState(bool state)
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (state)
		{
			bool equipped = playerNetwork.HazmatSuit.Equipped;
			fullBodyMeshHazmat.gameObject.SetActiveSafe(equipped);
			fullBodyMesh.gameObject.SetActiveSafe(!equipped);
		}
		else
		{
			fullBodyMeshHazmat.gameObject.SetActiveSafe(value: false);
			fullBodyMesh.gameObject.SetActiveSafe(value: false);
		}
	}

	public void SetArmMeshState(bool state)
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (state)
		{
			bool equipped = playerNetwork.HazmatSuit.Equipped;
			armMeshHazmat.gameObject.SetActiveSafe(equipped);
			armMesh.gameObject.SetActiveSafe(!equipped);
		}
		else
		{
			armMesh.gameObject.SetActiveSafe(value: false);
			armMeshHazmat.gameObject.SetActiveSafe(value: false);
		}
	}
}
