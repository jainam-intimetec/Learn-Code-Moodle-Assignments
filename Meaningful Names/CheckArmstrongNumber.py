def CalculateArmstrongSum(number):
    armstrongSum = 0
    count = 0

    tempNumber = number
    while tempNumber > 0:
        count += 1
        tempNumber //= 10

    tempNumber = number
    while tempNumber > 0:
        digit = tempNumber % 10
        armstrongSum += digit ** count
        tempNumber //= 10

    return armstrongSum


def CheckArmstrongNumber():
    userNumber = int(input("Please enter a number to check for Armstrong: "))

    if userNumber == CalculateArmstrongSum(userNumber):
        print(f"{userNumber} is an Armstrong Number.")
    else:
        print(f"{userNumber} is not an Armstrong Number.")


CheckArmstrongNumber()
