using System;
using System.Collections.Generic;
using System.Text;

namespace Wordle
{
    public enum LetterStatusEnum
    {
        Missing,
        Yellow,
        Green
    }

    public enum WordStatusEnum
    {
        NoSuchWord,
        KeepTrying,
        Success
    }

    public class LetterStatus
    {
        public char Letter { get; set; }
        public LetterStatusEnum LetterStatusEnum { get; set; }
    }

    public class Status
    {
        public WordStatusEnum WordStatus { get; set; }
        public LetterStatus[] LettersStatus {get; set;}
    }
}
