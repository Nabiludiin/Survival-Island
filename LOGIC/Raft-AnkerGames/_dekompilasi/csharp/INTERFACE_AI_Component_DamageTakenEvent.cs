using UnityEngine;

public interface INTERFACE_AI_Component_DamageTakenEvent
{
	void OnDamageTakenEvent(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageInflictorEntityType, bool isLocal);
}
