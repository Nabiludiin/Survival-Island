using UnityEngine;

namespace Cinemachine.Utility;

internal class PositionPredictor
{
	private Vector3 m_Position;

	private const float kSmoothingDefault = 10f;

	private float mSmoothing = 10f;

	private GaussianWindow1D_Vector3 m_Velocity = new GaussianWindow1D_Vector3(10f);

	private GaussianWindow1D_Vector3 m_Accel = new GaussianWindow1D_Vector3(10f);

	public float Smoothing
	{
		get
		{
			return mSmoothing;
		}
		set
		{
			if (value != mSmoothing)
			{
				mSmoothing = value;
				int maxKernelRadius = Mathf.Max(10, Mathf.FloorToInt(value * 1.5f));
				m_Velocity = new GaussianWindow1D_Vector3(mSmoothing, maxKernelRadius);
				m_Accel = new GaussianWindow1D_Vector3(mSmoothing, maxKernelRadius);
			}
		}
	}

	public bool IsEmpty => m_Velocity.IsEmpty();

	public void Reset()
	{
		m_Velocity.Reset();
		m_Accel.Reset();
	}

	public void AddPosition(Vector3 pos)
	{
		if (IsEmpty)
		{
			m_Velocity.AddValue(Vector3.zero);
		}
		else
		{
			Vector3 vector = m_Velocity.Value();
			Vector3 vector2 = (pos - m_Position) / Time.deltaTime;
			m_Velocity.AddValue(vector2);
			m_Accel.AddValue(vector2 - vector);
		}
		m_Position = pos;
	}

	public Vector3 PredictPosition(float lookaheadTime)
	{
		int num = Mathf.Min(Mathf.RoundToInt(lookaheadTime / Time.deltaTime), 6);
		float num2 = lookaheadTime / (float)num;
		Vector3 position = m_Position;
		Vector3 vector = (m_Velocity.IsEmpty() ? Vector3.zero : m_Velocity.Value());
		Vector3 vector2 = (m_Accel.IsEmpty() ? Vector3.zero : m_Accel.Value());
		for (int i = 0; i < num; i++)
		{
			position += vector * num2;
			Vector3 vector3 = vector + vector2 * num2;
			vector2 = Quaternion.FromToRotation(vector, vector3) * vector2;
			vector = vector3;
		}
		return position;
	}
}
