using System;
using System.Diagnostics;

class Program1
{
    const int _max = 10000;
    static void Main()
    {
	int[] array = new int[1000000];
	Method1(array);


	var s1 = Stopwatch.StartNew();
	for (int i = 0; i < _max; i++)
	{
	    Method1(array);
	}
	s1.Stop();
    }


    static void Method1(int[] array)
    {
	// Initialize each element in for-loop.
	for (int i = 0; i < array.Length; i++)
	{
	    array[i] = i;
	}
    }
 }    


