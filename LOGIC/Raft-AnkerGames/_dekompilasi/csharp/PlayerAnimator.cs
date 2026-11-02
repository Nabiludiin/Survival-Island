using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	public Action<string> OnAnimationEvent;

	public Action<Message_Player_Update> Action_OnDeSerializeNetworkProperties;

	[HideInInspector]
	public List<string> networkAnimTriggers = new List<string>();

	public Animator anim;

	public RuntimeAnimatorController fpController;

	[SerializeField]
	[Range(0f, 15f)]
	private float networkVelocityLerpSpeed = 9f;

	[SerializeField]
	private float maxTriggerTime = 3f;

	[HideInInspector]
	public int selectedIndex;

	[HideInInspector]
	public Vector4 animVelocity;

	[HideInInspector]
	public Vector4 animTargetVelocity;

	private float triggeringTimer;

	private void Awake()
	{
		triggeringTimer = maxTriggerTime;
	}

	private void Update()
	{
		animVelocity = Vector4.Lerp(animVelocity, animTargetVelocity, Time.deltaTime * networkVelocityLerpSpeed);
		anim.SetFloat("VelocityX", animVelocity.x);
		anim.SetFloat("VelocityY", animVelocity.y);
		anim.SetFloat("VelocityZ", animVelocity.z);
		anim.SetFloat("VelocityW", animVelocity.w);
		if (anim.GetBool("Triggering"))
		{
			triggeringTimer += Time.deltaTime;
			if (triggeringTimer >= maxTriggerTime)
			{
				StopTriggering();
				triggeringTimer = 0f;
			}
		}
		else
		{
			triggeringTimer = 0f;
		}
	}

	public void SetAnimation(PlayerFullBodyAnimation animation)
	{
		string[] array = animation.ToString().Split('_');
		if (array.Length >= 2)
		{
			int.TryParse(array[1], out var result);
			anim.SetInteger("FullBodyIndex", result);
		}
	}

	public void SetAnimation(PlayerAnimation animation, bool triggering = false)
	{
		string[] array = animation.ToString().Split('_');
		if (array.Length == 0)
		{
			return;
		}
		string text = array[0];
		if (text == "Index" && array.Length == 3)
		{
			int index = int.Parse(array[1]);
			SetAnimationIndex(index, overrideIndex: false);
		}
		else if (text == "Trigger" && array.Length == 2)
		{
			string text2 = array[1];
			anim.SetTrigger(text2);
			if (!networkAnimTriggers.Contains(text2))
			{
				networkAnimTriggers.Add(text2);
			}
			if (triggering)
			{
				anim.SetBool("Triggering", triggering);
			}
		}
	}

	public void SetAnimation(PlayerAnimation animation, bool overrideIndex, bool triggering)
	{
		string[] array = animation.ToString().Split('_');
		if (array.Length == 0)
		{
			return;
		}
		string text = array[0];
		if (text == "Index" && array.Length == 3)
		{
			int index = int.Parse(array[1]);
			SetAnimationIndex(index, overrideIndex);
		}
		else if (text == "Trigger" && array.Length == 2)
		{
			string text2 = array[1];
			anim.SetTrigger(text2);
			if (!networkAnimTriggers.Contains(text2))
			{
				networkAnimTriggers.Add(text2);
			}
			if (triggering)
			{
				anim.SetBool("Triggering", triggering);
			}
		}
	}

	public void ReselectAnimation()
	{
		anim.SetInteger("ItemID", selectedIndex);
		anim.SetTrigger("Switch");
	}

	public void SetAnimationIndex(int index, bool overrideIndex)
	{
		if (overrideIndex || selectedIndex != index)
		{
			selectedIndex = index;
			anim.SetInteger("ItemID", selectedIndex);
			anim.SetTrigger("Switch");
		}
	}

	public bool IsInFullBodyAnimation(PlayerFullBodyAnimation animation)
	{
		string[] array = animation.ToString().Split('_');
		if (array.Length < 2)
		{
			return false;
		}
		int.TryParse(array[1], out var result);
		return anim.GetInteger("FullBodyIndex") == result;
	}

	public void SetNetworkProperties(Message_Player_Update msg)
	{
		Vector4 animationVelocity = msg.AnimationVelocity;
		SetAnimVelocity(animationVelocity);
		anim.SetBool("ItemHit", msg.Anim_ItemHit);
		anim.SetBool("HookThrow", msg.Anim_HookThrow);
		anim.SetBool("HookInHand", msg.Anim_HookInHand);
		anim.SetBool("Crouching", msg.Anim_Crouching);
		anim.SetBool("HasAmmo", msg.Anim_HasAmmo);
		anim.SetInteger("FullBodyIndex", msg.Anim_FullBodyIndex);
		string[] anim_triggers = msg.anim_triggers;
		foreach (string trigger in anim_triggers)
		{
			anim.SetTrigger(trigger);
		}
		if (Action_OnDeSerializeNetworkProperties != null)
		{
			Action_OnDeSerializeNetworkProperties(msg);
		}
	}

	public void SetAnimVelocity(Vector4 velocity)
	{
		animTargetVelocity = velocity;
	}

	public void StopTriggering()
	{
		if (anim.GetBool("Triggering"))
		{
			anim.SetBool("Triggering", value: false);
			anim.ResetTrigger("PointPlace");
			anim.ResetTrigger("Plant");
			anim.ResetTrigger("ItemHitTrigger");
		}
	}

	public void AnimationString(string parameter)
	{
		if (OnAnimationEvent != null)
		{
			OnAnimationEvent(parameter);
		}
	}
}
