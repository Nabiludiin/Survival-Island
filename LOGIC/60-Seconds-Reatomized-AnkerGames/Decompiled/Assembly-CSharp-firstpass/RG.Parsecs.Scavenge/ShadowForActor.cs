using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ShadowForActor : MonoBehaviour
{
	public enum ShadowFallowType
	{
		Transform
	}

	public GameObject shadow;

	public ShadowFallowType fallowType;

	public bool IsParentRotating = true;

	public float scallerDivide = 3f;

	public bool RotateTo90 = true;

	private Vector3 baseScale;

	private Transform _shadowTransform;

	private Transform _transform;

	private Vector3 basePivot = Vector3.zero;

	private bool _shadowTransformDifferentFromTransform;

	private bool _flipUpdate;

	private static Vector3 SHADOW_ROTATION = new Vector3(90f, 0f, 0f);

	private void Awake()
	{
		_transform = transform;
		_shadowTransform = shadow.transform;
		if (RotateTo90)
		{
			_shadowTransform.eulerAngles = SHADOW_ROTATION;
		}
		_shadowTransformDifferentFromTransform = _transform != _shadowTransform;
	}

	private void Start()
	{
		baseScale = _shadowTransform.localScale;
		if (fallowType == ShadowFallowType.Transform)
		{
			basePivot = _shadowTransform.localPosition;
			basePivot.y = 0f;
		}
	}

	private void Update()
	{
		_flipUpdate = !_flipUpdate;
		if (_flipUpdate)
		{
			_ = scallerDivide;
			float num = _transform.position.y / scallerDivide;
			float num2 = 1f;
			if (num > 0.1f)
			{
				num2 = 1f / (num + 0.9f);
			}
			_shadowTransform.localScale = baseScale * num2;
		}
		if (IsParentRotating && RotateTo90)
		{
			_shadowTransform.eulerAngles = SHADOW_ROTATION;
		}
		Vector3 position = ((fallowType != ShadowFallowType.Transform || !_shadowTransformDifferentFromTransform) ? _shadowTransform.position : (_transform.position + basePivot));
		position.y = 0.01f;
		_shadowTransform.position = position;
	}

	public void HideShadow()
	{
		shadow.gameObject.SetActive(value: false);
		gameObject.SetActive(value: false);
	}

	public void ShowShadow()
	{
		shadow.gameObject.SetActive(value: true);
		gameObject.SetActive(value: true);
	}
}
