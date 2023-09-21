// See https://aka.ms/new-console-template for more information
internal class Program
{
    private static void Main(string[] args)
 {   Menu();
    static void Menu(){
        bool exit = true;
        while(exit)
            {
            Console.WriteLine("Main Menu:");
            Console.WriteLine("1. Compass");
            Console.WriteLine("2. Park Fees");
            Console.WriteLine("3. Exit");
            Console.Write("Please select an option (1, 2, or 3): ");
            int choice = Int32.Parse(Console.ReadLine()); 
            if (choice == 1)
            {
                Console.WriteLine("\nYou selected Compass.\n");
                Compass(0,0);
            }
            else if (choice == 2)
            {
                Console.WriteLine("\nYou selected Park Fees.\n");
                ParkFees();
            }
            else if (choice == 3)
            {
                Console.WriteLine("\nGoodbye!\n");
                exit = false; // Exit the program
            }
            else
            {
                Console.WriteLine("\nInvalid selection. Please choose a valid option (1, 2, or 3).\n");
            }
        }
           

        Console.WriteLine();} // Add a newline for readability
    static void Compass(int r, int l){
            int rightTurns = r;
            int leftTurns = l;
            bool RLQ = true;
            Console.WriteLine("Welcome to the Direction Calculator!");
            Console.WriteLine("You start facing North.");
            while (RLQ)
            {
                Console.Write("Enter 'R' for a right turn, 'L' for a left turn, or 'Q' to quit: ");
                string input = Console.ReadLine().ToUpper();

                if (input == "R")
                {
                    rightTurns++;
                }
                else if (input == "L")
                {
                    leftTurns++;
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

            if (direction == 0){
                finalDirection = "North";
            }
            if (direction == 1 || direction == -3){
                finalDirection = "East";
            }
            if (direction == 2 || direction == -2){
                finalDirection = "South";
            }
                if (direction == 3 || direction == -1){
                    finalDirection = "West";
                    }
            Console.WriteLine($"After {rightTurns} right turns and {leftTurns} left turns, you are facing {finalDirection}.");
            Console.WriteLine("Add More Turns? Y / N");
            string YN = Console.ReadLine().ToUpper();
            while (YN =="Y"){
                Compass(rightTurns,leftTurns);
                break;
            }
            while(YN =="N"){
                Console.WriteLine("Menu or Reset? M / R");
            string input2 = Console.ReadLine().ToUpper();
                if (input2 =="M"){
                    break;
                }
                if (input2 =="R"){
                    Compass(0,0);
                    break;
                }
                else{
                    Console.WriteLine("Invalid Option Enter M/R");
                }
            }

        }
    static void ParkFees(){
            double baseFee = 10.0;
            double rvFee = 20.0;
            double discountPercentage = 0.2;
            double taxRate = 0.09;
            double taxAmount = 5.0;
            Console.WriteLine("Welcome to the Fee Payment System!");

            // Prompt for RV status
            bool isRV=false;
            Console.Write("Are you driving an RV? (Y/N): ");
            isRV = Console.ReadLine().ToUpper() == "Y";

            // Prompt for the number of attendees
            Console.Write("Enter the number of attendees: ");
            int numberOfAttendees;
            if (!int.TryParse(Console.ReadLine(), out numberOfAttendees) || numberOfAttendees <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid number of attendees.");
                return;
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

            // Apply child discount
            if (numberOfAttendees > 0)
            {
                Console.Write("Enter the number of child attendees: ");
                int numberOfChildAttendees;
                if (int.TryParse(Console.ReadLine(), out numberOfChildAttendees) && numberOfChildAttendees >= 0 && numberOfChildAttendees <= numberOfAttendees)
                {
                    childDiscount = (numberOfChildAttendees * baseFee * discountPercentage);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number of child attendees.");
                    return;
                }
            }

            totalFee += (numberOfAttendees * baseFee) - childDiscount;
            double tax = totalFee * taxRate;
            totalFee+=tax;
            Console.WriteLine($"Total fee: ${totalFee}");
            Console.Write("Enter the amount paid: $");
            double amountPaid;

            if (double.TryParse(Console.ReadLine(), out amountPaid))
            {
                while (amountPaid < totalFee)
                {
                    totalFee-=amountPaid;
                    amountPaid=0;
                    Console.WriteLine("Error: The amount paid is less than the total fee.");
                    Console.WriteLine($"Remaining fee: ${totalFee}");
                    Console.Write("Enter the amount paid: $");
                    amountPaid=Convert.ToDouble(Console.ReadLine());
                }
                    double change = amountPaid - totalFee;
                    Console.WriteLine($"Change: ${change}");
                    Console.WriteLine("Thank you for your payment!");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid amount.");
            }
        }
    }
}