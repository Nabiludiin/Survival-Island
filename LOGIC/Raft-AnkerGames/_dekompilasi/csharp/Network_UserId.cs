using System;
using UnityEngine;

[Serializable]
public struct Network_UserId
{
	[SerializeField]
	private ulong m_id;

	public ulong Id => m_id;

	public Network_UserId(ulong id)
	{
		m_id = id;
	}

	public Network_UserId(string id)
	{
		m_id = Convert.ToUInt64(id, 16);
	}

	public bool IsValid()
	{
		return m_id != 0;
	}

	public void Clear()
	{
		m_id = 0uL;
	}

	public static implicit operator Network_UserId(string id)
	{
		return new Network_UserId(id);
	}

	public static bool operator ==(Network_UserId x, Network_UserId y)
	{
		return x.m_id == y.m_id;
	}

	public override int GetHashCode()
	{
		return m_id.GetHashCode();
	}

	public static implicit operator ulong(Network_UserId that)
	{
		return that.m_id;
	}

	public override bool Equals(object other)
	{
		if (other is Network_UserId)
		{
			return this == (Network_UserId)other;
		}
		return false;
	}

	public static bool operator !=(Network_UserId x, Network_UserId y)
	{
		return !(x == y);
	}

	public static implicit operator Network_UserId(ulong id)
	{
		return new Network_UserId(id);
	}

	public static implicit operator string(Network_UserId that)
	{
		return that.Id.ToString("X");
	}
}
