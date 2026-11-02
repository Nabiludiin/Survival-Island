using System;

namespace RG.Parsecs.Survival;

internal class DayStartedListener : IComparable<DayStartedListener>
{
	public int Order;

	public EndOfDayListenerList.OnDayStarted Listener;

	public int CompareTo(DayStartedListener other)
	{
		return Order.CompareTo(other.Order);
	}
}
