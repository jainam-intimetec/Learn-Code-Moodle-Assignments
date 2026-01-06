using NumberGuessingGame.Game;
using NumberGuessingGame.Settings;

class Program
{
    static void Main()
    {
        IGameSettings settings = new StandardGameSettings();
        GameController game = new GameController(settings);
        game.Play();
    }
}