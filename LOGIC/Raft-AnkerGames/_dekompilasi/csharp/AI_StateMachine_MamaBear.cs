using System;
using UnityEngine;

public class AI_StateMachine_MamaBear : AI_StateMachine_Bear
{
	[SerializeField]
	private AI_State_MoveToPoint moveToPointState;

	[HideInInspector]
	public Transform lureTransform;

	[HideInInspector]
	public Transform caveTransform;

	private ParticleSystem berryParticleSystem;

	public override void Start()
	{
		base.Start();
		if (Raft_Network.IsHost)
		{
			Quest questByType = QuestProgressTracker.GetQuestByType(QuestType.MamaBearLurePlaced);
			if (questByType != null)
			{
				questByType.OnQuestComplete = (Action)Delegate.Combine(questByType.OnQuestComplete, new Action(OnLureQuestComplete));
			}
		}
	}

	protected override void OnDeath(EntityType damageType)
	{
		base.OnDeath(damageType);
		QuestProgressTracker.FinishQuestRPC(QuestType.MamaBearKilled);
	}

	public void FindObjectOfInterests()
	{
		lureTransform = ObjectManager.GetObjectOfInterest(ObjectOfInterestType.MamaBear_Lure);
		caveTransform = ObjectManager.GetObjectOfInterest(ObjectOfInterestType.MamaBear_Cave);
	}

	private void OnLureQuestComplete()
	{
		if (Raft_Network.IsHost && currentState != moveToPointState)
		{
			ChangeState(moveToPointState);
		}
	}

	public void PlayMunchBerryParticle()
	{
		if (berryParticleSystem == null)
		{
			berryParticleSystem = lureTransform.GetComponentInChildren<ParticleSystem>();
		}
		berryParticleSystem.Play();
	}
}
