using System;
using Unity.Netcode;

[Serializable]
public class Message_WeatherManager_SetWeather : Message_NetworkBehaviour
{
	public int weatherIndex;

	public bool instant;

	protected bool Equals(Message_WeatherManager_SetWeather other)
	{
		if (Equals((Message_NetworkBehaviour)other) && weatherIndex == other.weatherIndex)
		{
			return instant == other.instant;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((Message_WeatherManager_SetWeather)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), weatherIndex, instant);
	}

	public static bool operator ==(Message_WeatherManager_SetWeather left, Message_WeatherManager_SetWeather right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_WeatherManager_SetWeather left, Message_WeatherManager_SetWeather right)
	{
		return !object.Equals(left, right);
	}

	public Message_WeatherManager_SetWeather()
	{
	}

	public Message_WeatherManager_SetWeather(Messages type, MonoBehaviour_Network behaviour, Weather weather, bool instant)
		: base(type, behaviour)
	{
		weatherIndex = (int)weather.so_weather.uniqueWeatherType;
		this.instant = instant;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in weatherIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in instant, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out weatherIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out instant, default(FastBufferWriter.ForPrimitives));
	}
}
