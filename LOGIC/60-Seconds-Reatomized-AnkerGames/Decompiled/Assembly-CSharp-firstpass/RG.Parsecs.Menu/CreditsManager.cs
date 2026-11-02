using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Menu;

public class CreditsManager : MonoBehaviour
{
	public List<GameObject> creditsScreens = new List<GameObject>();

	public float timer = 4f;

	private float _time;

	public int currentScreen;

	public RectTransform SkipButton;

	private void OnEnable()
	{
		currentScreen = 0;
		_time = 0f;
		ShowScreen(currentScreen);
	}

	private void Update()
	{
		if (currentScreen + 1 != creditsScreens.Count)
		{
			_time += Time.deltaTime;
			if (_time >= timer)
			{
				_time = 0f;
				currentScreen++;
				ShowScreen(currentScreen);
			}
			float num = _time * 360f / timer;
			SkipButton.rotation = Quaternion.Euler(0f, 0f, num * -1f);
		}
	}

	public void Skip()
	{
		_time = timer;
	}

	public void ShowPrevious()
	{
		_time = 0f;
		if (currentScreen > 0)
		{
			currentScreen--;
		}
		ShowScreen(currentScreen);
	}

	public void ShowScreen(int id)
	{
		for (int i = 0; i < creditsScreens.Count; i++)
		{
			if (i == id)
			{
				creditsScreens[i].SetActive(value: true);
			}
			else
			{
				creditsScreens[i].SetActive(value: false);
			}
		}
	}
}
