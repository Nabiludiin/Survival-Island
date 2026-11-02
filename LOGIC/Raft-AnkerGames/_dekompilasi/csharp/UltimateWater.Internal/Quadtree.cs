using System;
using UnityEngine;

namespace UltimateWater.Internal;

public class Quadtree<T> where T : class, IPoint2D
{
	protected Rect _Rect;

	protected Rect _MarginRect;

	protected Vector2 _Center;

	protected Quadtree<T> _A;

	protected Quadtree<T> _B;

	protected Quadtree<T> _C;

	protected Quadtree<T> _D;

	protected Quadtree<T> _Root;

	protected T[] _Elements;

	protected int _NumElements;

	private int _FreeSpace;

	private int _LastIndex;

	private int _Depth;

	public Rect Rect
	{
		get
		{
			return _Rect;
		}
		set
		{
			_Rect = value;
			_Center = _Rect.center;
			_MarginRect = _Rect;
			float num = _Rect.width * 0.0025f;
			_MarginRect.xMin -= num;
			_MarginRect.yMin -= num;
			_MarginRect.xMax += num;
			_MarginRect.yMax += num;
			if (_A != null)
			{
				float width = _Rect.width * 0.5f;
				float height = _Rect.height * 0.5f;
				_A.Rect = new Rect(_Rect.xMin, _Center.y, width, height);
				_B.Rect = new Rect(_Center.x, _Center.y, width, height);
				_C.Rect = new Rect(_Rect.xMin, _Rect.yMin, width, height);
				_D.Rect = new Rect(_Center.x, _Rect.yMin, width, height);
			}
		}
	}

	public int Count
	{
		get
		{
			if (_A == null)
			{
				return _NumElements;
			}
			return _A.Count + _B.Count + _C.Count + _D.Count;
		}
	}

	public int FreeSpace => _Root._FreeSpace;

	public Quadtree(Rect rect, int maxElementsPerNode, int maxTotalElements)
	{
		_Root = this;
		Rect = rect;
		_Elements = new T[maxElementsPerNode];
		_NumElements = 0;
		_FreeSpace = maxTotalElements;
	}

	public bool AddElement(T element)
	{
		Vector2 position = element.Position;
		if (_Root._FreeSpace <= 0 || float.IsNaN(position.x) || float.IsNaN(position.y) || float.IsInfinity(position.x) || float.IsInfinity(position.y))
		{
			element.Destroy();
			return false;
		}
		if (_Root == this && !_Rect.Contains(element.Position))
		{
			ExpandToContain(element.Position);
		}
		return AddElementInternal(element);
	}

	public void UpdateElements(Quadtree<T> root)
	{
		if (_A != null)
		{
			_A.UpdateElements(root);
			_B.UpdateElements(root);
			_C.UpdateElements(root);
			_D.UpdateElements(root);
		}
		if (_Elements == null)
		{
			return;
		}
		for (int i = 0; i < _Elements.Length; i++)
		{
			T val = _Elements[i];
			if (val != null && !_MarginRect.Contains(val.Position))
			{
				RemoveElementAt(i);
				root.AddElement(val);
			}
		}
	}

	public void ExpandToContain(Vector2 point)
	{
		Rect rect = _Rect;
		do
		{
			float num = rect.width * 0.5f;
			rect.xMin -= num;
			rect.yMin -= num;
			rect.xMax += num;
			rect.yMax += num;
		}
		while (!rect.Contains(point));
		Rect = rect;
		UpdateElements(_Root);
	}

	public virtual void Destroy()
	{
		_Elements = null;
		if (_A != null)
		{
			_A.Destroy();
			_B.Destroy();
			_C.Destroy();
			_D.Destroy();
			_A = (_B = (_C = (_D = null)));
		}
	}

	private Quadtree(Quadtree<T> root, Rect rect, int maxElementsPerNode)
		: this(rect, maxElementsPerNode, 0)
	{
		_Root = root;
	}

	private bool AddElementInternal(T element)
	{
		if (element == null)
		{
			throw new ArgumentException("Element null");
		}
		if (_A != null)
		{
			Vector2 position = element.Position;
			if (position.x < _Center.x)
			{
				if (position.y > _Center.y)
				{
					return _A.AddElementInternal(element);
				}
				return _C.AddElementInternal(element);
			}
			if (position.y > _Center.y)
			{
				return _B.AddElementInternal(element);
			}
			return _D.AddElementInternal(element);
		}
		if (_NumElements != _Elements.Length)
		{
			while (_LastIndex < _Elements.Length)
			{
				if (_Elements[_LastIndex] == null)
				{
					AddElementAt(element, _LastIndex);
					return true;
				}
				_LastIndex++;
			}
			for (_LastIndex = 0; _LastIndex < _Elements.Length; _LastIndex++)
			{
				if (_Elements[_LastIndex] == null)
				{
					AddElementAt(element, _LastIndex);
					return true;
				}
			}
			throw new InvalidOperationException("UltimateWater: Code supposed to be unreachable.");
		}
		if (_Depth < 80)
		{
			T[] elements = _Elements;
			SpawnChildNodes();
			_A._Depth = (_B._Depth = (_C._Depth = (_D._Depth = _Depth + 1)));
			_Elements = null;
			_NumElements = 0;
			_Root._FreeSpace += elements.Length;
			for (int i = 0; i < elements.Length; i++)
			{
				AddElementInternal(elements[i]);
			}
			return AddElementInternal(element);
		}
		throw new Exception("UltimateWater: Quadtree depth limit reached.");
	}

	protected virtual void AddElementAt(T element, int index)
	{
		_NumElements++;
		_Root._FreeSpace--;
		_Elements[index] = element;
	}

	protected virtual void RemoveElementAt(int index)
	{
		_NumElements--;
		_Root._FreeSpace++;
		_Elements[index] = null;
	}

	protected virtual void SpawnChildNodes()
	{
		float width = _Rect.width * 0.5f;
		float height = _Rect.height * 0.5f;
		_A = new Quadtree<T>(_Root, new Rect(_Rect.xMin, _Center.y, width, height), _Elements.Length);
		_B = new Quadtree<T>(_Root, new Rect(_Center.x, _Center.y, width, height), _Elements.Length);
		_C = new Quadtree<T>(_Root, new Rect(_Rect.xMin, _Rect.yMin, width, height), _Elements.Length);
		_D = new Quadtree<T>(_Root, new Rect(_Center.x, _Rect.yMin, width, height), _Elements.Length);
	}
}
