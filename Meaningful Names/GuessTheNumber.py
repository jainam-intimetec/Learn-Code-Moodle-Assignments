import random

def IsValidGuess(userInput):
    return userInput.isdigit() and 1 <= int(userInput) <= 100


def RunNumberGuessingGame():
    secretNumber = random.randint(1, 100)
    print(secretNumber)
    isGuessCorrect = False
    totalGuesses = 0

    userInput = input("Guess a number between 1 and 100: ")

    while not isGuessCorrect:
        if not IsValidGuess(userInput):
            userInput = input("Invalid input. Please enter a number between 1 and 100: ")
            continue

        numberOfGuesses += 1
        guessedNumber = int(userInput)

        if guessedNumber < secretNumber:
            userInput = input("Too low. Guess again: ")
        elif guessedNumber > secretNumber:
            userInput = input("Too high. Guess again: ")
        else:
            print(f"You guessed it in {numberOfGuesses} guesses!")
            isGuessCorrect = True


RunNumberGuessingGame()