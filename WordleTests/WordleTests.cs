using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wordle;

namespace WordleTests
{
    [TestClass]
    public class WordleTests
    {
        [TestMethod]
        public void WordleGetWord_GuessRight_StatusSuccess()
        {
            // Arrange
            var wordle = new Wordle.Wordle();
            wordle.Initialize("Resources\\dictionary.txt", "Resources\\words.txt");

            // Act
            var status = wordle.GetWord("rebut", 1);

            // Assert
            Assert.AreEqual(WordStatusEnum.Success, status.WordStatus);
            Assert.AreEqual(LetterStatusEnum.Green, status.LettersStatus[0].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Green, status.LettersStatus[1].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Green, status.LettersStatus[2].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Green, status.LettersStatus[3].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Green, status.LettersStatus[4].LetterStatusEnum);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void WordleGetWord_ShortWord_ArgumentException()
        {
            // Arrange
            var wordle = new Wordle.Wordle();
            wordle.Initialize("Resources\\dictionary.txt", "Resources\\words.txt");

            // Act
            var status = wordle.GetWord("rebu", 1);

            // Assert
            // Exception is thrown

        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void WordleGetWord_LongWord_ArgumentException()
        {
            // Arrange
            var wordle = new Wordle.Wordle();
            wordle.Initialize("Resources\\dictionary.txt", "Resources\\words.txt");

            // Act
            var status = wordle.GetWord("rebuty", 1);

            // Assert
            // Exception is thrown

        }

        [TestMethod]
        public void WordleGetWord_WordNotInDictionary_StatusNoSuchWord()
        {
            // Arrange
            var wordle = new Wordle.Wordle();
            wordle.Initialize("Resources\\dictionary.txt", "Resources\\words.txt");

            // Act
            var status = wordle.GetWord("aaaaa", 1);

            // Assert
            Assert.AreEqual(WordStatusEnum.NoSuchWord, status.WordStatus);

        }

        [TestMethod]
        public void WordleGetWord_GuessWrong_StatusKeepTrying()
        {
            // Arrange
            var wordle = new Wordle.Wordle();
            wordle.Initialize("Resources\\dictionary.txt", "Resources\\words.txt");

            // Act
            var status = wordle.GetWord("clock", 1);

            // Assert
            Assert.AreEqual(WordStatusEnum.KeepTrying, status.WordStatus);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[0].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[1].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[2].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[3].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[4].LetterStatusEnum);
        }

        [TestMethod]
        public void WordleGetWord_GuessWrong_SomeLettersGreen()
        {
            // Arrange
            var wordle = new Wordle.Wordle();
            wordle.Initialize("Resources\\dictionary.txt", "Resources\\words.txt");

            // Act
            var status = wordle.GetWord("robot", 1);

            // Assert
            Assert.AreEqual(WordStatusEnum.KeepTrying, status.WordStatus);
            Assert.AreEqual(LetterStatusEnum.Green, status.LettersStatus[0].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[1].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Green, status.LettersStatus[2].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[3].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Green, status.LettersStatus[4].LetterStatusEnum);
        }

        [TestMethod]
        public void WordleGetWord_GuessWrong_SomeLettersYellow()
        {
            // Arrange
            var wordle = new Wordle.Wordle();
            wordle.Initialize("Resources\\dictionary.txt", "Resources\\words.txt");

            // Act
            var status = wordle.GetWord("grave", 1);

            // Assert
            Assert.AreEqual(WordStatusEnum.KeepTrying, status.WordStatus);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[0].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Yellow, status.LettersStatus[1].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[2].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[3].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Yellow, status.LettersStatus[4].LetterStatusEnum);
        }

        [TestMethod]
        public void WordleGetWord_GuessWrongDoubleLetter_OneLetterGreenOneMissing()
        {
            // Arrange
            var wordle = new Wordle.Wordle();
            wordle.Initialize("Resources\\dictionary.txt", "Resources\\words.txt");

            // Act
            var status = wordle.GetWord("beeps", 1);

            // Assert
            Assert.AreEqual(WordStatusEnum.KeepTrying, status.WordStatus);
            Assert.AreEqual(LetterStatusEnum.Yellow, status.LettersStatus[0].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Green, status.LettersStatus[1].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[2].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[3].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[4].LetterStatusEnum);
        }

        [TestMethod]
        public void WordleGetWord_LetterAppears3Times_OneLetterGreenTwoYellows()
        {
            // Arrange
            var wordle = new Wordle.Wordle();
            wordle.Initialize("Resources\\dictionary.txt", "Resources\\words.txt");

            // Act
            var status = wordle.GetWord("esses", 2);

            // Assert
            Assert.AreEqual(WordStatusEnum.KeepTrying, status.WordStatus);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[0].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Yellow, status.LettersStatus[1].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Green, status.LettersStatus[2].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[3].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Yellow, status.LettersStatus[4].LetterStatusEnum);
        }

        [TestMethod]
        public void WordleGetWord_LetterAppears3Times_OneLetterGreenOneYellowOneMissing()
        {
            // Arrange
            var wordle = new Wordle.Wordle();
            wordle.Initialize("Resources\\dictionary.txt", "Resources\\words.txt");

            // Act
            var status = wordle.GetWord("eeven", 7);

            // Assert
            Assert.AreEqual(WordStatusEnum.KeepTrying, status.WordStatus);
            Assert.AreEqual(LetterStatusEnum.Green, status.LettersStatus[0].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Yellow, status.LettersStatus[1].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Yellow, status.LettersStatus[2].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[3].LetterStatusEnum);
            Assert.AreEqual(LetterStatusEnum.Missing, status.LettersStatus[4].LetterStatusEnum);
        }
    }
}
