def calculate_armstrong_sum(number):
    armstrong_sum = 0
    count = 0

    temp_number = number
    while temp_number > 0:
        count += 1
        temp_number //= 10

    temp_number = number
    while temp_number > 0:
        digit = temp_number % 10
        armstrong_sum += digit ** count
        temp_number //= 10

    return armstrong_sum


def check_armstrong_number():
    try:
        user_input = input("Please enter a number to check for Armstrong: ")
        user_number = int(user_input)

        if user_number < 0:
            raise ValueError("Negative numbers are not allowed")

        if user_number == calculate_armstrong_sum(user_number):
            print(f"{user_number} is an Armstrong Number.")
        else:
            print(f"{user_number} is not an Armstrong Number.")

    except ValueError:
        print("Invalid input. Please enter a valid positive integer.")


check_armstrong_number()
