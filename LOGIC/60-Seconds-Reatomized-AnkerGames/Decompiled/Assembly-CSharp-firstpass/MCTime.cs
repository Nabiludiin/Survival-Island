using UnityEngine;

public static class MCTime
{
	private static float _pauseTimeScale = 1f;

	private static float _introductionTimeScale = 1f;

	private static float _bulletTimeScale = 1f;

	public static float PauseTimeScale
	{
		get
		{
			return _pauseTimeScale;
		}
		set
		{
			_pauseTimeScale = value;
			Time.timeScale = _pauseTimeScale * _introductionTimeScale * _bulletTimeScale;
		}
	}

	public static float IntroductionTimeScale
	{
		get
		{
			return _introductionTimeScale;
		}
		set
		{
			_introductionTimeScale = value;
			Time.timeScale = _pauseTimeScale * _introductionTimeScale * _bulletTimeScale;
		}
	}

	public static float BulletTimeScale
	{
		get
		{
			return _bulletTimeScale;
		}
		set
		{
			_bulletTimeScale = value;
			Time.timeScale = _pauseTimeScale * _introductionTimeScale * _bulletTimeScale;
		}
	}

	public static float timeScale => Time.timeScale;

	public static void ResetTimeScale()
	{
		_pauseTimeScale = 1f;
		_introductionTimeScale = 1f;
		_bulletTimeScale = 1f;
		Time.timeScale = 1f;
	}
}
