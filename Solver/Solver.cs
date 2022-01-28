using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Wordle;
using System.Text.RegularExpressions;
using System.IO;

namespace Solver
{
    public class Solver
    {
        private string[] _dictionary;
        private bool _silent = false;

        public Solver(bool silent = false)
        {
            _silent = silent;
        }

        public (string, int) Start(string initialWord, int wordIndex)
        {
            var guessData = new GuessData();

            guessData.Words.Add(initialWord);

            _dictionary = File.ReadAllLines("Resources\\dictionary.txt").Union(File.ReadAllLines("Resources\\words.txt")).ToArray();

            var wordle = new Wordle.Wordle();
            wordle.Initialize("Resources\\dictionary.txt", "Resources\\words.txt");
            var status = wordle.GetWord(guessData.Words.First(), wordIndex);
            printStatus(status, 1);
            Analyze(status, guessData);

            int i;
            for(i=2;  status.WordStatus != WordStatusEnum.Success; i++)
            {
                string word = GetNextAttempt(guessData);
                guessData.Words.Add(word);
                status = wordle.GetWord(word, wordIndex);
                printStatus(status, i);
                Analyze(status, guessData);
            }

            return (guessData.Words.Last(), i-1);
        }

        private void Analyze(Status status, GuessData guessData)
        {
            for(int i=0; i<5; i++)
            {
                if (status.LettersStatus[i].LetterStatusEnum == LetterStatusEnum.Green)
                {
                    guessData.Template[i] = status.LettersStatus[i].Letter.ToString();
                    guessData.ExistingLetters.Add(status.LettersStatus[i].Letter);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                if(status.LettersStatus[i].LetterStatusEnum == LetterStatusEnum.Green)
                {
                    continue;
                }

                if (status.LettersStatus[i].LetterStatusEnum == LetterStatusEnum.Yellow)
                {
                    guessData.ExistingLetters.Add(status.LettersStatus[i].Letter);
                    guessData.NotHere[i].Add(status.LettersStatus[i].Letter);
                }
                else if(!guessData.ExistingLetters.Contains(status.LettersStatus[i].Letter))
                {
                    guessData.MissingLetters.Add(status.LettersStatus[i].Letter);
                }
            }
        }

        private void UpdateTemplate(GuessData guessData)
        {
            for (int i = 0; i < 5; i++)
            {
                if(guessData.Template[i].Length == 1)
                {
                    continue;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("[^");
                foreach(var letter in guessData.MissingLetters)
                {
                    sb.Append(letter);
                }
                foreach(var letter in guessData.NotHere[i])
                {
                    sb.Append(letter);
                }
                sb.Append("]");
                guessData.Template[i] = sb.ToString();
            }
        }

        private string GetNextAttempt(GuessData guessData)
        {
            UpdateTemplate(guessData);

            // Find words matching the template
            Regex regex = new Regex($"^{guessData.Template[0]}{guessData.Template[1]}{guessData.Template[2]}{guessData.Template[3]}{guessData.Template[4]}$");
            var candidates = _dictionary.Where(x => regex.IsMatch(x));

            // Make sure al existing letters are in the words
            if (guessData.ExistingLetters.Count > 0)
            {
                var existingLettersArray = guessData.ExistingLetters.ToArray();
                candidates = candidates.Where(w => guessData.ExistingLetters.All(w.Contains));

            }

            // Screen out words we already guessed
            candidates = candidates.Where(x => !guessData.Words.Contains(x));

            var lettersFrequencies = new LettersFrequencies();
            
            // Get the best word by letters frequencies
            return lettersFrequencies.GetBestWord(candidates.ToList(), guessData, _dictionary);
        }

        private void printStatus(Status status, int guessNumber)
        {
            if(_silent)
            {
                return;
            }

            if(status.WordStatus == WordStatusEnum.NoSuchWord)
            {
                Console.WriteLine($"There is no such word as {status.LettersStatus[0].Letter}{status.LettersStatus[1].Letter}{status.LettersStatus[2].Letter}{status.LettersStatus[3].Letter}{status.LettersStatus[4].Letter}");
                return;
            }

            Console.Write($"{guessNumber}. ");
            for (int i=0; i<5; i++)
            {
                if(status.LettersStatus[i].LetterStatusEnum == LetterStatusEnum.Green)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                else if (status.LettersStatus[i].LetterStatusEnum == LetterStatusEnum.Yellow)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                }

                Console.Write(status.LettersStatus[i].Letter.ToString().ToUpper());
                Console.ResetColor();
            }
            Console.WriteLine();

        }

    }
}
