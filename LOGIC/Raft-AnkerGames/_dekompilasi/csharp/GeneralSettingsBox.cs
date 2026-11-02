using UnityEngine;
using UnityEngine.UI;

public class GeneralSettingsBox : OptionsMenuBox
{
	public static bool AllowQuickBuild;

	public static bool ShowNameTag;

	public static bool AutoSave;

	public static bool MotionSickness = false;

	public static bool ChatVisible = true;

	public static bool HelpfulUIVisible = true;

	public static bool CursorInverted = true;

	public static bool AutoPlayNotes = true;

	[SerializeField]
	private Texture2D invertedCursorTexture;

	[SerializeField]
	private Toggle toggle_QuickBuild;

	[SerializeField]
	private Toggle toggle_ShowNameTag;

	[SerializeField]
	private Toggle toggle_AutoSave;

	[SerializeField]
	private Toggle toggle_MotionSickness;

	[SerializeField]
	private Toggle toggle_ChatVisibility;

	[SerializeField]
	private Toggle toggle_HelpfulUI;

	[SerializeField]
	private Toggle toggle_CursorInvert;

	[SerializeField]
	private Toggle toggle_AutoPlayNotes;

	[SerializeField]
	private GameObject privacy;

	[SerializeField]
	private GameObject cursor;

	[SerializeField]
	private Dropdown dropdown_language;

	public override void SetFocus(bool gamepad)
	{
		dropdown_language.Select();
	}

	private void Awake()
	{
		toggle_QuickBuild.onValueChanged.AddListener(delegate
		{
			QuickBuildChanged();
		});
		toggle_ShowNameTag.onValueChanged.AddListener(delegate
		{
			ShowNameTagChanged();
		});
		toggle_AutoSave.onValueChanged.AddListener(delegate
		{
			AutoSaveChanged();
		});
		toggle_MotionSickness.onValueChanged.AddListener(delegate
		{
			MotionSicknessChanged();
		});
		toggle_ChatVisibility.onValueChanged.AddListener(delegate
		{
			ChatVisiblityChanged();
		});
		toggle_HelpfulUI.onValueChanged.AddListener(delegate
		{
			HelpfulUIVisiblityChanged();
		});
		toggle_CursorInvert.onValueChanged.AddListener(delegate
		{
			CursorInvertChanged();
		});
		toggle_AutoPlayNotes.onValueChanged.AddListener(delegate
		{
			AutoPlayNotesChanged();
		});
	}

	public override void Save(RGD_Settings settings)
	{
		base.Save(settings);
		RGD_Settings_General general = settings.general;
		general.quickBuild = toggle_QuickBuild.isOn;
		general.showNameTag = toggle_ShowNameTag.isOn;
		general.autoSave = toggle_AutoSave.isOn;
		general.motionSickness = toggle_MotionSickness.isOn;
		general.chatVisible = toggle_ChatVisibility.isOn;
		general.helpfulUIVisible = toggle_HelpfulUI.isOn;
		general.cursorInvert = toggle_CursorInvert.isOn;
		general.autoPlayNotes = toggle_AutoPlayNotes.isOn;
	}

	public override void Load(RGD_Settings settings)
	{
		base.Load(settings);
		RGD_Settings_General general = settings.general;
		toggle_QuickBuild.isOn = (AllowQuickBuild = general.quickBuild);
		toggle_ShowNameTag.isOn = (ShowNameTag = general.showNameTag);
		toggle_AutoSave.isOn = (AutoSave = general.autoSave);
		toggle_MotionSickness.isOn = (MotionSickness = general.motionSickness);
		toggle_ChatVisibility.isOn = (ChatVisible = general.chatVisible);
		toggle_HelpfulUI.isOn = (HelpfulUIVisible = general.helpfulUIVisible);
		toggle_CursorInvert.isOn = (CursorInverted = general.cursorInvert);
		toggle_AutoPlayNotes.isOn = (AutoPlayNotes = general.autoPlayNotes);
		RefreshCursorInvert();
	}

	private void QuickBuildChanged()
	{
		AllowQuickBuild = toggle_QuickBuild.isOn;
	}

	private void ShowNameTagChanged()
	{
		ShowNameTag = toggle_ShowNameTag.isOn;
		ApplyNameTagSettingToGame();
	}

	private void AutoSaveChanged()
	{
		AutoSave = toggle_AutoSave.isOn;
	}

	private void MotionSicknessChanged()
	{
		MotionSickness = toggle_MotionSickness.isOn;
	}

	private void ChatVisiblityChanged()
	{
		ChatVisible = toggle_ChatVisibility.isOn;
	}

	private void HelpfulUIVisiblityChanged()
	{
		HelpfulUIVisible = toggle_HelpfulUI.isOn;
	}

	private void CursorInvertChanged()
	{
		CursorInverted = toggle_CursorInvert.isOn;
		RefreshCursorInvert();
	}

	private void AutoPlayNotesChanged()
	{
		AutoPlayNotes = toggle_AutoPlayNotes.isOn;
	}

	private void ApplyNameTagSettingToGame()
	{
		if (Raft_Network.InMenuScene)
		{
			return;
		}
		Raft_Network value = ComponentManager<Raft_Network>.Value;
		if (!(value != null))
		{
			return;
		}
		foreach (Network_Player value2 in value.remoteUsers.Values)
		{
			if (!(value2 == null))
			{
				value2.SetNameTagVisibility(toggle_ShowNameTag.isOn);
			}
		}
	}

	private void RefreshCursorInvert()
	{
		if (CursorInverted)
		{
			Cursor.SetCursor(invertedCursorTexture, new Vector2(44f, 0f), CursorMode.Auto);
		}
		else
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}
	}
}
