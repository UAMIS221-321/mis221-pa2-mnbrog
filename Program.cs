// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography.X509Certificates;
using System;
using System.IO;
using System.Linq;
using System.Transactions;
internal class Program
{
    struct User
    {
        public string Username;
        public string Password;
        public double Change;// for user to recive change at park desk
        public string DirectionsBack; //reversed output from Compass()
        
    }

    private static void Main(string[] args)
    {
        User[] users = new User[100];
        int userCount = 0;
        int newUserCount = 0;
        int CurrentUser = -1;
        LoadUserDataFromFile(ref users, ref userCount, @"C:\Users\Michael Brogan\PA2\mis221-pa2-mnbrog\userdata.txt");
        Menu(ref users, ref userCount, ref CurrentUser, ref newUserCount);

        static void Menu(ref User[] users, ref int userCount, ref int CurrentUser, ref int newUserCount)
        {
            bool exit = true;
            while (exit)
            {
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1. Compass");
                Console.WriteLine("2. Park Fees");
                Console.WriteLine("3. Sign Up");
                Console.WriteLine("4. Log In");
                Console.WriteLine("5. Change");
                Console.WriteLine("6. Directions Back");
                Console.WriteLine("7. Exit");
                Console.Write("Please select an option (1, 2, 3, 4, 5, 6, or 7): ");

                int choice = Int32.Parse(Console.ReadLine());
                if (choice == 1)
                {
                    Console.WriteLine("\nYou selected Compass.\n");
                     if (CurrentUser != -1)
                    {
                        users[CurrentUser].DirectionsBack = Compass(0, 0);
                    }
                    else
                    {
                        Console.WriteLine("\nYou need to Login or Sign Up to access Compass.\n");
                    }
                    
                }
                else if (choice == 2)
                {
                    Console.WriteLine("\nYou selected Park Fees.\n");
                    if (CurrentUser != -1)
                    {
                        users[CurrentUser].Change = ParkFees();
                    }
                    else
                    {
                        Console.WriteLine("\nYou need to Login or Sign Up to pay Park Fees.\n");
                    }
                }
                else if (choice == 3)
                {
                    Console.WriteLine("\nYou selected Sign Up.\n");
                    SignUp(ref users, ref userCount, ref CurrentUser, ref newUserCount);
                }
                else if (choice == 4)
                {
                    Console.WriteLine("\nYou selected Log In.\n");
                    Login(users, userCount, ref CurrentUser);
                }
                else if (choice == 5)
                {
                    Console.WriteLine("\nYou selected Change.");
                    Console.WriteLine("Change: $" + Math.Round(users[CurrentUser].Change, 2) + "\n");
                }
                else if (choice == 6)
                {
                    Console.WriteLine("\nYou selected Directions Back.");
                    if (CurrentUser != -1)
                    {
                    Console.WriteLine("Directions: " + users[CurrentUser].DirectionsBack);
                    }
                    else
                    {
                        Console.WriteLine("\nYou need to Login or Sign Up for Directions Back.\n");
                    }
                }
                else if (choice == 7)
                {
                    Console.WriteLine("\nGoodbye!");
                    SaveUserDataToFile(users, userCount, newUserCount);
                    exit = false; // Exit the program
                }
                else
                {
                    Console.WriteLine("\nInvalid selection. Please choose a valid option (1-7).");
                }
            }


            Console.WriteLine();
        } // Add a newline for readability

        static string Compass(int r, int l)//Add ons: DirectionsBack, Add More Turns?, Menu or reset?        
        {
            int rightTurns = r;
            int leftTurns = l;
            bool RLQ = true;
            string wayBack="";
            Console.WriteLine("Welcome to the Direction Calculator!");
            Console.WriteLine("You start facing North.");
            while (RLQ)
            {
                Console.Write("Enter 'R' for a right turn, 'L' for a left turn, or 'Q' to quit: ");
                string input = Console.ReadLine().ToUpper();

                if (input == "R")
                {
                    rightTurns++;
                    wayBack+="L";

                }
                else if (input == "L")
                {
                    leftTurns++;
                    wayBack+="R";
                }
                else if (input == "Q")
                {
                    RLQ = false;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'R', 'L', or 'Q'.");
                }
            }

            int direction = (rightTurns - leftTurns) % 4;

            string finalDirection = "";

            if (direction == 0)
            {
                finalDirection = "North";
            }
            if (direction == 1 || direction == -3)
            {
                finalDirection = "East";
            }
            if (direction == 2 || direction == -2)
            {
                finalDirection = "South";
            }
            if (direction == 3 || direction == -1)
            {
                finalDirection = "West";
            }
            string reverse="";
            for(int i=wayBack.Length-1;i>-1;i--){
                    reverse+=wayBack[i];
            }
            wayBack=reverse;
            Console.WriteLine($"After {rightTurns} right turns and {leftTurns} left turns, you are facing {finalDirection}.");
            Console.WriteLine("Add More Turns? Y / N");
            string YN = Console.ReadLine().ToUpper();
            while (YN == "Y")
            {
                Compass(rightTurns, leftTurns);
                return wayBack;
            }
            while (YN == "N")
            {
                Console.WriteLine("Menu or Reset? M / R");
                string input2 = Console.ReadLine().ToUpper();
                if (input2 == "M")
                {
                    return wayBack;
                }
                if (input2 == "R")
                {
                    Compass(0, 0);
                    return wayBack;
                }
                else
                {
                    Console.WriteLine("Invalid Option Enter M/R");
                }
            }
            return "";
        }

        static double ParkFees()//Add ons: Saves Chnage to profile, Rounds change to 2 decimal place
        {
            double baseFee = 10.0;
            double rvFee = 20.0;
            double discountPercentage = 0.2;
            double taxRate = 0.09;
            double taxAmount = 5.0;
            Console.WriteLine("Welcome to the Fee Payment System!");

            // Prompt for RV status
            bool isRV = false;
            string rvp = "";
            while (true)
            {
                Console.Write("Are you driving an RV? (Y/N): ");
                rvp = Console.ReadLine().ToUpper();
                if (rvp == "Y")
                {
                    isRV = true;
                    break;
                }
                else if (rvp == "N")
                {
                    break;
                }
                else { Console.Write("Enter Valid Answer \n"); }
            }

            // Prompt for the number of attendees
            int numberOfAttendees;
            while (true)
            {
                Console.Write("Enter the number of attendees: ");
                numberOfAttendees = 0;
                if (!int.TryParse(Console.ReadLine(), out numberOfAttendees) || numberOfAttendees <= 0)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number of attendees.");
                }
                else
                    break;
            }
            double totalFee = 0;
            if (isRV)
            {
                totalFee += rvFee;
            }
            else
            {
                totalFee += baseFee;
            }

            double childDiscount = 0.0;

            // Calculate child discount
            if (numberOfAttendees >= 6)
            {
                totalFee += taxAmount;
                Console.WriteLine("You are eligible for a $5 tax fee (for 6 or more attendees).");
            }
            int numberOfChildAttendees;
            // Apply child discount
            if (numberOfAttendees > 0)
            {

                while (true)
                {
                    numberOfChildAttendees = 0;
                    Console.Write("Enter the number of child attendees: ");
                    if (int.TryParse(Console.ReadLine(), out numberOfChildAttendees) && numberOfChildAttendees >= 0 && numberOfChildAttendees <= numberOfAttendees)
                    {
                        childDiscount = (numberOfChildAttendees * baseFee * discountPercentage);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number of child attendees.");
                    }
                }
            }

            totalFee += (numberOfAttendees * baseFee) - childDiscount;
            double tax = totalFee * taxRate;
            totalFee += tax;
            while (true)
            {
                Console.WriteLine($"Total fee: ${totalFee}");
                Console.Write("Enter the amount paid: $");
                double amountPaid;

                if (double.TryParse(Console.ReadLine(), out amountPaid))
                {
                    while (amountPaid < totalFee)
                    {
                        totalFee -= amountPaid;
                        amountPaid = 0;
                        Console.WriteLine("Error: The amount paid is less than the total fee.");
                        Console.WriteLine($"Remaining fee: ${totalFee}");
                        Console.Write("Enter the amount paid: $");
                        amountPaid = Convert.ToDouble(Console.ReadLine());
                    }
                    double change = amountPaid - totalFee;
                    Console.WriteLine($"Change: ${Math.Round(change, 2)}");
                    Console.WriteLine("Thank you for your payment!");
                    return change;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid amount.");
                }
            }
        }

        static void SignUp(ref User[] users, ref int userCount, ref int CurrentUser, ref int newUserCount)//ADD ON: Signs in user by creating new user and adding to existing User array 
        {
            Console.Write("Enter a username: ");
            string username = Console.ReadLine();
            Console.Write("Enter a password: ");
            string password = Console.ReadLine();

            if (userCount < users.Length)
            {
                User newUser = new User
                {
                    Username = username,
                    Password = password,
                    Change = 0

                };
                users[userCount] = newUser;
                userCount++;
                newUserCount++;
                CurrentUser = userCount;
                Console.WriteLine("Sign-up successful!");
            }
            else
            {
                Console.WriteLine("User storage is full. Cannot sign up.");
            }
        }

        static void Login(User[] users, int userCount, ref int CurrentUser)//ADD ON: checks through current user array to sign user in
        {
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            bool isAuthenticated = false;
            for (int i = 0; i < userCount + 1; i++)
            {
                if (users[i].Username == username && users[i].Password == password)
                {
                    CurrentUser = i;
                    isAuthenticated = true;
                }
            }
            if (isAuthenticated)
            {
                Console.WriteLine("Login successful!");
            }
            else
            {
                Console.WriteLine("Invalid username or password. Please try again.");
            }
        }

        static void SaveUserDataToFile(User[] users, int userCount, int newUserCount)//Saves user data to local file for later use
        {
            string path = @"C:\Users\Michael Brogan\PA2\mis221-pa2-mnbrog\userdata.txt";
            using (StreamWriter sw = File.AppendText(path))
            {
                for (int i = userCount - newUserCount; i < userCount; i++)
                {
                    sw.WriteLine($"{users[i].Username},{users[i].Password},{users[i].Change}");
                }
            }
        }

        static void LoadUserDataFromFile(ref User[] users, ref int userCount, string fileName)//ADD ON: Loads the above saved data into User array
        {
            if (File.Exists(fileName))
            {
                try
                {
                    string[] lines = File.ReadAllLines(fileName);
                    int count = 0;
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');

                        if (parts.Length == 3)
                        {
                            users[count].Username = parts[0];
                            users[count].Password = parts[1];
                            users[count].Change = double.Parse(parts[2]);
                            count++;

                        }
                        else
                        {
                            Console.WriteLine($"Invalid data format in the file: {line}");
                        }
                    }
                    userCount = count;
                    Console.WriteLine($"Loaded {userCount} users from the file.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while loading user data: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("User data file does not exist.");
            }
        }

    }

}
