using System;

namespace NumberGuessingGame.Services
{
    public class GuessEvaluator
    {
        public bool IsGuessCorrect(int guess, int secretNumber)
        {
            if (guess < secretNumber)
            {
                Console.WriteLine("Too low. Guess again.");
                return false;
            }

            if (guess > secretNumber)
            {
                Console.WriteLine("Too high. Guess again.");
                return false;
            }

            return true;
        }
    }
}
