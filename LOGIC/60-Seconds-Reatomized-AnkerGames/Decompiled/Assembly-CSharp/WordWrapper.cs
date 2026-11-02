using System;
using System.Linq;
using System.Text;
using UnityEngine;

public class WordWrapper
{
	public enum EWrapAlgorithm
	{
		Greedy,
		Dynamic
	}

	private const int INFINITY = int.MaxValue;

	private static WordWrapper Instance;

	private bool _recalculateLatinCharacters;

	private string _wordSeparator = " ";

	private char _defaultDelimiter = ' ';

	private string _defaultDelimiterStr = " ";

	private int[] _wordsLineCounterMatrix;

	private int[] _costMatrix;

	private int[] _costArrangement;

	private int[] _result;

	public bool RecalculateLatinCharacters
	{
		get
		{
			return _recalculateLatinCharacters;
		}
		set
		{
			_recalculateLatinCharacters = value;
		}
	}

	public string WordSeparator
	{
		get
		{
			return _wordSeparator;
		}
		set
		{
			_wordSeparator = value;
		}
	}

	public char DefaultDelimiter
	{
		get
		{
			return _defaultDelimiter;
		}
		set
		{
			_defaultDelimiter = value;
			_defaultDelimiterStr = _defaultDelimiter.ToString();
		}
	}

	public static WordWrapper GetInstance()
	{
		if (Instance == null)
		{
			Instance = new WordWrapper();
		}
		return Instance;
	}

	public StringBuilder ClearDelimiters(StringBuilder sb)
	{
		return sb.Replace(_defaultDelimiterStr, "");
	}

	public string ClearDelimiters(string s)
	{
		return s.Replace(_defaultDelimiterStr, "");
	}

	public string WrapText(string text, int lineWidth, EWrapAlgorithm algorithm = EWrapAlgorithm.Dynamic)
	{
		return WrapText(text, lineWidth, _defaultDelimiter, algorithm);
	}

	public string WrapText(string text, int lineWidth, char delimiter = ' ', EWrapAlgorithm algorithm = EWrapAlgorithm.Dynamic)
	{
		if (!text.Contains(delimiter.ToString()))
		{
			return text;
		}
		string[] array = text.Split(delimiter);
		if (text.Length - array.Length - 1 < lineWidth)
		{
			return text.Replace(delimiter.ToString(), "");
		}
		return algorithm switch
		{
			EWrapAlgorithm.Greedy => GreedyAlgorithm(array, lineWidth), 
			EWrapAlgorithm.Dynamic => DynamicAlgorithm(array, lineWidth), 
			_ => string.Empty, 
		};
	}

	private string GreedyAlgorithm(string[] words, int lineWidth)
	{
		int num = lineWidth;
		string text = string.Empty;
		foreach (string text2 in words)
		{
			if (text2.Length + _wordSeparator.Length > num)
			{
				text += Environment.NewLine;
				num = lineWidth;
				text = text + text2 + _wordSeparator;
				num -= text2.Length + _wordSeparator.Length;
			}
			else
			{
				text = text + text2 + _wordSeparator;
				num -= text2.Length + _wordSeparator.Length;
			}
		}
		return text;
	}

	private int calculateIndex(int row, int column, int matrixSize)
	{
		return matrixSize * row + column - row * (row + 1) / 2;
	}

	private int calculateMatrixSize(int size)
	{
		return size * (size + 1) / 2;
	}

	private void CheckMatrices(int wordsNumber)
	{
		if (_wordsLineCounterMatrix == null || _wordsLineCounterMatrix.Length < calculateMatrixSize(wordsNumber))
		{
			_wordsLineCounterMatrix = new int[calculateMatrixSize(wordsNumber)];
		}
		if (_costMatrix == null || _costMatrix.Length < calculateMatrixSize(wordsNumber))
		{
			_costMatrix = new int[calculateMatrixSize(wordsNumber)];
		}
		if (_costArrangement == null || _costArrangement.Length < wordsNumber + 1)
		{
			_costArrangement = new int[wordsNumber + 1];
		}
		if (_result == null || _result.Length < wordsNumber + 1)
		{
			_result = new int[wordsNumber + 1];
		}
	}

	private void CleanTables()
	{
		Array.Clear(_wordsLineCounterMatrix, 0, _wordsLineCounterMatrix.Length);
		Array.Clear(_costMatrix, 0, _costMatrix.Length);
		Array.Clear(_costArrangement, 0, _costArrangement.Length);
		Array.Clear(_result, 0, _result.Length);
	}

	private string DynamicAlgorithm(string[] words, int lineWidth)
	{
		int[] array = new int[words.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = words[i].Length;
			if (_recalculateLatinCharacters)
			{
				int num = words[i].ToLower().Count((char c) => (c >= 'a' && c <= 'z') || c == ' ');
				int num2 = Mathf.CeilToInt((float)num / 3f);
				array[i] = array[i] - num + num2;
			}
			lineWidth = Math.Max(lineWidth, words[i].Length);
		}
		int num3 = array.Length;
		CheckMatrices(num3);
		for (int num4 = 0; num4 < num3; num4++)
		{
			_wordsLineCounterMatrix[calculateIndex(num4, num4, num3)] = lineWidth - array[num4];
			for (int num5 = num4 + 1; num5 < num3; num5++)
			{
				_wordsLineCounterMatrix[calculateIndex(num4, num5, num3)] = _wordsLineCounterMatrix[calculateIndex(num4, num5 - 1, num3)] - array[num5] - _wordSeparator.Length;
			}
		}
		for (int num6 = 0; num6 < num3; num6++)
		{
			for (int num7 = num6; num7 < num3; num7++)
			{
				if (_wordsLineCounterMatrix[calculateIndex(num6, num7, num3)] < 0)
				{
					_costMatrix[calculateIndex(num6, num7, num3)] = int.MaxValue;
				}
				else if (num7 == num3 - 1 && _wordsLineCounterMatrix[calculateIndex(num6, num7, num3)] >= 0)
				{
					_costMatrix[calculateIndex(num6, num7, num3)] = 0;
				}
				else
				{
					_costMatrix[calculateIndex(num6, num7, num3)] = _wordsLineCounterMatrix[calculateIndex(num6, num7, num3)] * _wordsLineCounterMatrix[calculateIndex(num6, num7, num3)];
				}
			}
		}
		_costArrangement[0] = 0;
		for (int num8 = 1; num8 <= num3; num8++)
		{
			_costArrangement[num8] = int.MaxValue;
			for (int num9 = 1; num9 <= num8; num9++)
			{
				if (_costArrangement[num9 - 1] != int.MaxValue && _costMatrix[calculateIndex(num9 - 1, num8 - 1, num3)] != int.MaxValue && _costArrangement[num9 - 1] + _costMatrix[calculateIndex(num9 - 1, num8 - 1, num3)] < _costArrangement[num8])
				{
					_costArrangement[num8] = _costArrangement[num9 - 1] + _costMatrix[calculateIndex(num9 - 1, num8 - 1, num3)];
					_result[num8] = num9;
				}
			}
		}
		string message = string.Empty;
		GetWrappedText(num3, words, ref message);
		CleanTables();
		return message.TrimEnd();
	}

	private int GetWrappedText(int n, string[] words, ref string message)
	{
		int result = ((_result[n] == 1) ? 1 : (GetWrappedText(_result[n] - 1, words, ref message) + 1));
		for (int i = _result[n] - 1; i < n; i++)
		{
			message = message + words[i] + _wordSeparator;
		}
		message += Environment.NewLine;
		return result;
	}
}
