using System;
using NumberGuessingGame.Settings;
using NumberGuessingGame.Services;

namespace NumberGuessingGame.Game
{
    public class GameController
    {
        private readonly IGameSettings settings;
        private readonly NumberGenerator generator;
        private readonly GuessValidator validator;
        private readonly GuessEvaluator evaluator;

        public GameController(IGameSettings settings)
        {
            this.settings = settings;
            generator = new NumberGenerator();
            validator = new GuessValidator(settings);
            evaluator = new GuessEvaluator();
        }

        public void Play()
        {
            int secretNumber = generator.GenerateValue(settings);
            int guessCount = 0;

            while (true)
            {
                Console.Write($"Please enter a number between {settings.MinValue} to {settings.MaxValue}: ");
                string input = Console.ReadLine();

                if (!validator.IsInputValid(input, out int guess))
                {
                    Console.WriteLine("I won't count this one.");
                    continue;
                }
                 
                guessCount++;

                if (evaluator.IsGuessCorrect(guess, secretNumber))
                {
                    Console.WriteLine($"You guessed it in {guessCount} guesses!");
                    break;
                }
            }
        }
    }
}
