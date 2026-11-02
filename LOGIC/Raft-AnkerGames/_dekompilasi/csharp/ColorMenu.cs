using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorMenu : MonoBehaviour, SceneEventInterface
{
	public struct CurrentPaintCost
	{
		public List<Cost> costs;

		public int redCost;

		public int blueCost;

		public int yellowCost;

		public int whiteCost;

		public int blackCost;
	}

	public static bool UseTwoColors;

	public CostCollection cursorCostCollection;

	public ColorPicker primaryColorPicker;

	public ColorPicker secondaryColorPicker;

	[SerializeField]
	private PatternButton patternButtonPrefab;

	[SerializeField]
	private RectTransform patternButtonParent;

	[SerializeField]
	private Image secondaryButtonImage;

	[SerializeField]
	private Sprite greenButton;

	[SerializeField]
	private Sprite redButton;

	[SerializeField]
	private CanvasGroup canvasGroupSecondary;

	[SerializeField]
	[Range(0f, 1f)]
	private float disabledAlpha;

	[SerializeField]
	private List<PrevColorButton> prevColorButtons;

	[HideInInspector]
	public CurrentPaintCost currentPaintCost;

	[SerializeField]
	private GameObject gamepadSwap;

	[SerializeField]
	private Item_Base item_redPaint;

	[SerializeField]
	private Item_Base item_bluePaint;

	[SerializeField]
	private Item_Base item_yellowPaint;

	[SerializeField]
	private Item_Base item_whitePaint;

	[SerializeField]
	private Item_Base item_blackPaint;

	[SerializeField]
	private ColorCounter redCounter;

	[SerializeField]
	private ColorCounter blueCounter;

	[SerializeField]
	private ColorCounter yellowCounter;

	[SerializeField]
	private ColorCounter whiteCounter;

	[SerializeField]
	private ColorCounter blackCounter;

	private static SO_Pattern[] patterns;

	private Network_Player localPlayer;

	private List<PatternButton> patternButtons = new List<PatternButton>();

	[SerializeField]
	private GameObject[] gamepadObjects;

	[SerializeField]
	private GameObject[] keyboardObjects;

	private void Awake()
	{
		ComponentManager<ColorMenu>.Value = this;
		if (patterns == null)
		{
			patterns = Resources.LoadAll<SO_Pattern>("Patterns");
		}
		if (patterns == null)
		{
			return;
		}
		for (int i = 0; i < patterns.Length; i++)
		{
			PatternButton patternButton = UnityEngine.Object.Instantiate(patternButtonPrefab, patternButtonParent);
			patternButton.AttachPattern(patterns[i]);
			patternButton.SetSelectedState(state: false);
			if (i == 0)
			{
				PatternButton.CurrentButton = patternButton;
				patternButton.SetSelectedState(state: true);
			}
			patternButtons.Add(patternButton);
		}
	}

	private void Start()
	{
		ColorPicker colorPicker = primaryColorPicker;
		colorPicker.OnPickColor = (Action)Delegate.Combine(colorPicker.OnPickColor, new Action(RefreshPaintCost));
		ColorPicker colorPicker2 = secondaryColorPicker;
		colorPicker2.OnPickColor = (Action)Delegate.Combine(colorPicker2.OnPickColor, new Action(RefreshPaintCost));
		redCounter.Select(0, 0);
		blueCounter.Select(0, 0);
		yellowCounter.Select(0, 0);
		whiteCounter.Select(0, 0);
		blackCounter.Select(0, 0);
		localPlayer = ComponentManager<Network_Player>.Value;
		CanvasHelper.MenuOpenEvent += OnMenuOpen;
		CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
		instance.OnControlsChangedEvent = (Action<bool>)Delegate.Combine(instance.OnControlsChangedEvent, new Action<bool>(ControlsChangedEvent));
		ControlsChangedEvent(SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad);
	}

	private void ControlsChangedEvent(bool gamepad)
	{
		GameObject[] array = gamepadObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(gamepad);
		}
		array = keyboardObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(!gamepad);
		}
	}

	private void OnDestroy()
	{
		CanvasHelper.MenuOpenEvent -= OnMenuOpen;
		if ((bool)SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance)
		{
			CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
			instance.OnControlsChangedEvent = (Action<bool>)Delegate.Remove(instance.OnControlsChangedEvent, new Action<bool>(ControlsChangedEvent));
		}
	}

	private void OnMenuOpen(MenuType type)
	{
		if (type == MenuType.PaintMenu)
		{
			RefreshPaintCost();
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		UseTwoColors = false;
	}

	public void Button_Secondary()
	{
		UseTwoColors = !UseTwoColors;
		secondaryButtonImage.sprite = (UseTwoColors ? greenButton : redButton);
		canvasGroupSecondary.alpha = (UseTwoColors ? 1f : disabledAlpha);
		canvasGroupSecondary.blocksRaycasts = UseTwoColors;
		RefreshPaintCost();
	}

	public void Update()
	{
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			gamepadSwap.SetActive(UseTwoColors && primaryColorPicker.SelectedColorValue != secondaryColorPicker.SelectedColorValue);
		}
	}

	public void Button_SwapColors()
	{
		SO_ColorValue selectedColorValue = primaryColorPicker.SelectedColorValue;
		primaryColorPicker.SelectButtonFromColor(secondaryColorPicker.SelectedColorValue);
		secondaryColorPicker.SelectButtonFromColor(selectedColorValue);
	}

	public void Button_PrevColorPick(PrevColorButton prevColorButton)
	{
		if (!(prevColorButton.primaryColor != null) || !(prevColorButton.secondaryColor != null))
		{
			return;
		}
		primaryColorPicker.SelectButtonFromColor(prevColorButton.primaryColor);
		secondaryColorPicker.SelectButtonFromColor(prevColorButton.secondaryColor);
		foreach (PatternButton patternButton in patternButtons)
		{
			if (patternButton != null && patternButton.pattern == prevColorButton.pattern)
			{
				patternButton.OnClick();
				break;
			}
		}
	}

	public void OnPaintbrushPaint(SO_ColorValue primary, SO_ColorValue secondary, uint patternIndex)
	{
		if (prevColorButtons == null)
		{
			prevColorButtons = new List<PrevColorButton>();
		}
		if (primary != null && primary.uniqueColorIndex == 0 && secondary != null && secondary.uniqueColorIndex == 0)
		{
			return;
		}
		SO_Pattern patternFromIndex = GetPatternFromIndex(patternIndex);
		foreach (PrevColorButton prevColorButton2 in prevColorButtons)
		{
			if (prevColorButton2.primaryColor == primary && prevColorButton2.secondaryColor == secondary && prevColorButton2.pattern == patternFromIndex)
			{
				return;
			}
		}
		PrevColorButton prevColorButton = prevColorButtons[0];
		prevColorButtons.Remove(prevColorButton);
		prevColorButtons.Add(prevColorButton);
		prevColorButton.SetColors(primary, secondary, patternFromIndex);
		prevColorButton.transform.SetAsFirstSibling();
	}

	public bool HasEnoughMaterialsToPaint()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (localPlayer == null || localPlayer.Inventory == null)
		{
			return false;
		}
		if (Cheat.UseGodMode)
		{
			return true;
		}
		bool result = true;
		if (localPlayer.Inventory.GetItemCount(item_redPaint) < currentPaintCost.redCost || localPlayer.Inventory.GetItemCount(item_bluePaint) < currentPaintCost.blueCost || localPlayer.Inventory.GetItemCount(item_yellowPaint) < currentPaintCost.yellowCost || localPlayer.Inventory.GetItemCount(item_whitePaint) < currentPaintCost.whiteCost || localPlayer.Inventory.GetItemCount(item_blackPaint) < currentPaintCost.blackCost)
		{
			result = false;
		}
		return result;
	}

	public SO_Pattern GetPatternFromIndex(uint uniquePatternIndex)
	{
		foreach (PatternButton patternButton in patternButtons)
		{
			if (patternButton != null && patternButton.pattern.uniquePatternIndex == uniquePatternIndex)
			{
				return patternButton.pattern;
			}
		}
		return null;
	}

	public void SelectPatternButtonFromIndex(uint uniquePatternIndex)
	{
		foreach (PatternButton patternButton in patternButtons)
		{
			if (patternButton != null && patternButton.pattern.uniquePatternIndex == uniquePatternIndex)
			{
				patternButton.OnClick();
				break;
			}
		}
	}

	public void RefreshPaintCost()
	{
		if (primaryColorPicker.SelectedColorValue == null || secondaryColorPicker.SelectedColorValue == null)
		{
			return;
		}
		if (UseTwoColors)
		{
			currentPaintCost.costs = GetCombinedCost(primaryColorPicker.SelectedColorValue, secondaryColorPicker.SelectedColorValue);
		}
		else
		{
			if (currentPaintCost.costs == null)
			{
				currentPaintCost.costs = new List<Cost>();
			}
			currentPaintCost.costs.Clear();
			foreach (Cost item in primaryColorPicker.SelectedColorValue.PaintCost)
			{
				currentPaintCost.costs.Add(new Cost(item));
			}
		}
		currentPaintCost.redCost = 0;
		currentPaintCost.blueCost = 0;
		currentPaintCost.yellowCost = 0;
		currentPaintCost.whiteCost = 0;
		currentPaintCost.blackCost = 0;
		foreach (Cost cost in currentPaintCost.costs)
		{
			if (UseTwoColors)
			{
				cost.amount = Mathf.FloorToInt((float)cost.amount / 2f);
			}
			int currentPreviewSide = PaintBrush.PreviewData.currentPreviewSide;
			if (currentPreviewSide == 1 || currentPreviewSide == 2)
			{
				cost.amount = Mathf.CeilToInt((float)cost.amount / 2f);
			}
			if (cost.item == item_redPaint)
			{
				currentPaintCost.redCost += cost.amount;
			}
			else if (cost.item == item_bluePaint)
			{
				currentPaintCost.blueCost += cost.amount;
			}
			else if (cost.item == item_yellowPaint)
			{
				currentPaintCost.yellowCost += cost.amount;
			}
			else if (cost.item == item_whitePaint)
			{
				currentPaintCost.whiteCost += cost.amount;
			}
			else if (cost.item == item_blackPaint)
			{
				currentPaintCost.blackCost += cost.amount;
			}
		}
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (redCounter.canvasgroup.gameObject.activeInHierarchy && localPlayer != null && localPlayer.Inventory != null)
		{
			redCounter.Select(currentPaintCost.redCost, localPlayer.Inventory.GetItemCount(item_redPaint.UniqueName));
			blueCounter.Select(currentPaintCost.blueCost, localPlayer.Inventory.GetItemCount(item_bluePaint.UniqueName));
			yellowCounter.Select(currentPaintCost.yellowCost, localPlayer.Inventory.GetItemCount(item_yellowPaint.UniqueName));
			whiteCounter.Select(currentPaintCost.whiteCost, localPlayer.Inventory.GetItemCount(item_whitePaint.UniqueName));
			blackCounter.Select(currentPaintCost.blackCost, localPlayer.Inventory.GetItemCount(item_blackPaint.UniqueName));
		}
	}

	public void RefreshCursorCostCollection()
	{
		RefreshPaintCost();
		if (currentPaintCost.costs != null)
		{
			cursorCostCollection.ShowCost(currentPaintCost.costs);
		}
	}

	private List<Cost> GetCombinedCost(SO_ColorValue colorA, SO_ColorValue colorB)
	{
		if (colorA == null || colorB == null)
		{
			return null;
		}
		List<Cost> paintCost = colorA.PaintCost;
		List<Cost> paintCost2 = colorB.PaintCost;
		List<Cost> list = new List<Cost>();
		foreach (Cost item in paintCost)
		{
			list.Add(new Cost(item.item, item.amount));
		}
		foreach (Cost cost in paintCost2)
		{
			Cost cost2 = list.Find((Cost c) => c.item == cost.item);
			if (cost2 != null)
			{
				cost2.amount += cost.amount;
			}
			else
			{
				list.Add(new Cost(cost));
			}
		}
		return list;
	}
}
