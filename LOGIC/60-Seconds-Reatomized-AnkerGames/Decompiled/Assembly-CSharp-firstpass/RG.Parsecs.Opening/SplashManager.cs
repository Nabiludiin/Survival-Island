using FMODUnity;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Opening;

public class SplashManager : MonoBehaviour
{
	public static SplashManager Instance;

	[SerializeField]
	[EventRef]
	private string _backgroundMusic;

	[SerializeField]
	private GameObject _splashScreen;

	[SerializeField]
	private GameObject _loadingScreen;

	[SerializeField]
	private bool _useTimeout;

	[SerializeField]
	private float _timeoutInSeconds = 1f;

	[SerializeField]
	private bool _allowClickAway = true;

	[SerializeField]
	private bool _useSplashScreenTimeOut;

	[SerializeField]
	private float _splashScreenTimeout = 10f;

	private float _startTime;

	public bool SplashVisible { get; set; }

	private void Awake()
	{
		Instance = this;
		SplashVisible = true;
		_startTime = Time.time;
		if (!string.IsNullOrEmpty(_backgroundMusic))
		{
			RuntimeManager.PlayOneShot(_backgroundMusic);
		}
		if (_useSplashScreenTimeOut)
		{
			Invoke("FinishSplash", _splashScreenTimeout);
		}
	}

	public void FinishSplash()
	{
		SplashVisible = false;
	}

	private void Update()
	{
		if (_allowClickAway && (Input.anyKeyDown || (_useTimeout && Time.time - _startTime > _timeoutInSeconds)))
		{
			if (!Singleton<GameManager>.Instance.LoadingManager.LevelsLoaded)
			{
				_splashScreen.SetActive(value: false);
				_loadingScreen.SetActive(value: true);
			}
			SplashVisible = false;
		}
	}
}
