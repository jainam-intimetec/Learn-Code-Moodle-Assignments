import random

def is_valid_guess(user_input):
    return user_input.isdigit() and 1 <= int(user_input) <= 100


def run_number_guessing_game():
    secret_number = random.randint(1, 100)
    is_guess_correct = False
    total_guesses = 0

    user_input = input("Guess a number between 1 and 100: ")

    while not is_guess_correct:
        if not is_valid_guess(user_input):
            user_input = input("Invalid input. Please enter a number between 1 and 100: ")
            continue

        total_guesses += 1
        guessed_number = int(user_input)

        if guessed_number < secret_number:
            user_input = input("Too low. Guess again: ")
        elif guessed_number > secret_number:
            user_input = input("Too high. Guess again: ")
        else:
            print(f"You guessed it in {total_guesses} guesses!")
            is_guess_correct = True


run_number_guessing_game()