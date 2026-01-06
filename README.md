# Learn-Code-Moodle-Assignment - Week 1

🔹 Assignment 1
Assignment 1: The below program is to Roll the Dice
import random
def fun(s):
    n=random.randint(1, s)
    return n


def main():
    s=6
    r1=True
    while r1:
        r2=input("Ready to roll? Enter Q to Quit")
        if r2.lower() !="q":
            n=fun(s)
            print("You have rolled a",n)
        else:
            r1=False

Assignment 2: The below program is to guess the correct number between 1 to 100
def fun(s):
    if s.isdigit() and 1<= int(s) <=100:
        return True
    else:
        return False

def main():
    n=random.randint(1,100)
    gn=False
    g=input("Guess a number between 1 and 100:")
    ng=0
    while not gn:
        if not fun(g):
            g=input("I wont count this one Please enter a number between 1 to 100")
            continue
        else:
            ng+=1
            g=int(g)

        if g<n:
            g=input("Too low. Guess again")
        elif g>n:
            g=input("Too High. Guess again")
        else:
            print("You guessed it in",ng,"guesses!")
            gn=True


main()

Assignment 3: The below program is to check whether the number is Armstrong number or not
def fun(N):
    # Initializing Sum and Number of Digits
    s = 0
    t = 0

    # Calculating Number of individual digits
    t2 = N
    while t2 > 0:
        t = t + 1
        t2 = t2 // 10

    # Finding Armstrong Number
    t2 = N
    for n in range(1, t2 + 1):
        R = t2 % 10
        s = s + (R ** t)
        t2 //= 10
    return s


# End of Function

# User Input
N2 = int(input("\nPlease Enter the Number to Check for Armstrong: "))

if (N2 == fun(N2)):
    print("\n %d is Armstrong Number.\n" % N2)
else:
    print("\n %d is Not a Armstrong Number.\n" % N2)

📁 Solution Folder
Meaningful Names

🔹 Assignment 2
Problem Statement

Create a console application that takes the input as a Country Code
(Eg: IN, US, NZ) and displays the adjacent country names (in full).

Languages to Use : Java, C#, TypeScript, C++

📁 Solution Folder
CountryCodes
