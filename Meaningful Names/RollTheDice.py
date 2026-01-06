import random

def roll_dice(number_of_sides):
    return random.randint(1, number_of_sides)


def run_dice_game():
    total_sides = 6
    is_game_running = True

    while is_game_running:
        user_input = input("Ready to roll? Enter q to Quit: ")

        if user_input.lower() == "q":
            is_game_running = False
        else:
            rolled_value = roll_dice(total_sides)
            print(f"You rolled a {rolled_value}")


run_dice_game()