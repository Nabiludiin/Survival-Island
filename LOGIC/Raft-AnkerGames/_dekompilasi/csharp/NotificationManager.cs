using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
	[SerializeField]
	private List<Notification> notifications = new List<Notification>();

	[SerializeField]
	private Queue<Notification> notificationQue = new Queue<Notification>();

	private Notification currentNotification;

	private void Awake()
	{
		ComponentManager<NotificationManager>.Value = this;
	}

	private void Update()
	{
		if (notificationQue.Count != 0)
		{
			if (currentNotification == null)
			{
				currentNotification = notificationQue.Dequeue();
				currentNotification.Show();
			}
			if (!currentNotification.active)
			{
				currentNotification = null;
			}
		}
	}

	public Notification ShowNotification(string identifier)
	{
		Notification notification = notifications.Find((Notification n) => n.identifier == identifier);
		if (notification == null)
		{
			Debug.LogWarning("Can't find notification with identifier " + identifier);
			return null;
		}
		notificationQue.Enqueue(notification);
		return notification;
	}
}
