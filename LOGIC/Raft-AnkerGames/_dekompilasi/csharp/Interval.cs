using System;

[Serializable]
public abstract class Interval<T>
{
	public T minValue;

	public T maxValue;

	public abstract T GetRandomValue();

	public abstract T GetValueFromNormalized(float normalizedValue);

	public abstract bool IsInsideOfIntervalInclusive(T value);

	public abstract bool IsInsideOfIntervalExclusive(T value);
}
