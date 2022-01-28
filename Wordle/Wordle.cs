using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Wordle
{
    public class Wordle
    {
        private List<string> _dictionary;
        private List<string> _words;

        public void Initialize(string dictionaryPath, string wordsPath)
        {
            InitWords(wordsPath);
            InitDictionary(dictionaryPath);
        }

        private void InitDictionary(string filePath)
        {
            _dictionary = File.ReadAllLines(filePath).ToList();
            _dictionary = _dictionary.Union(_words).ToList();
        }

        private void InitWords(string filePath)
        {
            _words = File.ReadAllLines(filePath).ToList();
        }

        public Status GetWord(string attempt, int index)
        {
            if (attempt.Length != 5)
                throw new ArgumentException("Must be a 5 letter word");
            
            string realWord = _words[index];

            var lettersStatus = new LetterStatus[]
            {
                new LetterStatus {Letter = attempt[0] },
                new LetterStatus {Letter = attempt[1] },
                new LetterStatus {Letter = attempt[2] },
                new LetterStatus {Letter = attempt[3] },
                new LetterStatus {Letter = attempt[4] }
            };

            if(realWord.Equals(attempt))
            {
                for (int i = 0; i< 5; i++)
                {
                    lettersStatus[i].LetterStatusEnum = LetterStatusEnum.Green;
                }

                var status = new Status
                { 
                    WordStatus = WordStatusEnum.Success ,
                    LettersStatus = lettersStatus
                };

                return status;
            }

            if (!_dictionary.Contains(attempt))
            {
                var status = new Status
                {
                    WordStatus = WordStatusEnum.NoSuchWord,
                    LettersStatus = lettersStatus
                };

                return status;
            }

            var letters = new char[] { realWord[0], realWord[1], realWord[2], realWord[3], realWord[4] };
            for (int i = 0; i < 5; i++)
            {
                if (realWord[i] == attempt[i])
                {
                    lettersStatus[i].LetterStatusEnum = LetterStatusEnum.Green;
                    letters[i] = ' ';
                }
            }

            for (int i = 0; i < 5; i++)
            { 
                if(lettersStatus[i].LetterStatusEnum == LetterStatusEnum.Green)
                {
                    continue;
                }
                else if(letters.Contains(attempt[i]))
                {
                    lettersStatus[i].LetterStatusEnum = LetterStatusEnum.Yellow;
                    var lindex = Array.IndexOf(letters, attempt[i]);
                    letters[lindex] = ' ';
                }
                else
                {
                    lettersStatus[i].LetterStatusEnum = LetterStatusEnum.Missing;
                }
            }

            return new Status
            {
                WordStatus = WordStatusEnum.KeepTrying,
                LettersStatus = lettersStatus
            };

        }
    }
}
