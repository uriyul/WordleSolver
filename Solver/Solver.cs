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
        //private List<string> _words = new List<string>();
        private List<string> _dictionary;
        private bool _silent = false;

        public Solver(bool silent = false)
        {
            _silent = silent;
        }

        public (string, int) Start(string initialWord, int wordIndex)
        {
            var guessData = new GuessData();
            //HashSet<char> missingLetters = new HashSet<char>();
            //HashSet<char> existingLetters = new HashSet<char>();
            //List<char>[] notHere = { new List<char>(), new List<char>(), new List<char>(), new List<char>(), new List<char>() };

            guessData.Words.Add(initialWord);
            //string[] template = { @"\w", @"\w", @"\w", @"\w", @"\w" };

            _dictionary = File.ReadAllLines("Resources\\dictionary.txt").Union(File.ReadAllLines("Resources\\words.txt")).ToList();

            var wordle = new Wordle.Wordle();
            wordle.Initialize("Resources\\dictionary.txt", "Resources\\words.txt");
            var status = wordle.GetWord(guessData.Words.First(), wordIndex);
            if (!_silent)
            {
                Console.Write("1. ");
                printStatus(status);
            }
            Analyze(status, guessData);

            int i;
            for(i=2;  status.WordStatus != WordStatusEnum.Success; i++)
            {
                string word = GetNextAttempt(guessData);
                guessData.Words.Add(word);
                status = wordle.GetWord(word, wordIndex);
                if (!_silent)
                {
                    Console.Write($"{i}. ");
                    printStatus(status);
                }
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

        private void UpdateTemplate(string[] template, HashSet<char> missingLetters, List<char>[] notHere)
        {
            for (int i = 0; i < 5; i++)
            {
                if(template[i].Length == 1)
                {
                    continue;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("[^");
                foreach(var letter in missingLetters)
                {
                    sb.Append(letter);
                }
                foreach(var letter in notHere[i])
                {
                    sb.Append(letter);
                }
                sb.Append("]");
                template[i] = sb.ToString();
            }
        }

        private string GetNextAttempt(GuessData guessData)
        {
            UpdateTemplate(guessData.Template, guessData.MissingLetters, guessData.NotHere);

            Regex regex = new Regex($"^{guessData.Template[0]}{guessData.Template[1]}{guessData.Template[2]}{guessData.Template[3]}{guessData.Template[4]}$");
            var candidates = _dictionary.Where(x => regex.IsMatch(x));
            if (guessData.ExistingLetters.Count > 0)
            {
                var existingLettersArray = guessData.ExistingLetters.ToArray();
                candidates = candidates.Where(w => guessData.ExistingLetters.All(w.Contains));

            }
            candidates = candidates.Where(x => !guessData.Words.Contains(x));

            var lettersFrequencies = new LettersFrequencies();
            return lettersFrequencies.GetBestWord(candidates.ToList());
        }

        private void printStatus(Status status)
        {
            if(status.WordStatus == WordStatusEnum.NoSuchWord)
            {
                Console.WriteLine($"There is no such word as {status.LettersStatus[0].Letter}{status.LettersStatus[1].Letter}{status.LettersStatus[2].Letter}{status.LettersStatus[3].Letter}{status.LettersStatus[4].Letter}");
                return;
            }

            for(int i=0; i<5; i++)
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
