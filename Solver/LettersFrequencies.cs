using System;
using System.Collections.Generic;
using System.Linq;

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

        public string GetBestWord(List<string> words)
        {
            List<Word> rankedWords = new List<Word>();
            foreach(var word in words)
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
    }

    class Word
    {
        public string Text { get; set; }
        public float Rank { get; set; }
    }
}
