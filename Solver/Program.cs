using System;

namespace Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            var initialWord = "noise";
            Console.WriteLine($"Starting with {initialWord}");

            RunBatch(78, 78, initialWord);
        }

        private static void RunBatch(int startIndex, int endIndex, string initialWord)
        {
            var solver = new Solver(false);
            var startTime = DateTime.Now;
            int sum = 0;
            int min = 999;
            int max = 0;

            for (int i = startIndex; i <= endIndex; i++)
            {
                (string word, int attempts) = solver.Start(initialWord, i);
                Console.WriteLine($"Attempts to solve word '{word}' ({i}): {attempts}");
                sum += attempts;
                if (attempts > max) max = attempts;
                if (attempts < min) min = attempts;
            }

            var elapsed = DateTime.Now - startTime;
            Console.WriteLine($"Elapsed time: {elapsed}");
            Console.WriteLine($"Min attempts: {min}");
            Console.WriteLine($"Max attempts: {max}");
            Console.WriteLine($"Average attempts: {((float)sum)/((float)(endIndex-startIndex+1))}");

        }
    }
}
