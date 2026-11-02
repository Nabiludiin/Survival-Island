using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaintBrush : UsableTool
{
	public struct PreviewPaintData
	{
		public Block block;

		public SO_ColorValue colorA;

		public SO_ColorValue colorB;

		public SO_ColorValue patternColorA;

		public SO_ColorValue patternColorB;

		public uint patternIndex1;

		public uint patternIndex2;

		public int currentPreviewSide;

		public void PreviewSide(Block block, SO_ColorValue primary, SO_ColorValue secondary, int paintSide, uint patternIndex)
		{
			if (this.block != block)
			{
				if (currentPreviewSide != -1)
				{
					ResetPreview();
				}
				if (block != null)
				{
					this.block = block;
					colorA = block.currentColorA;
					colorB = block.currentColorB;
					patternColorA = block.currentPatternColorA;
					patternColorB = block.currentPatternColorB;
					patternIndex1 = block.currentPatternIndex1;
					patternIndex2 = block.currentPatternIndex2;
					currentPreviewSide = -1;
				}
			}
			if (!(this.block != null))
			{
				return;
			}
			if (currentPreviewSide != paintSide)
			{
				if (currentPreviewSide != -1)
				{
					ResetPreview();
				}
				currentPreviewSide = paintSide;
			}
			if (this.block != null)
			{
				this.block.SetInstanceColorAndPattern(primary, secondary, paintSide, patternIndex);
			}
		}

		public void ResetPreview()
		{
			if (!(block == null))
			{
				if (currentPreviewSide == 1)
				{
					block.SetInstanceColorAndPattern(colorA, patternColorA, 1, patternIndex1);
				}
				if (currentPreviewSide == 2)
				{
					block.SetInstanceColorAndPattern(colorB, patternColorB, 2, patternIndex2);
				}
				if (currentPreviewSide == 3)
				{
					block.SetInstanceColorAndPattern(colorA, patternColorA, 1, patternIndex1);
					block.SetInstanceColorAndPattern(colorB, patternColorB, 2, patternIndex2);
				}
				block = null;
				currentPreviewSide = -1;
				colorA = null;
				colorB = null;
				patternColorA = null;
				patternColorB = null;
				patternIndex1 = 0u;
				patternIndex2 = 0u;
			}
		}

		public void OnBlockPainted()
		{
			colorA = block.currentColorA;
			colorB = block.currentColorB;
			patternColorA = block.currentPatternColorA;
			patternColorB = block.currentPatternColorB;
			patternIndex1 = block.currentPatternIndex1;
			patternIndex2 = block.currentPatternIndex2;
		}
	}

	public static PreviewPaintData PreviewData;

	public ParticleSystem paintParticles;

	public MeshRenderer blob_primary;

	public MeshRenderer blob_primary_half;

	public MeshRenderer blob_secondary;

	public MeshRenderer blob_secondary_half;

	public EventReference er_paint;

	private CanvasHelper canvas;

	private ColorMenu colorMenu;

	private ParticleSystem[] particleSystems;

	private Block aimedAtBlock;

	private Transform lockedPivot;

	private MaterialPropertyBlock propertyBlock;

	private bool singleSidedPainting;

	protected override void Start()
	{
		base.Start();
		canvas = ComponentManager<CanvasHelper>.Value;
		lockedPivot = SingletonGeneric<GameManager>.Singleton.lockedPivot;
		particleSystems = paintParticles.GetComponentsInChildren<ParticleSystem>();
	}

	protected override bool Update()
	{
		if (!playerNetwork.IsLocalPlayer)
		{
			return false;
		}
		if (colorMenu == null)
		{
			colorMenu = ComponentManager<ColorMenu>.Value;
		}
		if (!colorMenu.cursorCostCollection.gameObject.activeInHierarchy)
		{
			colorMenu.cursorCostCollection.gameObject.SetActive(value: true);
		}
		if (CanvasHelper.ActiveMenu != MenuType.None && colorMenu.cursorCostCollection.gameObject.activeInHierarchy)
		{
			colorMenu.cursorCostCollection.gameObject.SetActive(value: false);
		}
		else
		{
			colorMenu.RefreshCursorCostCollection();
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Context"];
		if (CanvasHelper.ActiveMenu == MenuType.None)
		{
			canvas.displayTextManager.ShowText(Helper.GetTerm("Game/ColorMenu/MoreColors"), action.GetBindingDisplayString(), MyInput.Keybinds["RMB"].MainKey, 1, 0, clearAllTexts: false);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "RMB"))
			{
				canvas.OpenMenuCloseOther(MenuType.PaintMenu);
				ComponentManager<CanvasHelper>.Value.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Paint);
			}
			else
			{
				InputAction action2 = PlayerInput.GetPlayerByIndex(0).actions["Rotate"];
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/ColorMenu/PaintOneSide"), action2.GetBindingDisplayString(), MyInput.Keybinds["PaintOneSide"].MainKey, 2, 0, clearAllTexts: false);
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action2, "PaintOneSide"))
				{
					singleSidedPainting = !singleSidedPainting;
					RuntimeManager.PlayOneShot(er_paint, transform.position);
				}
				if (Helper.HitAtCursor(out var hit, Player.UseDistance * 1.5f, LayerMasks.MASK_Block))
				{
					aimedAtBlock = hit.transform.GetComponentInParent<Block>();
				}
				else
				{
					aimedAtBlock = null;
				}
				if (aimedAtBlock != null)
				{
					InputAction action3 = PlayerInput.GetPlayerByIndex(0).actions["Special"];
					if (aimedAtBlock.buildableItem.settings_buildable.PrimaryPaintAxis != Axis.None)
					{
						canvas.displayTextManager.ShowText(Helper.GetTerm("Controls/ColorPick"), action3.GetBindingDisplayString(), MyInput.Keybinds["ColorPick"].MainKey, 3, 0, clearAllTexts: false);
					}
					else
					{
						canvas.displayTextManager.HideDisplayTexts(3);
					}
					if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action3, "ColorPick"))
					{
						PreviewData.ResetPreview();
						ColorPickFromBlock(aimedAtBlock, GetPaintSideFromPaintAxis(aimedAtBlock, hit, careAboutSingleSidedSetting: false));
					}
				}
				else
				{
					canvas.displayTextManager.HideDisplayTexts(3);
				}
				PreviewPaint(aimedAtBlock, GetPaintSideFromPaintAxis(aimedAtBlock, hit, careAboutSingleSidedSetting: true));
			}
		}
		else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(action, "RMB") && CanvasHelper.ActiveMenu == MenuType.PaintMenu)
		{
			canvas.CloseMenu(MenuType.PaintMenu);
			canvas.displayTextManager.HideDisplayTexts(1);
			canvas.displayTextManager.HideDisplayTexts(2);
			canvas.displayTextManager.HideDisplayTexts(3);
			ComponentManager<CanvasHelper>.Value.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Inventory);
		}
		HandleBlobs();
		return true;
	}

	protected override void OnToolDeSelect()
	{
		if (playerNetwork != null && playerNetwork.IsLocalPlayer)
		{
			if (canvas != null)
			{
				canvas.CloseMenu(MenuType.PaintMenu);
				canvas.displayTextManager.HideDisplayTexts(1);
				canvas.displayTextManager.HideDisplayTexts(2);
				canvas.displayTextManager.HideDisplayTexts(3);
			}
			if (colorMenu != null)
			{
				colorMenu.cursorCostCollection.gameObject.SetActive(value: false);
			}
			PreviewData.ResetPreview();
		}
	}

	protected override void OnToolUse()
	{
		if (colorMenu == null)
		{
			colorMenu = ComponentManager<ColorMenu>.Value;
		}
		SO_ColorValue selectedColorValue = colorMenu.primaryColorPicker.SelectedColorValue;
		SO_ColorValue sO_ColorValue = (ColorMenu.UseTwoColors ? colorMenu.secondaryColorPicker.SelectedColorValue : selectedColorValue);
		uint patternIndex = ((PatternButton.CurrentButton != null) ? PatternButton.CurrentButton.pattern.uniquePatternIndex : 0u);
		if (selectedColorValue == null || sO_ColorValue == null || !colorMenu.HasEnoughMaterialsToPaint())
		{
			return;
		}
		if (Helper.HitAtCursor(out var hit, Player.UseDistance * 1.5f, LayerMasks.MASK_Block))
		{
			Block componentInParent = hit.transform.GetComponentInParent<Block>();
			if (componentInParent == null)
			{
				return;
			}
			PreviewData.ResetPreview();
			int paintSideFromPaintAxis = GetPaintSideFromPaintAxis(componentInParent, hit, careAboutSingleSidedSetting: true);
			if (!componentInParent.buildableItem.settings_buildable.Paintable || componentInParent.HasColor(selectedColorValue, sO_ColorValue, paintSideFromPaintAxis, patternIndex))
			{
				return;
			}
			Vector3 vector = playerNetwork.Camera.transform.position - hit.point;
			Vector3 vector2 = lockedPivot.InverseTransformPoint(hit.point + vector.normalized * 0.25f);
			Message_PaintBlock message = new Message_PaintBlock(Messages.PaintBlock, playerNetwork, componentInParent.ObjectIndex, vector2, selectedColorValue, sO_ColorValue, paintSideFromPaintAxis, patternIndex);
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				PaintBlock(componentInParent, vector2, selectedColorValue, sO_ColorValue, paintSideFromPaintAxis, patternIndex, skipValidChecks: true);
			}
			else
			{
				playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		playerAnimator.SetAnimation(PlayerAnimation.Trigger_ItemHitTrigger);
	}

	public bool PaintBlock(Block block, Vector3 localLockedPivotPosition, SO_ColorValue primary, SO_ColorValue secondary, int paintSide, uint patternIndex, bool skipValidChecks)
	{
		if (block == null)
		{
			return false;
		}
		if (primary == null || secondary == null)
		{
			return false;
		}
		if (!skipValidChecks && (!block.buildableItem.settings_buildable.Paintable || block.HasColor(primary, secondary, paintSide, patternIndex)))
		{
			return false;
		}
		if (playerNetwork.IsLocalPlayer)
		{
			if (!Cheat.UseGodMode)
			{
				playerNetwork.Inventory.RemoveItem("Color_Red", colorMenu.currentPaintCost.redCost);
				playerNetwork.Inventory.RemoveItem("Color_Yellow", colorMenu.currentPaintCost.yellowCost);
				playerNetwork.Inventory.RemoveItem("Color_Blue", colorMenu.currentPaintCost.blueCost);
				playerNetwork.Inventory.RemoveItem("Color_White", colorMenu.currentPaintCost.whiteCost);
				playerNetwork.Inventory.RemoveItem("Color_Black", colorMenu.currentPaintCost.blackCost);
			}
			AchievementHandler.AddBuildPaintCount();
		}
		block.SetInstanceColorAndPattern(primary, secondary, paintSide, patternIndex);
		if (PreviewData.block == block)
		{
			PreviewData.OnBlockPainted();
		}
		paintParticles.transform.SetParentSafe(lockedPivot);
		paintParticles.transform.localPosition = localLockedPivotPosition;
		paintParticles.transform.LookAt(playerNetwork.transform, Vector3.up);
		paintParticles.Play();
		RuntimeManager.PlayOneShot(er_paint, block.transform.position);
		if (particleSystems != null && particleSystems.Length != 0)
		{
			ParticleSystem[] array = particleSystems;
			foreach (ParticleSystem particleSystem in array)
			{
				if (!(particleSystem == null))
				{
					ParticleSystem.MainModule main = particleSystem.main;
					main.startColor = primary.buttonColor;
				}
			}
		}
		if (colorMenu != null)
		{
			colorMenu.OnPaintbrushPaint(primary, secondary, patternIndex);
		}
		return true;
	}

	private void PreviewPaint(Block block, int paintSide)
	{
		SO_ColorValue selectedColorValue = colorMenu.primaryColorPicker.SelectedColorValue;
		SO_ColorValue sO_ColorValue = (ColorMenu.UseTwoColors ? colorMenu.secondaryColorPicker.SelectedColorValue : selectedColorValue);
		uint patternIndex = ((PatternButton.CurrentButton != null) ? PatternButton.CurrentButton.pattern.uniquePatternIndex : 0u);
		if (selectedColorValue == null || sO_ColorValue == null)
		{
			return;
		}
		if (colorMenu.HasEnoughMaterialsToPaint())
		{
			if (block != null)
			{
				if (block == null)
				{
					PreviewData.ResetPreview();
				}
				else if (!block.buildableItem.settings_buildable.Paintable)
				{
					PreviewData.ResetPreview();
				}
				else
				{
					PreviewData.PreviewSide(block, selectedColorValue, sO_ColorValue, paintSide, patternIndex);
				}
			}
			else
			{
				PreviewData.ResetPreview();
			}
		}
		else
		{
			PreviewData.ResetPreview();
		}
	}

	private int GetPaintSideFromPaintAxis(Block block, RaycastHit hit, bool careAboutSingleSidedSetting)
	{
		if (block == null)
		{
			return 0;
		}
		if (careAboutSingleSidedSetting && !singleSidedPainting)
		{
			return 3;
		}
		Vector3 zero = Vector3.zero;
		Vector3 vector = Vector3.zero;
		switch (block.buildableItem.settings_buildable.PaintMode)
		{
		case PaintMode.CameraPosition:
			vector = block.transform.InverseTransformPoint(playerNetwork.CameraTransform.position);
			break;
		case PaintMode.LocalNormal:
			vector = block.transform.InverseTransformDirection(hit.normal);
			break;
		}
		switch (block.buildableItem.settings_buildable.PrimaryPaintAxis)
		{
		case Axis.X:
			if (vector.x >= zero.x)
			{
				return 1;
			}
			return 2;
		case Axis.NX:
			if (vector.x < zero.x)
			{
				return 1;
			}
			return 2;
		case Axis.Y:
			if (vector.y <= zero.y)
			{
				return 1;
			}
			return 2;
		case Axis.NY:
			if (vector.y > zero.y)
			{
				return 1;
			}
			return 2;
		case Axis.Z:
			if (vector.z >= zero.z)
			{
				return 1;
			}
			return 2;
		case Axis.NZ:
			if (vector.z < zero.z)
			{
				return 1;
			}
			return 2;
		case Axis.All:
			return 3;
		case Axis.None:
			return 0;
		default:
			return 3;
		}
	}

	private void ColorPickFromBlock(Block block, int paintSide)
	{
		SO_ColorValue sO_ColorValue = null;
		SO_ColorValue sO_ColorValue2 = null;
		int num = -1;
		if (paintSide == 1)
		{
			sO_ColorValue = block.currentColorA;
			sO_ColorValue2 = block.currentPatternColorA;
			num = (int)block.currentPatternIndex1;
		}
		if (paintSide == 2)
		{
			sO_ColorValue = block.currentColorB;
			sO_ColorValue2 = block.currentPatternColorB;
			num = (int)block.currentPatternIndex2;
		}
		if (paintSide == 3)
		{
			if (block.buildableItem.settings_buildable.DecorationPaint)
			{
				sO_ColorValue = block.currentColorA;
				sO_ColorValue2 = block.currentColorB;
				num = (int)block.currentPatternIndex1;
			}
			else
			{
				sO_ColorValue = block.currentColorB;
				sO_ColorValue2 = block.currentPatternColorA;
				num = (int)block.currentPatternIndex1;
			}
		}
		if (sO_ColorValue != null)
		{
			colorMenu.primaryColorPicker.SelectButtonFromColor(sO_ColorValue);
		}
		else
		{
			colorMenu.primaryColorPicker.SelectButtonFromColor(ColorPicker.GetColorFromUniqueIndex(0u));
		}
		if (sO_ColorValue2 != null)
		{
			colorMenu.secondaryColorPicker.SelectButtonFromColor(sO_ColorValue2);
		}
		else
		{
			colorMenu.secondaryColorPicker.SelectButtonFromColor(ColorPicker.GetColorFromUniqueIndex(0u));
		}
		if (num >= 0 && sO_ColorValue != null && sO_ColorValue2 != null && sO_ColorValue != sO_ColorValue2)
		{
			colorMenu.SelectPatternButtonFromIndex((uint)num);
		}
		if (sO_ColorValue != sO_ColorValue2 && !ColorMenu.UseTwoColors)
		{
			colorMenu.Button_Secondary();
		}
	}

	private void HandleBlobs()
	{
		if (!(colorMenu == null))
		{
			SO_ColorValue selectedColorValue = colorMenu.primaryColorPicker.SelectedColorValue;
			SO_ColorValue color = colorMenu.secondaryColorPicker.SelectedColorValue;
			if (!ColorMenu.UseTwoColors)
			{
				color = selectedColorValue;
			}
			if (!singleSidedPainting)
			{
				SetBlobValue(blob_primary, null);
				SetBlobValue(blob_secondary, null);
				SetBlobValue(blob_primary_half, selectedColorValue);
				SetBlobValue(blob_secondary_half, color);
			}
			else
			{
				SetBlobValue(blob_primary, selectedColorValue);
				SetBlobValue(blob_secondary, color);
				SetBlobValue(blob_primary_half, null);
				SetBlobValue(blob_secondary_half, null);
			}
		}
	}

	private void SetBlobValue(MeshRenderer blob, SO_ColorValue color)
	{
		if (propertyBlock == null)
		{
			propertyBlock = new MaterialPropertyBlock();
		}
		if (color == null || color.uniqueColorIndex == 0)
		{
			blob.enabled = false;
			return;
		}
		blob.enabled = true;
		blob.GetPropertyBlock(propertyBlock);
		propertyBlock.SetColor("_Side1_Base", color.paintColor);
		propertyBlock.SetColor("_Side2_Base", color.paintColor);
		propertyBlock.SetFloat("_PaintSide", 0f);
		propertyBlock.SetFloat("_DecoPaintSelect", 3f);
		blob.SetPropertyBlock(propertyBlock);
	}
}
