using NumberGuessingGame.Settings;
using System;

namespace NumberGuessingGame.Services
{
    public class NumberGenerator
    {
        private readonly Random random = new Random();

        public int GenerateValue(IGameSettings settings)
        {
            return random.Next(settings.MinValue, settings.MaxValue + 1);
        }
    }
}
