using UnityEngine;

[RequireComponent(typeof(SteeringWheel))]
public class SteeringWheelRudderAttachment : MonoBehaviour
{
	[SerializeField]
	private SteeringWheel steeringWheel;

	public Transform rudderAttachmentPipeParent;

	public GameObject rudderPipePrefab;

	public GameObject rudderHalfPipePrefab;

	public GameObject rudderPrefab;

	public float buildRudderHeightOffset = 0.1f;

	private void OnValidate()
	{
		if (steeringWheel == null)
		{
			steeringWheel = GetComponent<SteeringWheel>();
		}
	}

	private void Start()
	{
		if (steeringWheel == null)
		{
			steeringWheel = GetComponent<SteeringWheel>();
		}
	}

	public void BuildRudderPipesAttachment(SteeringWheel steeringWheel)
	{
		float num = steeringWheel.transform.localPosition.y / 2.42f;
		int num2 = Mathf.RoundToInt(num);
		float num3 = num - (float)num2;
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		if (num3 > 0.45f)
		{
			vector2 = new Vector3(0f, -1.21f, 0f);
			BuildHalfRudderPipe(vector2 + vector);
		}
		for (int i = 1; i < num2 + 1; i++)
		{
			vector = vector2 + new Vector3(0f, (float)(-i) * 2.42f, 0f);
			BuildRudderPipe(vector);
		}
		if (num2 == 0)
		{
			vector += vector2;
		}
		ExtendSteeringWheelColliderToPosition(steeringWheel, vector);
		BuildRudder(new Vector3(0f, vector.y - buildRudderHeightOffset, 0f));
	}

	private void BuildRudderPipe(Vector3 position)
	{
		Object.Instantiate(rudderPipePrefab, rudderAttachmentPipeParent).transform.localPosition = position;
	}

	private void BuildHalfRudderPipe(Vector3 position)
	{
		Object.Instantiate(rudderHalfPipePrefab, rudderAttachmentPipeParent).transform.localPosition = position;
	}

	private void BuildRudder(Vector3 position)
	{
		GameObject gameObject = Object.Instantiate(rudderPrefab, rudderAttachmentPipeParent);
		gameObject.transform.localPosition = position;
		steeringWheel.rudderRotator = gameObject.GetComponent<SteeringWheelRudderRotator>();
	}

	private void ExtendSteeringWheelColliderToPosition(SteeringWheel steeringWheel, Vector3 position)
	{
		steeringWheel.rudderPipeCollider.center = new Vector3(steeringWheel.rudderPipeCollider.center.x, (position.y - steeringWheel.rudderPipeCollider.center.y) / 2f, steeringWheel.rudderPipeCollider.center.z);
		steeringWheel.rudderPipeCollider.size = steeringWheel.rudderPipeCollider.size + new Vector3(0f, Mathf.Abs(position.y), 0f);
	}
}
