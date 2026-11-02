using System;
using UnityEngine;

public class Rope : MonoBehaviour
{
	[Serializable]
	public class RopeAutoFollow
	{
		public int index;

		public Transform followParent;

		[HideInInspector]
		public Vector3 lastRefreshPosition;
	}

	[SerializeField]
	private bool startOff = true;

	public bool refreshTileInUpdate;

	public bool autoTileMaterial = true;

	[SerializeField]
	private Renderer ropeMaterial;

	[SerializeField]
	private Vector2 bumpTile = Vector2.zero;

	[SerializeField]
	private Vector2 diffuseTile = Vector2.zero;

	[SerializeField]
	private RopeAutoFollow[] autoFollow;

	[SerializeField]
	private Transform[] ropeParts;

	private MaterialPropertyBlock propertyBlock;

	public void EditorRefresh()
	{
		Refresh();
	}

	private void Start()
	{
		if (autoFollow != null && autoFollow.ContainsItems())
		{
			for (int i = 0; i < autoFollow.Length; i++)
			{
				RopeAutoFollow ropeAutoFollow = autoFollow[i];
				if (ropeAutoFollow != null && !(ropeAutoFollow.followParent == null))
				{
					SetPosition(ropeAutoFollow.index, ropeAutoFollow.followParent);
				}
			}
		}
		Refresh();
		if (startOff)
		{
			gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (!refreshTileInUpdate || autoFollow == null || autoFollow.Length == 0)
		{
			enabled = false;
			return;
		}
		bool flag = false;
		for (int i = 0; i < autoFollow.Length; i++)
		{
			RopeAutoFollow ropeAutoFollow = autoFollow[i];
			if (ropeAutoFollow != null)
			{
				Transform transform = ropeParts[ropeAutoFollow.index];
				if (flag || transform.position != ropeAutoFollow.lastRefreshPosition)
				{
					flag = true;
					ropeAutoFollow.lastRefreshPosition = transform.position;
				}
			}
		}
		if (flag)
		{
			Refresh();
		}
	}

	public void SetPosition(int index, Vector3 position, bool localPosition = false)
	{
		if (!gameObject.activeInHierarchy)
		{
			gameObject.SetActive(value: true);
		}
		if (index >= 0 && index <= ropeParts.Length - 1)
		{
			if (!localPosition)
			{
				ropeParts[index].position = position;
			}
			else
			{
				ropeParts[index].localPosition = position;
			}
			Refresh();
		}
	}

	public void SetPosition(int index, Transform transformToFollow)
	{
		if (index >= 0 && index <= ropeParts.Length - 1)
		{
			ropeParts[index].SetParent(transformToFollow);
			SetPosition(index, transformToFollow.position);
		}
	}

	public void SetPosition(int index, Transform transformToFollow, Vector3 localPosition)
	{
		if (index >= 0 && index <= ropeParts.Length - 1)
		{
			ropeParts[index].SetParentSafe(transformToFollow);
			SetPosition(index, localPosition, localPosition: true);
		}
	}

	public void ResetParent()
	{
		for (int i = 0; i < ropeParts.Length; i++)
		{
			if (ropeParts[i].parent != transform)
			{
				ropeParts[i].SetParent(transform);
			}
		}
	}

	private void Refresh()
	{
		Quaternion rotation = Quaternion.identity;
		for (int i = 0; i < ropeParts.Length; i++)
		{
			Transform transform = ropeParts[i];
			if (!(transform == null))
			{
				if (i == ropeParts.Length - 1)
				{
					transform.rotation = rotation;
					continue;
				}
				transform.transform.LookAt(ropeParts[i + 1].transform);
				rotation = transform.rotation;
			}
		}
		if (autoTileMaterial && ropeMaterial != null && ropeParts != null && ropeParts.Length != 0)
		{
			float num = Vector3.Distance(ropeParts[0].position, ropeParts[ropeParts.Length - 1].position);
			if (propertyBlock == null)
			{
				propertyBlock = new MaterialPropertyBlock();
			}
			ropeMaterial.GetPropertyBlock(propertyBlock);
			propertyBlock.SetVector("_NormalMap_ST", new Vector4(bumpTile.x, bumpTile.y * num));
			propertyBlock.SetVector("_Diffuse_ST", new Vector4(diffuseTile.x, diffuseTile.y * num));
			ropeMaterial.SetPropertyBlock(propertyBlock);
		}
	}
}
