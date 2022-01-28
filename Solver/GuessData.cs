using System;
using System.Collections.Generic;
using System.Text;

namespace Solver
{
    class GuessData
    {
        public HashSet<char> MissingLetters { get; set; } = new HashSet<char>();
        public HashSet<char> ExistingLetters { get; set; } = new HashSet<char>();
        public List<string> Words { get; set; } = new List<string>();
        public List<char>[] NotHere { get; set; } = { new List<char>(), new List<char>(), new List<char>(), new List<char>(), new List<char>() };
        public string[] Template { get; set; } = { @"\w", @"\w", @"\w", @"\w", @"\w" };

    }
}
