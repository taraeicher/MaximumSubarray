using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace maximumSubarray
{
    class Program
    {
        static Random arrayVal = new Random();     //random number generator
        const int n = 10000;              
        static int[] theArray = new int[n];    //array of n values

        //Call each algorithm for solving the maximum subarray problem. Report the results and time taken to solve.
        static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();  //timer for measuring running time of each algorithm.
            int min = 0, max = 0;            //max and min elements in array
            int sum = int.MinValue;                       //sum between max and min elements in array

            //Initialize the array values. Print the values for verification.
            Console.WriteLine("The array is:" + Environment.NewLine);
            for (int i = 0; i < theArray.Length; i++)
            {
                theArray[i] = arrayVal.Next(-100,100);
                Console.Write(theArray[i].ToString() + " ");
            }
            Console.WriteLine(Environment.NewLine);

            //Execute the brute force algorithm and measure the running time.
            timer.Start();
            BruteForce(ref min, ref max, ref sum);
            timer.Stop();
            Console.WriteLine("Brute force algorithm results: " + Environment.NewLine + "\tStarting index = " + min.ToString()
                + Environment.NewLine + "\tEnding index = " + max.ToString() + Environment.NewLine + "\tSum = " +
                sum.ToString() + "\tTime elapsed = " + timer.ElapsedTicks.ToString() + Environment.NewLine);
            timer.Reset();

            min = 0;    //re-initialize values
            max = 0;
            sum = 0;

            //Execute the recursive algorithm and measure the running time.
            timer.Start();
            Recursive(0, theArray.Length - 1, out min, out max, out sum);
            timer.Stop();
            Console.WriteLine("Recursive algorithm results: " + Environment.NewLine + "\tStarting index = " + min.ToString()
                + Environment.NewLine + "\tEnding index = " + max.ToString() + Environment.NewLine + "\tSum = " +
                sum.ToString() + "\tTime elapsed = " + timer.ElapsedTicks.ToString() + Environment.NewLine);

            //Exit.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        //Brute force algorithm
        static void BruteForce(ref int low, ref int high, ref int total)
        {
            int sum = int.MinValue;     //used for keeping a running sum
            for (int i = 0; i < theArray.Length; i++)
            {
                //Initialize the sum to the first element.
                sum = theArray[i];
                if (total < sum)
                {
                    total = sum;
                    low = i;
                    high = i;
                }

                //Loop through until a greater sum is found.
                for (int j = i + 1; j < theArray.Length; j++)
                {
                    sum += theArray[j];
                    if (total < sum)
                    {
                        total = sum;
                        low = i;
                        high = j;
                    }
                }
            }
        }

        //Recursive algorithm
        static void Recursive(int start, int finish, out int low, out int high, out int total)
        {
            low = 0;    //initualization of output values.
            high = 0;
            total = int.MinValue;

            int leftSum = int.MinValue;  //maximum subarray values for left, right, and crossing subarrays
            int leftLow = 0;
            int leftHigh = 0;
            int rightSum = int.MinValue;
            int rightLow = 0;
            int rightHigh = 0;
            int crossSum = int.MinValue;
            int crossLow = 0;
            int crossHigh = 0;

            int mid = 0;    //middle of array

            // If high == low, we have base case: only one element.
            //Find midpoint, then run functions to find the maximum subarray on the left side, right side, and middle.
            if (start != finish)
            {
                mid = (start + finish)/2;
                Recursive(start, mid, out leftLow, out leftHigh, out leftSum);
                Recursive(mid + 1, finish, out rightLow, out rightHigh, out rightSum);
                FindMaxCrossingSubarray(start, mid, finish, out crossLow, out crossHigh, out crossSum);

                //If the left subarray contains the maximum subarray, return it.
                if (leftSum >= rightSum && leftSum >= crossSum)
                {
                    low = leftLow;
                    high = leftHigh;
                    total = leftSum;
                }

                //If the right subarray contains the maximum subarray, return it.
                else if (rightSum >= leftSum && rightSum >= crossSum)
                {
                    low = rightLow;
                    high = rightHigh;
                    total = rightSum;
                }

                //If maximum subarray crosses the left and right subarrays, return the values of the crossing subarray.
                else
                {
                    low = crossLow;
                    high = crossHigh;
                    total = crossSum;
                }
            }
        }

        //Helper function for the Recursive method. Returns the values for the maximum subarray crossing the midpoint.
        static void FindMaxCrossingSubarray(int start, int mid, int finish, out int low, out int high, out int total)
        {
            total = 0;                  //initialization of output values
            low = 0;
            high = 0;
            int leftSum = int.MinValue;           //left and right components of output
            int leftMaxIndex = mid;
            int rightSum = int.MinValue;
            int rightMaxIndex = mid;

            //If left subarray is a single element, return a sum of 0.
            if (mid == start)
            {
                leftMaxIndex = start;
                leftSum = theArray[start];
            }
            else
            {
                //Investigate left subarray.
                for (int i = mid; i >= start; i--)
                {
                    total += theArray[i];

                    //If a sum is found greater than the greatest previous sum, store it as the greatest sum.
                    if (total > leftSum)
                    {
                        leftSum = total;
                        leftMaxIndex = i;
                    }
                }
            }

            //Investigate right subarray.
            total = 0;
            for (int i = mid + 1; i <= finish; i++)
            {
                total += theArray[i];
                //If a sum is found greater than the greatest previous sum, store it as the greatest sum.
                if (total > rightSum)
                {
                    rightSum = total;
                    rightMaxIndex = i;
                }
            }

            //Combine and return the values found.
            low = leftMaxIndex;
            high = rightMaxIndex;
            total = leftSum + rightSum;

        }

    }
}
