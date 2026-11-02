using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class JusticeScale : MonoBehaviour_ID_Network
{
	[SerializeField]
	private Transform scale1;

	[SerializeField]
	private Transform scale2;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private GameObject cogwheelStack;

	[SerializeField]
	private Transform destroyWeightPoint;

	[SerializeField]
	private ScaleWeight weightPrefab;

	[SerializeField]
	private Transform[] weightSlots;

	[SerializeField]
	private Mesh[] weightMeshes;

	[SerializeField]
	private int weight1;

	[SerializeField]
	private int weight2 = 2;

	[SerializeField]
	private int startWeight2;

	[SerializeField]
	private float maxWeight = 1f;

	[SerializeField]
	private float speed = 3f;

	[SerializeField]
	private float floorHeight = 3f;

	[SerializeField]
	private float weightTravelTime = 0.5f;

	[SerializeField]
	private bool isMoving;

	[SerializeField]
	private StudioEventEmitter emitter_lift1;

	[SerializeField]
	private StudioEventEmitter emitter_lift2;

	[SerializeField]
	private SoundPlayer_IntegerConnection soundPlayer;

	private InteractableButton_Network_Forwarder buttonForwarder;

	private bool changingWeight;

	public bool CanAddWeight
	{
		get
		{
			if (!isMoving)
			{
				return !changingWeight;
			}
			return false;
		}
	}

	private void Awake()
	{
		buttonForwarder = new InteractableButton_Network_Forwarder(GetComponentsInChildren<InteractableButton_Networked>(includeInactive: true));
	}

	private void Update()
	{
		bool flag = isMoving;
		isMoving = !UpdatePlatformPosition(instant: false);
		if (animator != null)
		{
			animator.speed = (isMoving ? 1f : 0f);
		}
		if (!flag && isMoving)
		{
			if (emitter_lift1 != null)
			{
				emitter_lift1.Stop();
				emitter_lift1.PlaySafe();
			}
			if (emitter_lift2 != null)
			{
				emitter_lift2.Stop();
				emitter_lift2.PlaySafe();
			}
		}
		else if (!isMoving)
		{
			if (emitter_lift1 != null && emitter_lift1.IsPlaying())
			{
				emitter_lift1.EventInstance.keyOff();
			}
			if (emitter_lift2 != null && emitter_lift2.IsPlaying())
			{
				emitter_lift2.EventInstance.keyOff();
			}
		}
	}

	private void UpdateButtonMeshes()
	{
		if (Application.isPlaying)
		{
			return;
		}
		buttonForwarder = new InteractableButton_Network_Forwarder(GetComponentsInChildren<InteractableButton_Networked>(includeInactive: true));
		buttonForwarder.buttons = GetComponentsInChildren<InteractableButton_Networked>(includeInactive: true);
		int num = 0;
		InteractableButton_Networked[] buttons = buttonForwarder.buttons;
		for (int i = 0; i < buttons.Length; i++)
		{
			InteractableButton_Networked_JusticeScale interactableButton_Networked_JusticeScale = buttons[i] as InteractableButton_Networked_JusticeScale;
			if (!(interactableButton_Networked_JusticeScale == null))
			{
				interactableButton_Networked_JusticeScale.buttonIndex = num++;
				MeshFilter componentInChildren = interactableButton_Networked_JusticeScale.GetComponentInChildren<MeshFilter>(includeInactive: true);
				if (componentInChildren != null)
				{
					SetWeightMeshOnFilter(componentInChildren, interactableButton_Networked_JusticeScale.weight - 1);
					ReApplyBoxCollider(componentInChildren.transform);
				}
				interactableButton_Networked_JusticeScale.transform.name = "ButtonIndex = " + interactableButton_Networked_JusticeScale.buttonIndex + ", Weight = " + interactableButton_Networked_JusticeScale.weight;
			}
		}
		Debug.Log("Updated button meshes");
	}

	private bool UpdatePlatformPosition(bool instant)
	{
		weight2 = startWeight2 + (cogwheelStack.activeSelf ? 1 : 0);
		float num = Mathf.Clamp(-(weight1 - weight2), 0f - maxWeight, maxWeight);
		float num2 = Mathf.Clamp(-(weight2 - weight1), 0f - maxWeight, maxWeight);
		Vector3 vector = new Vector3(scale1.localPosition.x, num * floorHeight, scale1.localPosition.z);
		Vector3 vector2 = new Vector3(scale2.localPosition.x, num2 * floorHeight, scale2.localPosition.z);
		if (!instant)
		{
			scale1.localPosition = Vector3.MoveTowards(scale1.localPosition, vector, Time.deltaTime * speed);
			scale2.localPosition = Vector3.MoveTowards(scale2.localPosition, vector2, Time.deltaTime * speed);
		}
		else
		{
			scale1.localPosition = vector;
			scale2.localPosition = vector2;
		}
		if (scale1.localPosition == vector)
		{
			return scale2.localPosition == vector2;
		}
		return false;
	}

	private bool ButtonPress(InteractableButton_Networked button)
	{
		if (button.buttonIndex >= 100)
		{
			int num = button.buttonIndex / 100 - 1;
			if (!weightSlots.IsIndexWithinRange(num))
			{
				return false;
			}
			Transform transform = weightSlots[num];
			if (transform == null || transform.childCount == 0)
			{
				return false;
			}
			ScaleWeight componentInChildren = transform.GetComponentInChildren<ScaleWeight>();
			if (componentInChildren == null)
			{
				return false;
			}
			CoroutineManager.Singleton.AddCoroutine(RemoveWeight(componentInChildren, weightTravelTime));
			return true;
		}
		InteractableButton_Networked_JusticeScale interactableButton_Networked_JusticeScale = button as InteractableButton_Networked_JusticeScale;
		if (interactableButton_Networked_JusticeScale == null || interactableButton_Networked_JusticeScale.buttonIndex < 0)
		{
			return false;
		}
		if (Raft_Network.IsHost)
		{
			if (!CanAddWeight)
			{
				return false;
			}
			if (Mathf.Abs(interactableButton_Networked_JusticeScale.transform.position.y - scale1.position.y) >= floorHeight / 2f)
			{
				return false;
			}
		}
		Transform transform2 = FindEmptyWeightSlot();
		if (transform2 != null)
		{
			CoroutineManager.Singleton.AddCoroutine(AddWeight(transform2.InverseTransformPoint(interactableButton_Networked_JusticeScale.transform.position), transform2, weightTravelTime, interactableButton_Networked_JusticeScale.weight));
			return true;
		}
		return false;
	}

	private InteractableButton_Networked GetButtonFromIndex(int buttonIndex)
	{
		if (buttonForwarder != null && buttonForwarder.buttons != null)
		{
			for (int i = 0; i < buttonForwarder.buttons.Length; i++)
			{
				InteractableButton_Networked interactableButton_Networked = buttonForwarder.buttons[i];
				if (!(interactableButton_Networked == null) && interactableButton_Networked.buttonIndex == buttonIndex)
				{
					return interactableButton_Networked;
				}
			}
		}
		return null;
	}

	private IEnumerator AddWeight(Vector3 localStartPosition, Transform slot, float travelTime, int weight)
	{
		changingWeight = true;
		ScaleWeight newWeight = Object.Instantiate(weightPrefab);
		newWeight.transform.SetParentSafe(slot);
		newWeight.transform.localPosition = localStartPosition;
		newWeight.Initialize(slot.GetComponent<RaycastInteractable>(), weight);
		soundPlayer.PlaySoundRetrieval(0, newWeight.transform.position);
		SetWeightMeshOnFilter(newWeight.GetComponent<MeshFilter>(), weight - 1);
		BoxCollider newButtonCollider = ReApplyBoxCollider(newWeight.transform);
		newButtonCollider.enabled = false;
		Vector3 targetScale = Vector3.one;
		if (travelTime > 0f)
		{
			float timer = 0f;
			float normalizedTime = 0f;
			while (normalizedTime < 1f)
			{
				timer += Time.deltaTime;
				normalizedTime = Mathf.Clamp01(timer / travelTime);
				newWeight.transform.localPosition = Helper.GetBezierCurveValue(localStartPosition, Vector3.zero, 2f, normalizedTime);
				newWeight.transform.localScale = targetScale * normalizedTime;
				yield return null;
			}
		}
		newWeight.transform.localPosition = Vector3.zero;
		newWeight.transform.localScale = targetScale;
		newButtonCollider.enabled = true;
		weight1 += weight;
		changingWeight = false;
		soundPlayer.PlaySoundPlacement(weight - 1, newWeight.transform.position);
	}

	public IEnumerator RemoveWeight(ScaleWeight weight, float travelTime)
	{
		weight1 -= weight.Weight;
		soundPlayer.PlaySoundRetrieval(0, weight.transform.position);
		float timer = 0f;
		float normalizedTime = 0f;
		while (normalizedTime < 1f)
		{
			timer += Time.deltaTime;
			normalizedTime = Mathf.Clamp01(timer / travelTime);
			weight.transform.position = Helper.GetBezierCurveValue(weight.transform.parent.position, destroyWeightPoint.position, 2f, normalizedTime);
			weight.transform.localScale = Vector3.one * (1f - normalizedTime);
			yield return null;
		}
		Object.Destroy(weight.gameObject);
	}

	private void SetWeightMeshOnFilter(MeshFilter filter, int meshIndex)
	{
		if (filter != null)
		{
			filter.sharedMesh = weightMeshes[meshIndex];
			ReApplyBoxCollider(filter.transform);
		}
	}

	private BoxCollider ReApplyBoxCollider(Transform transform)
	{
		if (transform == null)
		{
			return null;
		}
		BoxCollider component = transform.GetComponent<BoxCollider>();
		if (component != null)
		{
			if (!Application.isPlaying && Application.isEditor)
			{
				Object.DestroyImmediate(component);
			}
			else
			{
				Object.Destroy(component);
			}
		}
		return transform.gameObject.AddComponent<BoxCollider>();
	}

	private Transform FindEmptyWeightSlot()
	{
		Transform result = null;
		Transform[] array = weightSlots;
		foreach (Transform transform in array)
		{
			if (transform != null && transform.childCount == 0)
			{
				result = transform;
				break;
			}
		}
		return result;
	}

	public RGD_LandmarkItem_JusticeScale GetRGDSave()
	{
		RGD_LandmarkItem_JusticeScale rGD_LandmarkItem_JusticeScale = new RGD_LandmarkItem_JusticeScale(GetComponent<LandmarkItem>());
		List<int> list = new List<int>();
		Transform[] array = weightSlots;
		foreach (Transform transform in array)
		{
			if (!(transform == null) && transform.childCount != 0)
			{
				ScaleWeight componentInChildren = transform.GetComponentInChildren<ScaleWeight>();
				if (!(componentInChildren == null))
				{
					list.Add(componentInChildren.Weight);
				}
			}
		}
		if (list.Count > 0)
		{
			rGD_LandmarkItem_JusticeScale.weightSlots = list.ToArray();
		}
		return rGD_LandmarkItem_JusticeScale;
	}

	public void RestoreRGDSave(RGD_LandmarkItem_JusticeScale rgd)
	{
		if (rgd.weightSlots == null || rgd.weightSlots.Length == 0)
		{
			return;
		}
		for (int i = 0; i < rgd.weightSlots.Length; i++)
		{
			int weight = rgd.weightSlots[i];
			Transform transform = FindEmptyWeightSlot();
			if (transform != null)
			{
				CoroutineManager.Singleton.AddCoroutine(AddWeight(Vector3.zero, transform, 0f, weight));
			}
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		Messages type = msg.Type;
		if (type == Messages.InteractableButtonPress || type == Messages.InteractableButtonPress_Int)
		{
			Message_InteractableButtonPress message_InteractableButtonPress = msg as Message_InteractableButtonPress;
			buttonForwarder.HandleMessage(message_InteractableButtonPress);
			if (message_InteractableButtonPress.success)
			{
				InteractableButton_Networked buttonFromIndex = GetButtonFromIndex(message_InteractableButtonPress.buttonIndex);
				message_InteractableButtonPress.success = ButtonPress(buttonFromIndex);
			}
			return true;
		}
		return base.Deserialize(msg, remoteID);
	}
}
