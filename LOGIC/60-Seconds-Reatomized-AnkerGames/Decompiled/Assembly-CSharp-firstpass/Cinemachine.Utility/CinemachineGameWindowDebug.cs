using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine.Utility;

public class CinemachineGameWindowDebug
{
	private static HashSet<Object> mClients;

	public static void ReleaseScreenPos(Object client)
	{
		if (mClients != null && mClients.Contains(client))
		{
			mClients.Remove(client);
		}
	}

	public static Rect GetScreenPos(Object client, string text, GUIStyle style)
	{
		if (mClients == null)
		{
			mClients = new HashSet<Object>();
		}
		if (!mClients.Contains(client))
		{
			mClients.Add(client);
		}
		Vector2 position = new Vector2(0f, 0f);
		Vector2 size = style.CalcSize(new GUIContent(text));
		if (mClients != null)
		{
			using HashSet<Object>.Enumerator enumerator = mClients.GetEnumerator();
			while (enumerator.MoveNext() && !(enumerator.Current == client))
			{
				position.y += size.y;
			}
		}
		return new Rect(position, size);
	}
}
