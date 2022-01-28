using System;

namespace Wordle
{
    class Program
    {
        static void Main(string[] args)
        {
            var wordle = new Wordle();
            wordle.Initialize("Resources\\dictionary.txt", "Resources\\words.txt");

        }
    }
}
