using UnityEngine;

[RequireComponent(typeof(Subregion))]
public abstract class SubregionComponent : MonoBehaviour
{
	public abstract void AddSubregionComponent();

	public virtual void LateUpdateSubRegionComponent(bool playerIsWithinSubregion)
	{
	}
}
