using System;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct dfTouchInfo
{
	private int m_FingerId;

	private Vector2 m_Position;

	private Vector2 m_RawPosition;

	private Vector2 m_PositionDelta;

	private float m_TimeDelta;

	private int m_TapCount;

	private TouchPhase m_Phase;

	public int fingerId => m_FingerId;

	public Vector2 position => m_Position;

	public Vector2 rawPosition => m_RawPosition;

	public Vector2 deltaPosition => m_PositionDelta;

	public float deltaTime => m_TimeDelta;

	public int tapCount => m_TapCount;

	public TouchPhase phase => m_Phase;

	public dfTouchInfo(int fingerID, TouchPhase phase, int tapCount, Vector2 position, Vector2 positionDelta, float timeDelta)
	{
		m_FingerId = fingerID;
		m_Phase = phase;
		m_Position = position;
		m_PositionDelta = positionDelta;
		m_TapCount = tapCount;
		m_TimeDelta = timeDelta;
		m_RawPosition = position;
	}

	public static implicit operator dfTouchInfo(Touch touch)
	{
		return new dfTouchInfo
		{
			m_PositionDelta = touch.deltaPosition,
			m_TimeDelta = touch.deltaTime,
			m_FingerId = touch.fingerId,
			m_Phase = touch.phase,
			m_Position = touch.position,
			m_TapCount = touch.tapCount
		};
	}
}
