using UnityEngine;

public class SubRegionComponent_OxygenBounds : SubregionComponent
{
	[SerializeField]
	private float surfaceOffset;

	[SerializeField]
	private Transform optionalVisualWaterRemover;

	public float SurfacePointY => transform.position.y + surfaceOffset;

	public override void AddSubregionComponent()
	{
		GetComponent<Subregion>().subregionComponents.Add(typeof(SubRegionComponent_OxygenBounds), this);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawLine(transform.position, transform.position + Vector3.up * surfaceOffset);
	}

	private void AdjustVisualWaterRemover()
	{
		if (optionalVisualWaterRemover != null)
		{
			optionalVisualWaterRemover.transform.position = transform.position + transform.up * 0.35f;
			optionalVisualWaterRemover.transform.eulerAngles = transform.eulerAngles;
			if (optionalVisualWaterRemover.parent == transform)
			{
				optionalVisualWaterRemover.localScale = Vector3.one;
			}
		}
	}
}
