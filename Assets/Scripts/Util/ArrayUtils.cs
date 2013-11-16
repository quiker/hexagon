using UnityEngine;
using System;

public class ArrayUtils
{
	public static void RandomSort<T>(T[] array)
	{
    	for (int i = 0; i < array.Length; i++) {
        	int ri = UnityEngine.Random.Range(i, array.Length);
			T temp = array[i];
			array[i] = array[ri];
			array[ri] = temp;
		}
    }
	
	public static int[] Slice(int[] array, int index)
	{
		int[] result = new int[array.Length - index];
		System.Array.Copy(array, index, result, 0, result.Length);
		return result;
	}
	
	public static int[] Slice(float[] array, int index)
	{
		int[] result = new int[array.Length - index];
		for (int i = 0; i < array.Length - index; i++) {
			result[i] = (int)array[index + i];
		}
		return result;
	}
	
	public static float[] SliceF(int[] array, int index)
	{
		float[] result = new float[array.Length - index];
		for (int i = 0; i < array.Length - index; i++) {
			result[i] = (float)array[index + i];
		}
		return result;
	}
	
	public static float[] SliceF(float[] array, int index)
	{
		float[] result = new float[array.Length - index];
		System.Array.Copy(array, index, result, 0, result.Length);
		return result;
	}
}
