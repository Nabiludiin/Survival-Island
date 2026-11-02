using System;
using System.IO;
using System.IO.Compression;
using UnityEngine;

public class CompressionUtil
{
	public static byte[] Compress(byte[] data, System.IO.Compression.CompressionLevel level = System.IO.Compression.CompressionLevel.Optimal)
	{
		using MemoryStream memoryStream = new MemoryStream();
		using (DeflateStream deflateStream = new DeflateStream(memoryStream, level, leaveOpen: true))
		{
			deflateStream.Write(data, 0, data.Length);
		}
		return memoryStream.ToArray();
	}

	public static byte[] Decompress(byte[] compressedData, System.IO.Compression.CompressionLevel level = System.IO.Compression.CompressionLevel.Optimal)
	{
		using MemoryStream memoryStream = new MemoryStream(compressedData);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		using DeflateStream deflateStream = new DeflateStream(memoryStream, level);
		using MemoryStream memoryStream2 = new MemoryStream();
		try
		{
			deflateStream.CopyTo(memoryStream2);
			deflateStream.Flush();
			memoryStream2.Flush();
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
		return memoryStream2.ToArray();
	}
}
