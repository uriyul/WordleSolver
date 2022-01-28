using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace WordleTests
{
    [TestClass]
    public class SolverTests
    {
        [TestMethod]
        public void Solver_GuessFirstWord_SucceedIn6OrLess()
        {
            // arrange
            var solver = new Solver.Solver(true);

            // act
            (string word, int attempts) = solver.Start("stare", 1);

            // assert
            Assert.AreEqual("rebut", word);
            Assert.IsTrue(attempts <= 6);
        }

        [TestMethod]
        public void Solver_Guess10Words_SucceedIn6OrLess()
        {
            // arrange
            var solver = new Solver.Solver(true);

            // act
            int sum = 0;
            for (int i = 50; i < 60; i++)
            {
                (string _, int attempts) = solver.Start("stare", 1);
                sum += attempts;
            }

            float average = sum / 10f;

            // assert
            Assert.IsTrue(average < 5);
        }

        [TestMethod]
        public void Solver_GuessWord21StartinWithAahed_SucceedIn6OrLess()
        {
            // arrange
            var solver = new Solver.Solver(true);

            // act
            (string word, int attempts) = solver.Start("aahed", 21);

            // assert
            Assert.AreEqual("death", word);
            Assert.IsTrue(attempts <= 6);
        }
    }
}
