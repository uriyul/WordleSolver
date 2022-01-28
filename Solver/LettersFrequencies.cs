using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Solver
{

    class LettersFrequencies
    {
        private Dictionary<char, float> _frequencies = new Dictionary<char, float>
        {
            { 'a', 7.8f},
            { 'b', 2f},
            { 'c', 4},
            { 'd', 3.8f},
            { 'e', 11f},
            { 'f', 1.4f},
            { 'g', 3f},
            { 'h', 2.3f},
            { 'i', 8.2f},
            { 'j', 0.21f},
            { 'k', 2.5f},
            { 'l', 5.3f},
            { 'm', 2.7f},
            { 'n', 7.2f},
            { 'o', 6.1f},
            { 'p', 2.8f},
            { 'q', 0.24f},
            { 'r', 7.3f},
            { 's', 8.7f},
            { 't', 6.7f},
            { 'u', 3.3f},
            { 'v', 1f},
            { 'w', 0.91f},
            { 'x', 0.27f},
            { 'y', 1.6f},
            { 'z', 0.44f},
        };

        public string GetBestWord(List<string> candidates, GuessData guessData, string[] dictionary)
        {
            var greenLettersCount = guessData.Template.Count(x => x.Length == 1);

            if (greenLettersCount >= 3 && candidates.Count > 2)
            {
                var newWord = GetWordOfMissingLetters(candidates, guessData, dictionary);
                if (newWord != null)
                {
                    return newWord;
                }
            }

            return GetBestWordByFrequency(candidates, guessData, dictionary);
        }

        private string GetBestWordByFrequency(List<string> candidates, GuessData guessData, string[] dictionary)
        {
            List<Word> rankedWords = new List<Word>();
            foreach (var word in candidates)
            {
                rankedWords.Add(new Word { Text = word, Rank = GetRank(word) });
            }

            var maxRank = rankedWords.Max(x => x.Rank);
            var maxRankedWord = rankedWords.Where(w => w.Rank == maxRank).Select(x => x.Text).First();

            return maxRankedWord;
        }

        private float GetRank(string word)
        {
            float rank = 0;
            HashSet<char> letters = word.ToHashSet();
            foreach(var letter in letters )
            {
                rank += _frequencies[letter];
            }

            return rank;
        }

        private string GetWordOfMissingLetters(List<string> words, GuessData guessData, string[] dictionary)
        {
            var letters = GetMissingLetters(words, guessData);

            // Find a word with these letters

            var sb = new StringBuilder();
            sb.Append("[");
            foreach(var letter in letters)
            {
                sb.Append(letter);
            }
            sb.Append("]");

            var candidates = GetCandidatesByRegex($"^{sb}{{5}}$", dictionary, guessData);

            if(candidates.Count() == 0)
            {
                // Try finding a word with 4 letters
                candidates = GetCandidatesByRegex($"^(.{sb}{{4}}|{sb}.{sb}{{3}}|{sb}{{2}}.{sb}{{2}}|{sb}{{3}}.{sb}|{sb}{{4}}.)$", dictionary, guessData);

                return candidates.FirstOrDefault();
            }

            return candidates.FirstOrDefault();
        }

        private HashSet<char> GetMissingLetters(List<string> words, GuessData guessData)
        {
            var letters = new HashSet<char>();
            var notGreenIndices = new List<int>();

            for (int i = 0; i < guessData.Template.Length; i++)
            {
                if (guessData.Template[i].Length > 1)
                {
                    notGreenIndices.Add(i);
                }
            }

            foreach (var word in words)
            {
                foreach (int i in notGreenIndices)
                {
                    letters.Add(word[i]);
                }
            }

            return letters;
        }

        private IEnumerable<string> GetCandidatesByRegex(string template, string[] dictionary, GuessData guessData)
        {
            var regex = new Regex(template);
            var candidates = dictionary.Where(x => regex.IsMatch(x));

            // Screen out words we already guessed
            candidates = candidates.Where(x => !guessData.Words.Contains(x));

            // Choose word with most distinct letters
            candidates = candidates.OrderByDescending(x => x.ToHashSet().Count);

            return candidates;
        }
    }

    class Word
    {
        public string Text { get; set; }
        public float Rank { get; set; }
    }
}
