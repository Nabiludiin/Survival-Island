using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffDisplayer : MonoBehaviour
{
	[Serializable]
	private class DisplayTypeCon
	{
		public BuffDisplayType type;

		public Sprite sprite;
	}

	[SerializeField]
	private Transform timerParent;

	[SerializeField]
	private BuffDisplayObject timerPrefab;

	[SerializeField]
	private Color goodColor;

	[SerializeField]
	private Color badColor;

	[SerializeField]
	private List<DisplayTypeCon> spriteLibrary = new List<DisplayTypeCon>();

	private List<BuffDisplayObject> currentObjects = new List<BuffDisplayObject>();

	private void Awake()
	{
		ComponentManager<BuffDisplayer>.Value = this;
	}

	public void ClearAllBuffDisplays()
	{
		int num;
		for (num = 0; num < currentObjects.Count; num++)
		{
			BuffDisplayObject buffDisplayObject = currentObjects[num];
			if (buffDisplayObject != null && buffDisplayObject.OnTimerExpire != null)
			{
				buffDisplayObject.OnTimerExpire();
			}
			else
			{
				currentObjects.RemoveAt(num);
			}
			num--;
		}
	}

	public BuffDisplayObject DisplayBuff(BuffDisplayType type, float duration, float progress)
	{
		BuffDisplayObject obj = GetDisplayObjectFromType(type);
		if (obj != null && !obj.isExpired)
		{
			obj.SetNewDurationAndProgress(duration, progress);
		}
		else
		{
			bool flag = type.ToString().EndsWith("_Good");
			obj = UnityEngine.Object.Instantiate(timerPrefab, timerParent);
			obj.StartTimer(duration, progress, type, GetSpriteFromLibrary(type), flag ? goodColor : badColor);
			BuffDisplayObject buffDisplayObject = obj;
			buffDisplayObject.OnTimerExpire = (Action)Delegate.Combine(buffDisplayObject.OnTimerExpire, (Action)(() =>
			{
				currentObjects.Remove(obj);
				UnityEngine.Object.Destroy(obj.gameObject);
			}));
			currentObjects.Add(obj);
		}
		return obj;
	}

	public BuffDisplayObject GetDisplayObjectFromType(BuffDisplayType type)
	{
		foreach (BuffDisplayObject currentObject in currentObjects)
		{
			if (currentObject.type == type)
			{
				return currentObject;
			}
		}
		return null;
	}

	public Sprite GetSpriteFromLibrary(BuffDisplayType type)
	{
		foreach (DisplayTypeCon item in spriteLibrary)
		{
			if (item.type == type)
			{
				return item.sprite;
			}
		}
		return null;
	}
}
