using NumberGuessingGame.Settings;

namespace NumberGuessingGame.Services
{
    public class GuessValidator
    {
        private readonly IGameSettings settings;

        public GuessValidator(IGameSettings settings)
        {
            this.settings = settings;
        }

        public bool IsInputValid(string input, out int guess)
        {
            if (!int.TryParse(input, out guess))
                return false;

            return guess >= settings.MinValue && guess <= settings.MaxValue;
        }
    }
}
