using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class HandsController : MonoBehaviour
{
	public HandUI HandPrefab;

	[SerializeField]
	private HandUI[] _hands;

	[SerializeField]
	private Color32 _secondIconColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 120);

	[SerializeField]
	private Sprite _hand;

	private int _str;

	private int lastPos;

	public int AvailableHands => _str - lastPos;

	public bool AreHandsFull => AvailableHands == 0;

	private void Awake()
	{
		Hide();
	}

	public void Show()
	{
		gameObject.SetActive(value: true);
		for (int i = 0; i < _str; i++)
		{
			_hands[i].Show(i);
		}
	}

	public void Hide()
	{
		for (int i = 0; i < _hands.Length; i++)
		{
			_hands[i].Hide();
		}
	}

	public bool IsHoldingSomething()
	{
		return lastPos > 0;
	}

	public void SetHands(ScavangeItemData itemData)
	{
		int weight = itemData.Weight;
		if (lastPos + weight > _hands.Length)
		{
			return;
		}
		for (int i = 0; i < weight; i++)
		{
			Color color = Color.white;
			if (i > 0)
			{
				color = _secondIconColor;
			}
			_hands[lastPos].SetHands(itemData, i, color);
			lastPos++;
		}
	}

	public void NotEnoughHands()
	{
		for (int i = 0; i < _str; i++)
		{
			_hands[i].NotEnoughHands();
		}
	}

	public void NoItemAvailable()
	{
		for (int i = 0; i < _str; i++)
		{
			_hands[i].NoItemAvailable();
		}
	}

	public void ClearHands()
	{
		lastPos = 0;
		HandUI[] hands = _hands;
		for (int i = 0; i < hands.Length; i++)
		{
			hands[i].ClearHands(_hand);
		}
	}

	public void AbouToClearHands()
	{
		HandUI[] hands = _hands;
		for (int i = 0; i < hands.Length; i++)
		{
			hands[i].AboutToClearHands(_hand);
		}
	}

	internal void Init(ScavengeManager scavenge, ActorBrain character)
	{
		_str = character.HandsCount;
		_hand = character.handSprite;
		for (int i = 0; i < _str; i++)
		{
			_hands[i].Init(scavenge.GetPerWeightDelay(), scavenge.GetPickUpDelay());
		}
		for (int j = _str; j < _hands.Length; j++)
		{
			_hands[j].gameObject.SetActive(value: false);
		}
		ClearHands();
	}
}
