using UnityEngine;
using UnityEngine.UI;

public class Reciever_Dot : MonoBehaviour
{
	[HideInInspector]
	public bool targetedByReciever;

	public ChunkPoint chunkPoint;

	private float lengthToPoint;

	private RectTransform rect;

	[SerializeField]
	private Text dotNumberText;

	[SerializeField]
	private Image dotImage;

	[SerializeField]
	private Sprite dotSprite;

	[SerializeField]
	private Sprite targetedDotSprite;

	[SerializeField]
	private Sprite notTargetedDotSprite;

	public float LengthToPoint => lengthToPoint;

	private void Awake()
	{
		rect = GetComponent<RectTransform>();
	}

	public void SetText(string text)
	{
		dotNumberText.text = text;
	}

	public void SetLocalPosition(Vector3 localPos)
	{
		rect.localPosition = localPos;
	}

	public void SetLengthToPoint(float length)
	{
		lengthToPoint = length;
	}

	public void SetTargetedByReciever(bool targeted)
	{
		targetedByReciever = targeted;
		if (targetedByReciever)
		{
			if (dotImage.sprite != targetedDotSprite)
			{
				dotImage.sprite = targetedDotSprite;
			}
		}
		else if (chunkPoint != null && chunkPoint.rule != null && chunkPoint.rule.isFrequencyPoint)
		{
			if (dotImage.sprite != notTargetedDotSprite)
			{
				dotImage.sprite = notTargetedDotSprite;
			}
		}
		else if (dotImage.sprite != dotSprite)
		{
			dotImage.sprite = dotSprite;
		}
	}
}
