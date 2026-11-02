using System;

namespace UnityEngine.AzureSky;

[Serializable]
public class AzureSkyTimeOfDayComponent
{
	public int selectableDayInt = 15;

	public int dayOfYear = 1;

	public string[] selectableDayList = new string[42]
	{
		"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
		"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
		"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
		"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
		"40", "41"
	};

	public float hour = 6f;

	public int day = 15;

	public int month = 9;

	public int year = 1903;

	public int dayInMonth = 30;

	public float utc;

	public float latitude;

	public float longitude;

	public int timeMode;

	public float dayCycle = 10f;

	public bool setTimeByCurve;

	public float hourByCurve = 6f;

	public AnimationCurve dayCycleCurve = AnimationCurve.Linear(0f, 0f, 24f, 24f);

	public float lst;

	private float m_radians;

	private float m_radLatitude;

	private float m_sinLatitude;

	private float m_cosLatitude;

	private Vector2 m_hourAndMinutes;

	private Vector3 m_date;

	private DateTime m_dateTime;

	private int m_daysInMonth = 30;

	private int m_dayOfWeek;

	private int m_fixLeapYear;

	public void UpdateCalendar(string[] dayList)
	{
		m_daysInMonth = DateTime.DaysInMonth(year, month);
		if (day > m_daysInMonth)
		{
			day = m_daysInMonth;
		}
		if (day < 1)
		{
			day = 1;
		}
		m_dateTime = new DateTime(year, month, 1);
		m_dayOfWeek = (int)m_dateTime.DayOfWeek;
		selectableDayInt = day - 1 + m_dayOfWeek;
		m_dateTime = new DateTime(year, month, day);
		for (int i = 0; i < dayList.Length; i++)
		{
			if (i < m_dayOfWeek || i >= m_dayOfWeek + m_daysInMonth)
			{
				dayList[i] = "";
				continue;
			}
			m_dateTime = new DateTime(year, month, i - m_dayOfWeek + 1);
			dayList[i] = m_dateTime.Day.ToString();
		}
	}

	public int GetDayOfWeek()
	{
		m_dateTime = new DateTime(year, month, day);
		return (int)m_dateTime.DayOfWeek;
	}

	public int GetDayOfWeek(int dayNumber)
	{
		m_dateTime = new DateTime(year, month, dayNumber);
		return (int)m_dateTime.DayOfWeek;
	}

	public int GetDayOfYear()
	{
		m_dateTime = new DateTime(year, month, day);
		dayOfYear = m_dateTime.DayOfYear;
		if (dayOfYear > 59 && !IsLeapYear())
		{
			m_fixLeapYear = 1;
		}
		else
		{
			m_fixLeapYear = 0;
		}
		return m_dateTime.DayOfYear - 1 + m_fixLeapYear;
	}

	public bool IsLeapYear()
	{
		return DateTime.IsLeapYear(year);
	}

	public Vector2 GetTime()
	{
		if (setTimeByCurve)
		{
			m_hourAndMinutes.x = Mathf.Floor(hourByCurve);
			m_hourAndMinutes.y = 60f * (hourByCurve - m_hourAndMinutes.x);
			m_hourAndMinutes.y = Mathf.Floor(m_hourAndMinutes.y);
		}
		else
		{
			m_hourAndMinutes.x = Mathf.Floor(hour);
			m_hourAndMinutes.y = 60f * (hour - m_hourAndMinutes.x);
			m_hourAndMinutes.y = Mathf.Floor(m_hourAndMinutes.y);
		}
		return m_hourAndMinutes;
	}

	public Vector3 GetDate()
	{
		m_date.x = month;
		m_date.y = day;
		m_date.z = year;
		return m_date;
	}

	public float GetSystemTime()
	{
		return (float)DateTime.Now.Hour + 1f / 60f * (float)DateTime.Now.Minute;
	}

	public void ApplySystemTime()
	{
		hour = (float)DateTime.Now.Hour + 1f / 60f * (float)DateTime.Now.Minute;
	}

	public void ApplySystemDate()
	{
		month = DateTime.Now.Month;
		day = DateTime.Now.Day;
		year = DateTime.Now.Year;
	}

	public void StartNextDay(int repeatMode)
	{
		if (repeatMode != 1)
		{
			day++;
		}
		hour = 0f;
		if (day <= m_daysInMonth)
		{
			return;
		}
		day = 1;
		if (repeatMode == 2)
		{
			return;
		}
		month++;
		if (month <= 12)
		{
			return;
		}
		month = 1;
		if (repeatMode != 3)
		{
			year++;
			if (year > 9999)
			{
				year = 1;
			}
		}
	}

	public void JumpToNextDay(bool keepTime = true)
	{
		day++;
		if (!keepTime)
		{
			hour = 0f;
		}
		if (day <= m_daysInMonth)
		{
			return;
		}
		day = 1;
		month++;
		if (month > 12)
		{
			month = 1;
			year++;
			if (year > 9999)
			{
				year = 1;
			}
		}
	}

	public void GotoDate(int month, int day, int year)
	{
		this.month = month;
		this.day = day;
		this.year = year;
	}

	public void GotoTime(int hours, int minutes)
	{
		hour = (float)hours + 1f / 60f * (float)minutes;
	}

	public void GotoTime(float hourInTimeline)
	{
		hour = hourInTimeline;
	}

	public float GetDayLength()
	{
		if (dayCycle > 0f)
		{
			return 0.4f / dayCycle;
		}
		return 0f;
	}

	public void CalculateTimeByCurve()
	{
		hourByCurve = dayCycleCurve.Evaluate(hour);
	}

	public float SetSimpleSunPosition()
	{
		if (setTimeByCurve)
		{
			return (hourByCurve + utc) * 360f / 24f - 90f;
		}
		return (hour + utc) * 360f / 24f - 90f;
	}

	public Quaternion SetSimpleMoonPosition(Transform sun)
	{
		return sun.transform.rotation * Quaternion.Euler(0f, -180f, 0f);
	}

	public Vector3 SetRealisticSunPosition()
	{
		m_radians = MathF.PI / 180f;
		m_radLatitude = m_radians * latitude;
		m_sinLatitude = Mathf.Sin(m_radLatitude);
		m_cosLatitude = Mathf.Cos(m_radLatitude);
		float num = hour - utc;
		float num2 = 367 * year - 7 * (year + (month + 9) / 12) / 4 + 275 * month / 9 + day - 730530;
		num2 += num / 24f;
		float num3 = 23.4393f - 3.563E-07f * num2;
		float f = m_radians * num3;
		float num4 = Mathf.Sin(f);
		float num5 = Mathf.Cos(f);
		float num6 = 282.9404f + 4.70935E-05f * num2;
		float num7 = 0.016709f - 1.151E-09f * num2;
		float num8 = 356.047f + 0.98560023f * num2;
		float num9 = m_radians * num8;
		float num10 = Mathf.Sin(num9);
		float num11 = Mathf.Cos(num9);
		float f2 = num9 + num7 * num10 * (1f + num7 * num11);
		float num12 = Mathf.Sin(f2);
		float num13 = Mathf.Cos(f2) - num7;
		float num14 = Mathf.Sqrt(1f - num7 * num7) * num12;
		float num15 = 57.29578f * Mathf.Atan2(num14, num13);
		float num16 = Mathf.Sqrt(num13 * num13 + num14 * num14);
		float f3 = m_radians * (num15 + num6);
		float num17 = Mathf.Sin(f3);
		float num18 = Mathf.Cos(f3);
		float num19 = num16 * num18;
		float num20 = num16 * num17;
		float num21 = num19;
		float num22 = num20 * num5;
		float y = num20 * num4;
		float num23 = Mathf.Atan2(num22, num21);
		float f4 = Mathf.Atan2(y, Mathf.Sqrt(num21 * num21 + num22 * num22));
		float num24 = Mathf.Sin(f4);
		float num25 = Mathf.Cos(f4);
		float num26 = num15 + num6 + 180f;
		float num27 = 15f * num;
		float num28 = num26 + num27;
		float f5 = (lst = m_radians * (num28 + longitude)) - num23;
		float num29 = Mathf.Sin(f5);
		float num30 = Mathf.Cos(f5) * num25;
		float num31 = num29 * num25;
		float num32 = num24;
		float x = num30 * m_sinLatitude - num32 * m_cosLatitude;
		float y2 = num31;
		float f6 = num30 * m_cosLatitude + num32 * m_sinLatitude;
		float f7 = Mathf.Atan2(y2, x) + m_radians * 180f;
		float num33 = Mathf.Asin(f6);
		float f8 = 90f * m_radians - num33;
		Vector3 vector = default;
		vector.z = Mathf.Sin(f8) * Mathf.Cos(f7);
		vector.x = Mathf.Sin(f8) * Mathf.Sin(f7);
		vector.y = Mathf.Cos(f8);
		return vector * -1f;
	}

	public Vector3 SetRealisticMoonPosition()
	{
		float num = hour - utc;
		float num2 = 367 * year - 7 * (year + (month + 9) / 12) / 4 + 275 * month / 9 + day - 730530;
		num2 += num / 24f;
		float num3 = 23.4393f - 3.563E-07f * num2;
		float f = m_radians * num3;
		float num4 = Mathf.Sin(f);
		float num5 = Mathf.Cos(f);
		float num6 = 125.1228f - 0.05295381f * num2;
		float num7 = 5.1454f;
		float num8 = 318.0634f + 0.16435732f * num2;
		float num9 = 0.0549f;
		float num10 = 115.3654f + 13.064993f * num2;
		float num11 = m_radians * num10;
		float f2 = num11 + num9 * Mathf.Sin(num11) * (1f + num9 * Mathf.Cos(num11));
		float num12 = 60.2666f * (Mathf.Cos(f2) - num9);
		float num13 = 60.2666f * (Mathf.Sqrt(1f - num9 * num9) * Mathf.Sin(f2));
		float num14 = 57.29578f * Mathf.Atan2(num13, num12);
		float num15 = Mathf.Sqrt(num12 * num12 + num13 * num13);
		float f3 = m_radians * (num14 + num8);
		float num16 = Mathf.Sin(f3);
		float num17 = Mathf.Cos(f3);
		float f4 = m_radians * num6;
		float f5 = m_radians * num7;
		float num18 = num15 * (Mathf.Cos(f4) * num17 - Mathf.Sin(f4) * num16 * Mathf.Cos(f5));
		float num19 = num15 * (Mathf.Sin(f4) * num17 + Mathf.Cos(f4) * num16 * Mathf.Cos(f5));
		float num20 = num15 * (num16 * Mathf.Sin(f5));
		float num21 = num18;
		float num22 = num19 * num5 - num20 * num4;
		float y = num19 * num4 + num20 * num5;
		float num23 = Mathf.Atan2(num22, num21);
		float f6 = Mathf.Atan2(y, Mathf.Sqrt(num21 * num21 + num22 * num22));
		float f7 = lst - num23;
		float num24 = Mathf.Cos(f7) * Mathf.Cos(f6);
		float num25 = Mathf.Sin(f7) * Mathf.Cos(f6);
		float num26 = Mathf.Sin(f6);
		float x = num24 * m_sinLatitude - num26 * m_cosLatitude;
		float y2 = num25;
		float f8 = num24 * m_cosLatitude + num26 * m_sinLatitude;
		float f9 = Mathf.Atan2(y2, x) + m_radians * 180f;
		float num27 = Mathf.Asin(f8);
		float f10 = 90f * m_radians - num27;
		Vector3 vector = default;
		vector.z = Mathf.Sin(f10) * Mathf.Cos(f9);
		vector.x = Mathf.Sin(f10) * Mathf.Sin(f9);
		vector.y = Mathf.Cos(f10);
		return vector * -1f;
	}
}
