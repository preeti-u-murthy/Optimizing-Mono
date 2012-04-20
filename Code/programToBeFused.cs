using System;
using System.Diagnostics;

class Program1
{
    const int _max = 10000000;
    static void Main()
    {
	int[] array = new int[12];
	int[] array1 = new int[12];

	var s1 = Stopwatch.StartNew();
	for (int i = 0; i < _max; i++)
	{
	    Method1(array,array1);
	}
	s1.Stop();
    }


    static void Method1(int[] array, int[] array1)
    {
	// Initialize each element in for-loop.
	for (int i = 0; i < 12; i++)
	{
	    array[i] = i;
	}
	for (int i = 0; i < 12; i++)
	{
	    array1[i] = i;
	}
    }


}

