import random

def RollDice(numberOfSides):
    return random.randint(1, numberOfSides)


def RunDiceGame():
    totalSides = 6
    isGameRunning = True

    while isGameRunning:
        userInput = input("Ready to roll? Enter q to Quit: ")

        if userInput.lower() == "q":
            isGameRunning = False
        else:
            rolledValue = RollDice(totalSides)
            print(f"You rolled a {rolledValue}")


RunDiceGame()