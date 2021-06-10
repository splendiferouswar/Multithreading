using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace Multithreading
{
    class Program
    {
        public static int[] numberArray = new int[10000000];                                //creating an empty array of size 10 million
        public static Thread[] threadArray = new Thread[Environment.ProcessorCount];
        static void Main()
        {
            Random randNum = new Random();
            for (int i = 0; i < numberArray.Length; i++)
            {
                numberArray[i] = randNum.Next(0, 999999999);                                    //adding random numbers to the aray from 0 to one less than a billion
            }

            var stopwatch2 = Stopwatch.StartNew();                                              //starting the stopwatch
            Console.WriteLine(findMax(numberArray, 0, numberArray.Length));                     //finding the max number within the array
            Console.WriteLine("Single Thread: {0}ms\n\n", stopwatch2.ElapsedMilliseconds);      //stoping and writing the time

            // Create an array of Thread references.

            int start = 0;                                                                      //start and end variables used to divide arrays
            int end = numberArray.Length / threadArray.Length;

            ConcurrentBag<int> maxNumberList = new ConcurrentBag<int>();                        //storing the max numbers from the subdivided arrays

            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < threadArray.Length; i++)
            {
                int newStart = start;                                                           //locking variables 
                int newEnd = end;

                threadArray[i] = new Thread(() => maxNumberList.Add(findMax(numberArray, newStart, newEnd)));//using lambda function to pass parameters to the thread
                threadArray[i].Start();                                                         // Start the thread with a ThreadStart.
                //Console.WriteLine("Running thread number... " + i);

                start += numberArray.Length / threadArray.Length;                               //moving the "dividers"
                end += numberArray.Length / threadArray.Length;
            }
            // Join all the threads.
            for (int i = 0; i < threadArray.Length; i++) threadArray[i].Join();

            Console.WriteLine("\n" + findMax(maxNumberList.ToArray(), 0, maxNumberList.Count));
            Console.WriteLine("Multi Thread: {0}ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine("\nThe number of threads are: " + Environment.ProcessorCount);
        }
        public static int findMax(int[] arr, int start, int end)
        {
            int max = arr[start];
            for (int i = start; i < end; i++)
            {

                if (arr[i] > max)
                {
                    max = arr[i];
                    Console.WriteLine(" Found new Max Number (" + max + ") at thread number " + Environment.CurrentManagedThreadId);
                }
            }
            return max;
        }
    }
}
