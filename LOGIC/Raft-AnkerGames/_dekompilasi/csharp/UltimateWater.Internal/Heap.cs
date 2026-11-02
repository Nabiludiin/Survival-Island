using System;
using System.Collections;
using System.Collections.Generic;

namespace UltimateWater.Internal;

public sealed class Heap<T> : IEnumerable<T>, IEnumerable where T : IComparable<T>
{
	private T[] _Elements;

	private int _NumElements;

	public int Count => _NumElements;

	public T Max => _Elements[0];

	public Heap()
		: this(8)
	{
	}

	public Heap(int capacity)
	{
		_Elements = new T[capacity];
	}

	public T ExtractMax()
	{
		if (_NumElements == 0)
		{
			throw new InvalidOperationException("Heap is empty.");
		}
		T result = _Elements[0];
		_Elements[0] = _Elements[--_NumElements];
		_Elements[_NumElements] = default;
		BubbleDown(0);
		return result;
	}

	public void Insert(T element)
	{
		if (_Elements.Length == _NumElements)
		{
			Resize(_Elements.Length * 2);
		}
		_Elements[_NumElements++] = element;
		BubbleUp(_NumElements - 1, element);
	}

	public void Remove(T element)
	{
		for (int i = 0; i < _NumElements; i++)
		{
			if (_Elements[i].Equals(element))
			{
				_Elements[i] = _Elements[--_NumElements];
				_Elements[_NumElements] = default;
				BubbleDown(i);
				break;
			}
		}
	}

	public void Clear()
	{
		_NumElements = 0;
	}

	public T[] GetUnderlyingArray()
	{
		return _Elements;
	}

	public void Resize(int len)
	{
		Array.Resize(ref _Elements, len);
	}

	public IEnumerator<T> GetEnumerator()
	{
		if (_Elements.Length != _NumElements)
		{
			Array.Resize(ref _Elements, _NumElements);
		}
		return ((IEnumerable<T>)_Elements).GetEnumerator();
	}

	private void BubbleUp(int index, T element)
	{
		while (index != 0)
		{
			int num = index - 1 >> 1;
			T other = _Elements[num];
			if (element.CompareTo(other) <= 0)
			{
				break;
			}
			_Elements[index] = _Elements[num];
			_Elements[num] = element;
			index = num;
		}
	}

	private void BubbleDown(int index)
	{
		T val = _Elements[index];
		for (int num = (index << 1) + 1; num < _NumElements; num = (index << 1) + 1)
		{
			T val2 = _Elements[num];
			int num2;
			if (num + 1 < _NumElements)
			{
				T val3 = _Elements[num + 1];
				if (val2.CompareTo(val3) > 0)
				{
					num2 = num;
				}
				else
				{
					val2 = val3;
					num2 = num + 1;
				}
			}
			else
			{
				num2 = num;
			}
			if (val.CompareTo(val2) >= 0)
			{
				break;
			}
			_Elements[num2] = val;
			_Elements[index] = val2;
			index = num2;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		if (_Elements.Length != _NumElements)
		{
			Array.Resize(ref _Elements, _NumElements);
		}
		return _Elements.GetEnumerator();
	}
}
