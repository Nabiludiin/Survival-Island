using System;
using System.Collections;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PlayerStats : Network_Entity
{
	[SerializeField]
	private ParticleSystem bloodParticles;

	public Stat_Bonus_Consumable stat_hunger;

	public Stat_Bonus_Consumable stat_thirst;

	public Stat_Health stat_BonusHealth;

	public Stat_Oxygen stat_oxygen;

	public bool hasDeathPrevention;

	public Action OnDeath;

	[SerializeField]
	private float healthLostPerSecondDrowning = 1f;

	[SerializeField]
	private float healthLostPerSecondWellbeing = 1f;

	[SerializeField]
	private float healthLostPerSecondZeroed = 1f;

	[SerializeField]
	private TimerEventer updateUISliderTimer;

	[SerializeField]
	private SO_Buff deathPreventionTriggeredInvurnabilityBuff;

	[SerializeField]
	private SO_Buff deathPreventionTriggeredHealEffect;

	[SerializeField]
	private float sineFrequency = 1f;

	[SerializeField]
	private Interval_Float vignetteInterval;

	[SerializeField]
	private Interval_Float aberationInterval;

	[SerializeField]
	private Interval_Float saturationInterval;

	[SerializeField]
	private SO_ScreenEffect bloodSplatterSettings;

	[SerializeField]
	private SO_CameraShaker cameraShakeSettings;

	[SerializeField]
	private float soundIntervall = 3f;

	[SerializeField]
	private StudioEventEmitter eventEmitter_Damaged;

	[SerializeField]
	private StudioEventEmitter eventEmitter_Drowning;

	[SerializeField]
	private StudioEventEmitter eventEmitter_Thirsty;

	[SerializeField]
	private StudioEventEmitter eventEmitter_Hungry;

	[SerializeField]
	private StudioEventEmitter eventEmitter_LowBreath;

	private Coroutine coroutine_hungrySound;

	private Coroutine coroutine_thirstySound;

	private Coroutine coroutine_lowBreathSound;

	private Coroutine coroutine_drowningSound;

	private CanvasHelper canvas;

	private Network_Player playerNetwork;

	private PostProcessingProfile postEffects;

	private ScreenEffectManager screenEffectManager;

	private ArmorHandler armorHandler;

	private VignetteModel.Settings defVignette;

	private ChromaticAberrationModel.Settings defAberation;

	private ColorGradingModel.Settings defColorGrading;

	private float voiceParameter;

	private float sineTime;

	public void SendOnDeath()
	{
		OnDeath?.Invoke();
	}

	protected override void Awake()
	{
		base.Awake();
		armorHandler = GetComponent<ArmorHandler>();
		playerNetwork = GetComponent<Network_Player>();
	}

	protected override void Start()
	{
		base.Start();
		canvas = ComponentManager<CanvasHelper>.Value;
		screenEffectManager = ComponentManager<ScreenEffectManager>.Value;
		Settings value = ComponentManager<Settings>.Value;
		if (value != null)
		{
			postEffects = value.graphicsBox.postEffects;
			defVignette = postEffects.vignette.settings;
			defAberation = postEffects.chromaticAberration.settings;
			defColorGrading = postEffects.colorGrading.settings;
		}
		NetworkIDManager.AddNetworkID(this, typeof(Network_Entity));
		if (playerNetwork != null && playerNetwork.currentModel != null)
		{
			voiceParameter = (float)playerNetwork.currentModel.voiceType;
		}
	}

	protected override void OnDestroy()
	{
		if (playerNetwork.IsLocalPlayer && postEffects != null)
		{
			Debug.Log("Reset post effect when exiting game!");
			postEffects.vignette.settings = defVignette;
			postEffects.vignette.enabled = false;
			postEffects.chromaticAberration.settings = defAberation;
			postEffects.chromaticAberration.enabled = false;
			postEffects.colorGrading.settings = defColorGrading;
		}
		playerNetwork = null;
	}

	protected override void Update()
	{
		base.Update();
		if (!playerNetwork.IsLocalPlayer)
		{
			return;
		}
		if (updateUISliderTimer.DoUpdate(Time.deltaTime, autoResetTimer: true))
		{
			UpdateUISliders();
		}
		HandleUIFeedback();
		HandleSoundFeedback();
		if (IsDead)
		{
			return;
		}
		if (GameModeValueManager.IsPlayerInvurnerable)
		{
			Stat_WellBeing.Factor = WellBeing.Good;
			UpdateUISliders();
			HandleUIFeedback();
			return;
		}
		SetOxygenUI();
		float num = 0f;
		if (stat_oxygen.IsZero)
		{
			num += healthLostPerSecondDrowning * Time.deltaTime;
		}
		else if (stat_hunger.Normal.IsZero || stat_thirst.Normal.IsZero)
		{
			num += healthLostPerSecondZeroed * Time.deltaTime;
		}
		else if (!stat_thirst.Normal.MeetsWellBeing && !stat_hunger.Normal.MeetsWellBeing)
		{
			num += healthLostPerSecondWellbeing * Time.deltaTime;
		}
		else if (Stat_WellBeing.Factor != WellBeing.Bad)
		{
			stat_health.Regenerate();
		}
		if (num > 0f)
		{
			float num2 = num;
			float value = stat_BonusHealth.statTarget.Value;
			if (num > 0f && value > 0f)
			{
				if (num > value)
				{
					stat_BonusHealth.Value -= value;
					num2 -= value;
				}
				else
				{
					stat_BonusHealth.Value -= num;
					num2 = 0f;
				}
			}
			if (num2 > 0f)
			{
				stat_health.Value -= num2;
			}
		}
		if (IsDead)
		{
			Message_NetworkBehaviour message = new Message_NetworkBehaviour(Messages.PlayerKilled, playerNetwork);
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				playerNetwork.PlayerScript.Kill();
			}
			else
			{
				playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void Consume(Item_Base edibleItem)
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponent<Network_Player>();
		}
		if (playerNetwork.IsLocalPlayer)
		{
			ItemInstance_Consumeable settings_consumeable = edibleItem.settings_consumeable;
			stat_hunger.Consume(settings_consumeable.HungerYield, settings_consumeable.BonusHungerYield, settings_consumeable.IsRaw);
			if (settings_consumeable.ThirstYield != 0f)
			{
				stat_thirst.Consume(settings_consumeable.ThirstYield, settings_consumeable.BonusThirstYield, settings_consumeable.IsRaw);
			}
			if (settings_consumeable.OxygenYield > 0f)
			{
				stat_oxygen.Value += settings_consumeable.OxygenYield;
			}
		}
		string eventRef_ConsumeSound = edibleItem.settings_consumeable.EventRef_ConsumeSound;
		if (!string.IsNullOrEmpty(eventRef_ConsumeSound))
		{
			RuntimeManager.PlayOneShotAttached(eventRef_ConsumeSound, gameObject);
		}
	}

	private void UpdateUISliders()
	{
		if (canvas == null)
		{
			canvas = ComponentManager<CanvasHelper>.Value;
		}
		if (!(canvas == null) && !(stat_thirst == null) && !(stat_health == null) && !(stat_hunger == null))
		{
			canvas.healthSlider.SetValue(stat_health.NormalValue);
			canvas.healthSlider.SetTargetValue(stat_health.NormalValue - buffManager.BuffResult.estimatedDamage / stat_health.Max);
			stat_health.statTarget.Value = stat_health.Value;
			stat_BonusHealth.statTarget.Value = stat_BonusHealth.Value;
			HandleBonusSlider(canvas.healthSlider, canvas.bonusHealthSlider, canvas.bonusHealthBackground, stat_health, stat_BonusHealth, forceShowBonusSlider: false, simplified: true);
			canvas.hungerSlider.SetValue(stat_hunger.Normal.NormalValue);
			canvas.hungerSlider.SetTargetValue(stat_hunger.Normal.statTarget.NormalValue);
			HandleBonusSlider(canvas.hungerSlider, canvas.bonusHungerSlider, canvas.bonusHungerBackground, stat_hunger.Normal, stat_hunger.Bonus, stat_hunger.bonusConsumable.Consuming, simplified: false);
			canvas.thirstSlider.SetValue(stat_thirst.Normal.NormalValue);
			canvas.thirstSlider.SetTargetValue(stat_thirst.Normal.statTarget.NormalValue);
			HandleBonusSlider(canvas.thirstSlider, canvas.bonusThirstSlider, canvas.bonusThirstBackground, stat_thirst.Normal, stat_thirst.Bonus, stat_thirst.bonusConsumable.Consuming, simplified: false);
		}
	}

	private void HandleBonusSlider(UISlider_Stat normalSlider, UISlider_Stat bonusSlider, GameObject bonusUIBackground, Stat_Target normal, Stat_Target bonus, bool forceShowBonusSlider, bool simplified)
	{
		float x = normalSlider.sliderTransform.sizeDelta.x;
		float num = bonusSlider.sliderTransform.sizeDelta.x / x;
		float num2 = 0f;
		num2 = ((!simplified) ? (normal.statTarget.Value + bonus.statTarget.Value * num) : (normal.statTarget.Value + bonus.statTarget.Value));
		bool flag = num2 > normal.Max;
		if (bonusUIBackground.activeSelf != flag)
		{
			bonusUIBackground.SetActive(flag);
		}
		bool flag2 = (bonus.Value > 0f) | forceShowBonusSlider;
		if (bonusSlider.gameObject.activeSelf != flag2)
		{
			bonusSlider.gameObject.SetActive(flag2);
		}
		if (flag2)
		{
			bonusSlider.SetValue(bonus.NormalValue);
			bonusSlider.SetTargetValue(bonus.statTarget.NormalValue);
		}
	}

	private void HandleUIFeedback()
	{
		if (postEffects == null || stat_thirst == null || stat_hunger == null || stat_health == null)
		{
			return;
		}
		bool num = !stat_thirst.Normal.MeetsWellBeing || !stat_hunger.Normal.MeetsWellBeing || !stat_health.MeetsWellBeing;
		VignetteModel.Settings settings = postEffects.vignette.settings;
		ChromaticAberrationModel.Settings settings2 = postEffects.chromaticAberration.settings;
		ColorGradingModel.Settings settings3 = postEffects.colorGrading.settings;
		float num2 = Mathf.Sin(sineTime * sineFrequency);
		if (num)
		{
			sineTime += Time.deltaTime;
			postEffects.vignette.enabled = true;
			postEffects.chromaticAberration.enabled = true;
			settings.intensity = Helper.MapValue(-1f, 1f, num2, vignetteInterval.minValue, vignetteInterval.maxValue);
			settings2.intensity = Helper.MapValue(-1f, 1f, num2, aberationInterval.minValue, aberationInterval.maxValue);
		}
		else
		{
			if (num2 > -0.98f)
			{
				sineTime += Time.deltaTime;
			}
			settings.intensity = Mathf.Lerp(settings.intensity, 0f, Time.deltaTime);
			settings2.intensity = Mathf.Lerp(settings2.intensity, 0f, Time.deltaTime);
			if (settings.intensity < 0.05f)
			{
				settings.intensity = 0f;
				postEffects.vignette.enabled = false;
			}
			if (settings2.intensity < 0.05f)
			{
				settings2.intensity = 0f;
				postEffects.chromaticAberration.enabled = false;
			}
		}
		if (!stat_health.MeetsWellBeing)
		{
			settings3.basic.saturation = Helper.MapValue(-1f, 1f, num2 * -1f, saturationInterval.minValue, saturationInterval.maxValue);
		}
		else
		{
			settings3.basic.saturation = Mathf.Lerp(settings3.basic.saturation, 1f, Time.deltaTime);
		}
		postEffects.vignette.settings = settings;
		postEffects.chromaticAberration.settings = settings2;
		postEffects.colorGrading.settings = settings3;
	}

	private void HandleSoundFeedback()
	{
		if (playerNetwork.PlayerScript.IsDead)
		{
			if (coroutine_thirstySound != null)
			{
				StopCoroutine(coroutine_thirstySound);
				coroutine_thirstySound = null;
			}
			if (coroutine_hungrySound != null)
			{
				StopCoroutine(coroutine_hungrySound);
				coroutine_hungrySound = null;
			}
			if (coroutine_lowBreathSound != null)
			{
				StopCoroutine(coroutine_lowBreathSound);
				coroutine_lowBreathSound = null;
			}
			if (coroutine_drowningSound != null)
			{
				StopCoroutine(coroutine_drowningSound);
				coroutine_drowningSound = null;
			}
			return;
		}
		if (!stat_thirst.Normal.MeetsWellBeing)
		{
			if (coroutine_thirstySound == null)
			{
				coroutine_thirstySound = StartCoroutine(PlaySoundOnInterval(eventEmitter_Thirsty, HapticEvent.Thirsty));
			}
		}
		else if (coroutine_thirstySound != null)
		{
			StopCoroutine(coroutine_thirstySound);
			coroutine_thirstySound = null;
		}
		if (!stat_hunger.Normal.MeetsWellBeing)
		{
			if (coroutine_hungrySound == null)
			{
				coroutine_hungrySound = StartCoroutine(PlaySoundOnInterval(eventEmitter_Hungry, HapticEvent.Hungry));
			}
		}
		else if (coroutine_hungrySound != null)
		{
			StopCoroutine(coroutine_hungrySound);
			coroutine_hungrySound = null;
		}
		if (stat_oxygen.NormalValue == 0f)
		{
			if (coroutine_drowningSound == null)
			{
				coroutine_drowningSound = StartCoroutine(PlaySoundOnInterval(eventEmitter_Drowning, HapticEvent.Drowning));
			}
			return;
		}
		if (!stat_oxygen.MeetsWellBeing)
		{
			if (coroutine_lowBreathSound == null)
			{
				coroutine_lowBreathSound = StartCoroutine(PlaySoundOnInterval(eventEmitter_LowBreath, HapticEvent.LowAir));
			}
			return;
		}
		if (coroutine_lowBreathSound != null)
		{
			StopCoroutine(coroutine_lowBreathSound);
			coroutine_lowBreathSound = null;
		}
		if (coroutine_drowningSound != null)
		{
			StopCoroutine(coroutine_drowningSound);
			coroutine_drowningSound = null;
		}
	}

	private void SetOxygenUI()
	{
		float normalValue = stat_oxygen.NormalValue;
		if (canvas != null)
		{
			canvas.SetFatigueSlider(normalValue);
		}
	}

	public override void Damage(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageInflictorEntityType, bool isLocal, SO_Buff buffAsset = null)
	{
		if (playerNetwork.IsLocalPlayer && GameModeValueManager.IsPlayerInvurnerable)
		{
			return;
		}
		if (hasDeathPrevention && stat_health.Value + stat_BonusHealth.statTarget.Value - damage <= 0f)
		{
			buffManager.RemoveBuffDuration(BuffType.DeathPrevention);
			buffManager.AddBuff(deathPreventionTriggeredInvurnabilityBuff);
			buffManager.AddBuff(deathPreventionTriggeredHealEffect);
			return;
		}
		if ((damageInflictorEntityType == EntityType.Enemy || damageInflictorEntityType == EntityType.Player) && armorHandler != null)
		{
			float damageReduction = armorHandler.DamageReduction;
			if (damageReduction != 0f)
			{
				damage = Mathf.FloorToInt(damage * (1f - damageReduction));
			}
		}
		float num = damage;
		float value = stat_BonusHealth.statTarget.Value;
		if (damage > 0f && value > 0f)
		{
			if (damage > value)
			{
				stat_BonusHealth.Value -= value;
				num -= value;
			}
			else
			{
				stat_BonusHealth.Value -= damage;
				num = 0f;
			}
		}
		base.Damage(num, hitPoint, hitNormal, damageInflictorEntityType, isLocal, buffAsset);
		if (!IsDead && damage > 0f)
		{
			eventEmitter_Damaged.Play();
			eventEmitter_Damaged.SetParameter("character", voiceParameter);
		}
		if (playerNetwork.IsLocalPlayer)
		{
			if (playerNetwork.NoteBookUI != null && playerNetwork.NoteBookUI.isDisplayed && damage > 0f)
			{
				playerNetwork.NoteBookUI.ToggleBookActivity();
			}
			SimpleMonoBehaviourSingleton<HapticManager>.Instance?.PlayOneShot(HapticEvent.TakeDamage);
			if (damageInflictorEntityType == EntityType.Enemy || damageInflictorEntityType == EntityType.FallDamage || damageInflictorEntityType == EntityType.Environment)
			{
				screenEffectManager.ShakeCamera(cameraShakeSettings);
			}
			if (IsDead)
			{
				canvas.healthSlider.SetValue(0f, stat_health.Value.ToString("F1"));
				Message_NetworkBehaviour message = new Message_NetworkBehaviour(Messages.PlayerKilled, playerNetwork);
				if (Raft_Network.IsHost)
				{
					playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					playerNetwork.PlayerScript.Kill();
				}
				else
				{
					playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
		}
		if (damage > 0f && (damageInflictorEntityType == EntityType.Player || damageInflictorEntityType == EntityType.Enemy))
		{
			if (playerNetwork.IsLocalPlayer)
			{
				playerNetwork.Animator.SetAnimation(PlayerAnimation.Trigger_DamageLight);
				screenEffectManager.screenImage_Blood.Enable(this, bloodSplatterSettings);
			}
			else
			{
				bloodParticles.transform.position = hitPoint;
				bloodParticles.transform.rotation = Quaternion.LookRotation(hitNormal);
				bloodParticles.Play();
			}
		}
	}

	private IEnumerator PlaySoundOnInterval(StudioEventEmitter emitter, HapticEvent hapticEvent = HapticEvent.SharkBitesRaft_0)
	{
		while (true)
		{
			yield return new WaitForSeconds(soundIntervall);
			emitter.Play();
			switch (hapticEvent)
			{
			case HapticEvent.Thirsty:
			case HapticEvent.Drowning:
			case HapticEvent.LowAir:
				emitter.SetParameter("character", voiceParameter);
				break;
			case HapticEvent.Hungry:
				emitter.SetParameter("character", 0f);
				break;
			}
			if (hapticEvent != HapticEvent.SharkBitesRaft_0)
			{
				SimpleMonoBehaviourSingleton<HapticManager>.Instance?.PlayOneShot(hapticEvent);
			}
		}
	}

	protected override bool ShouldClearBuffs()
	{
		if (playerNetwork.PlayerScript.IsDead)
		{
			return true;
		}
		return false;
	}
}
