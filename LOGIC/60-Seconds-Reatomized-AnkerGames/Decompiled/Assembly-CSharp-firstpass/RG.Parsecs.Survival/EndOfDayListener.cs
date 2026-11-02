using System;

namespace RG.Parsecs.Survival;

public class EndOfDayListener : IComparable<EndOfDayListener>
{
	public int Order;

	public EndOfDayListenerList.OnEndOfDay Listener;

	public string SystemName;

	public int CompareTo(EndOfDayListener other)
	{
		return Order.CompareTo(other.Order);
	}
}
