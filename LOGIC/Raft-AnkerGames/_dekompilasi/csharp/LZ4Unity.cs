using System;
using System.Runtime.InteropServices;
using Unity.Collections;

public class LZ4Unity
{
	public class API
	{
		private const string DLLName = "lz4unity";

		[DllImport("lz4unity", CallingConvention = CallingConvention.Cdecl)]
		public static extern int LZ4Unity_Compress(IntPtr src, IntPtr dst, int srcSize, int dstCapacity);

		[DllImport("lz4unity", CallingConvention = CallingConvention.Cdecl)]
		public static extern int LZ4Unity_Decompress(IntPtr src, IntPtr dst, int compressedSize, int dstCapacity);

		[DllImport("lz4unity", CallingConvention = CallingConvention.Cdecl)]
		public static extern int LZ4Unity_CompressBound(int inputSize);
	}

	public static NativeArray<byte> CompressedBuffer = new NativeArray<byte>(10485760, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

	public static NativeArray<byte> DecompressedBuffer = new NativeArray<byte>(10485760, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

	public static int Compress(IntPtr src, IntPtr dst, int srcSize, int dstCapacity)
	{
		return API.LZ4Unity_Compress(src, dst, srcSize, dstCapacity);
	}

	public static int Decompress(IntPtr src, IntPtr dst, int compressedSize, int dstCapacity)
	{
		return API.LZ4Unity_Decompress(src, dst, compressedSize, dstCapacity);
	}

	public static int Compress(byte[] src, ref byte[] dst)
	{
		if (src != null && src.Length != 0 && dst != null && dst.Length != 0)
		{
			GCHandle gCHandle = GCHandle.Alloc(src, GCHandleType.Pinned);
			GCHandle gCHandle2 = GCHandle.Alloc(dst, GCHandleType.Pinned);
			int result = Compress(gCHandle.AddrOfPinnedObject(), gCHandle2.AddrOfPinnedObject(), src.Length, dst.Length);
			gCHandle.Free();
			gCHandle2.Free();
			return result;
		}
		return 0;
	}

	public static int Decompress(byte[] src, ref byte[] dst)
	{
		if (src != null && src.Length != 0 && dst != null && dst.Length != 0)
		{
			GCHandle gCHandle = GCHandle.Alloc(src, GCHandleType.Pinned);
			GCHandle gCHandle2 = GCHandle.Alloc(dst, GCHandleType.Pinned);
			int result = Decompress(gCHandle.AddrOfPinnedObject(), gCHandle2.AddrOfPinnedObject(), src.Length, dst.Length);
			gCHandle.Free();
			gCHandle2.Free();
			return result;
		}
		return 0;
	}

	public static int Decompress(byte[] src, IntPtr dst, int dstCapacity)
	{
		if (src != null && src.Length != 0)
		{
			GCHandle gCHandle = GCHandle.Alloc(src, GCHandleType.Pinned);
			int result = Decompress(gCHandle.AddrOfPinnedObject(), dst, src.Length, dstCapacity);
			gCHandle.Free();
			return result;
		}
		return 0;
	}

	public static int CompressBound(int inputSize)
	{
		return API.LZ4Unity_CompressBound(inputSize);
	}
}
