using UnityEngine;

public class Network_Entity : MonoBehaviour_ID_Network
{
	public delegate void OnDamageTaken(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageInflictorEntityType, bool isLocal);

	public EntityType entityType;

	public Stat_Health stat_health;

	public BuffManager buffManager;

	public bool IsInvurnerable;

	public bool removesDurabilityWhenHit = true;

	public Renderer renderer;

	private Raft_Network network;

	private MaterialPropertyBlock materialPropertyBlock;

	public bool IsDead => stat_health.Value <= 0f;

	public Raft_Network Network
	{
		get
		{
			if (network == null)
			{
				network = ComponentManager<Raft_Network>.Value;
			}
			return network;
		}
	}

	public event OnDamageTaken OnDamageTakenEvent;

	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
		RefreshHPShader(1f);
	}

	protected virtual void Update()
	{
		if (buffManager != null)
		{
			buffManager.UpdateBuffs();
		}
	}

	public void Button_Damage()
	{
		Damage(10f, transform.position, Vector3.up, EntityType.Player, isLocal: false);
	}

	public void Button_Kill()
	{
		Damage(99999f, transform.position, Vector3.up, EntityType.Player, isLocal: false);
	}

	public virtual void Damage(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageInflictorEntityType, bool isLocal, SO_Buff buffAsset = null)
	{
		if (damage != 999999f && IsInvurnerable)
		{
			damage = 0f;
		}
		stat_health.Value -= damage;
		RefreshHPShader();
		OnDamageTakenEvent?.Invoke(damage, hitPoint, hitNormal, damageInflictorEntityType, isLocal);
		if (buffManager != null)
		{
			if (ShouldClearBuffs())
			{
				buffManager.ClearBuffs();
			}
			if (buffAsset != null)
			{
				buffManager.AddBuff(buffAsset);
			}
		}
	}

	protected virtual bool ShouldClearBuffs()
	{
		if (IsDead)
		{
			return true;
		}
		return false;
	}

	public void RefreshHPShader(float normalizedHP = -1f)
	{
		if (renderer == null)
		{
			return;
		}
		if (normalizedHP == -1f)
		{
			if (stat_health == null)
			{
				return;
			}
			normalizedHP = stat_health.NormalValue;
		}
		if (materialPropertyBlock == null)
		{
			materialPropertyBlock = new MaterialPropertyBlock();
		}
		renderer.GetPropertyBlock(materialPropertyBlock);
		materialPropertyBlock.SetFloat("_Damaged_HP", normalizedHP);
		renderer.SetPropertyBlock(materialPropertyBlock);
	}
}
