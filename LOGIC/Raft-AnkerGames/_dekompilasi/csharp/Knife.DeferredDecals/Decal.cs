using UnityEngine;

namespace Knife.DeferredDecals;

[ExecuteInEditMode]
public class Decal : MonoBehaviour
{
	[SerializeField]
	private Material m_Material;

	public int SortingOrder;

	public Color InstancedColor = Color.white;

	[Range(0f, 1f)]
	public float Fade = 1f;

	public Vector2 UVTiling = new Vector2(1f, 1f);

	public Vector2 UVOffset = new Vector2(0f, 0f);

	[HideInInspector]
	public float DistanceFade;

	public bool NeedDrawGizmos = true;

	public MaterialPropertyBlock PropertyBlock;

	private Bounds decalBounds;

	private Vector3 cachedPosition;

	private Vector3 cachedSize;

	private Quaternion cachedRotation;

	private bool needRebuildBounds;

	private Transform cachedTransform;

	private GameObject decalGameObject;

	[SerializeField]
	private bool rebuildOnUpdate;

	public Material DecalMaterial
	{
		get
		{
			if (m_Material == null)
			{
				return DeferredDecalsSystem.DefaultDecalMaterial;
			}
			return m_Material;
		}
		set
		{
			m_Material = value;
		}
	}

	public Vector4 UV
	{
		get
		{
			return new Vector4(UVTiling.x, UVTiling.y, UVOffset.x, UVOffset.y);
		}
		set
		{
			UVTiling.x = value.x;
			UVTiling.y = value.y;
			UVOffset.x = value.z;
			UVOffset.y = value.w;
		}
	}

	public Bounds Bounds
	{
		get
		{
			return decalBounds;
		}
		set
		{
			decalBounds = value;
		}
	}

	public Transform CachedTransform => cachedTransform;

	public GameObject DecalGameObject
	{
		get
		{
			if (decalGameObject == null)
			{
				decalGameObject = gameObject;
			}
			return decalGameObject;
		}
		private set
		{
			decalGameObject = value;
		}
	}

	private bool IsEnabledAndActive
	{
		get
		{
			if (enabled)
			{
				return DecalGameObject.activeInHierarchy;
			}
			return false;
		}
	}

	public void OnEnable()
	{
		cachedTransform = transform;
		if (IsEnabledAndActive)
		{
			DeferredDecalsManager.instance.AddOrUpdateDecal(this);
		}
		PropertyBlock = new MaterialPropertyBlock();
		CheckForRebuild();
	}

	public void Start()
	{
		if (IsEnabledAndActive)
		{
			DeferredDecalsManager.instance.AddOrUpdateDecal(this);
		}
	}

	public void OnDisable()
	{
		DeferredDecalsManager.instance.RemoveDecal(this);
	}

	private void OnValidate()
	{
		if (IsEnabledAndActive)
		{
			UpdateSortingOrder(SortingOrder);
		}
	}

	public void UpdateSortingOrder(int order)
	{
		SortingOrder = order;
		DeferredDecalsManager.instance.AddOrUpdateDecal(this);
	}

	public void SetupBounds()
	{
		if (needRebuildBounds)
		{
			float num = 0.5f;
			float num2 = 0.5f;
			float num3 = 0.5f;
			Vector3 position = new Vector3(0f - num, (0f - num2) * 2f, 0f - num3);
			Vector3 position2 = new Vector3(0f - num, (0f - num2) * 2f, num3);
			Vector3 position3 = new Vector3(num, (0f - num2) * 2f, num3);
			Vector3 position4 = new Vector3(num, (0f - num2) * 2f, 0f - num3);
			Vector3 position5 = new Vector3(0f - num, 0f, 0f - num3);
			Vector3 position6 = new Vector3(0f - num, 0f, num3);
			Vector3 position7 = new Vector3(num, 0f, num3);
			Vector3 position8 = new Vector3(num, 0f, num3);
			position = CachedTransform.TransformPoint(position);
			position2 = CachedTransform.TransformPoint(position2);
			position3 = CachedTransform.TransformPoint(position3);
			position4 = CachedTransform.TransformPoint(position4);
			position5 = CachedTransform.TransformPoint(position5);
			position6 = CachedTransform.TransformPoint(position6);
			position7 = CachedTransform.TransformPoint(position7);
			position8 = CachedTransform.TransformPoint(position8);
			float x = Mathf.Min(position.x, position2.x, position3.x, position4.x, position5.x, position6.x, position7.x, position8.x);
			float y = Mathf.Min(position.y, position2.y, position3.y, position4.y, position5.y, position6.y, position7.y, position8.y);
			float z = Mathf.Min(position.z, position2.z, position3.z, position4.z, position5.z, position6.z, position7.z, position8.z);
			float x2 = Mathf.Max(position.x, position2.x, position3.x, position4.x, position5.x, position6.x, position7.x, position8.x);
			float y2 = Mathf.Max(position.y, position2.y, position3.y, position4.y, position5.y, position6.y, position7.y, position8.y);
			float z2 = Mathf.Max(position.z, position2.z, position3.z, position4.z, position5.z, position6.z, position7.z, position8.z);
			Vector3 min = new Vector3(x, y, z);
			Vector3 max = new Vector3(x2, y2, z2);
			Bounds bounds = Bounds;
			bounds.SetMinMax(min, max);
			Bounds = bounds;
			needRebuildBounds = false;
		}
	}

	public void UpdateBoundsCenter()
	{
		Bounds bounds = Bounds;
		bounds.center = CachedTransform.position - CachedTransform.up * 0.5f * cachedSize.y;
		Bounds = bounds;
	}

	private void DrawGizmo(bool selected)
	{
		_ = Gizmos.matrix;
		if (NeedDrawGizmos || !selected)
		{
			Color color = new Color(0f, 0.7f, 1f, 1f);
			color.a = (selected ? 0.1f : 0.05f);
			if (!DeferredDecalsSystem.DrawDecalGizmos && !selected)
			{
				color.a = 0f;
			}
			Gizmos.color = color;
			Gizmos.matrix = CachedTransform.localToWorldMatrix;
			Gizmos.DrawCube(Vector3.zero - Vector3.up * 0.5f, Vector3.one);
			color.a = (selected ? 0.5f : 0.25f);
			if (!DeferredDecalsSystem.DrawDecalGizmos && !selected)
			{
				color.a = 0f;
			}
			Gizmos.color = color;
			Gizmos.DrawWireCube(Vector3.zero - Vector3.up * 0.5f, Vector3.one);
		}
	}

	private void Update()
	{
		if (rebuildOnUpdate)
		{
			CheckForRebuild();
		}
	}

	private void CheckForRebuild()
	{
		if (Vector3.Distance(cachedPosition, CachedTransform.position) >= 0.001f)
		{
			cachedPosition = CachedTransform.position;
			UpdateBoundsCenter();
		}
		if (Vector3.Distance(cachedSize, CachedTransform.localScale) >= 0.001f)
		{
			needRebuildBounds = true;
			cachedSize = CachedTransform.localScale;
		}
		if (Quaternion.Angle(cachedRotation, CachedTransform.rotation) >= 0.1f)
		{
			needRebuildBounds = true;
			cachedRotation = CachedTransform.rotation;
		}
	}

	public void OnDrawGizmos()
	{
		DrawGizmo(selected: false);
	}

	public void OnDrawGizmosSelected()
	{
		DrawGizmo(selected: true);
	}
}
