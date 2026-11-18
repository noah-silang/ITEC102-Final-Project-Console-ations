using System;
using System.Collections.Generic;
using System.Linq;

class Donation
{
    public string DonorName {get; set;} = "";
    public string DonationType {get; set;} = "";
    public decimal? DonationAmount {get; set;}
    public string ItemName {get; set;} = "";
    public int? ItemQuantity {get; set;}
    public DateTime DateRecorded {get; set;}
}

class Program
{
    static List<Donation> donationRecords = new List<Donation>();
    static Dictionary<string, int> itemTotals = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
    static decimal totalMoney = 0;
    static decimal donationTarget = 0;
    static bool targetHasBeenSet = false;

    static void SuccessMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    static void WarningMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    static void ErrorMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    static void InfoMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    static void RecordDonation()
    {
        string donorName = "";

        while (true)
        {
            Console.Write("\nEnter donor name: ");
            donorName = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(donorName))
            {
                ErrorMessage("Donor name cannot be empty!");
            }
            else if (!donorName.Any(char.IsLetter))
            {
                ErrorMessage("Donor name must include letters!");
            }
            else if (donorName.Length > 100)
            {
                ErrorMessage("Donor name must not exceed 100 characters!");
            }
            else
            {
                break;
            }
        }

        Console.WriteLine("\nDonation Types");
        Console.WriteLine("1. Monetary Donation");
        Console.WriteLine("2. Donated Goods");
        int donationType;

        while (true)
        {
            Console.Write("\nEnter choice (1 or 2): ");
            string input = Console.ReadLine() ?? "";

            if (!int.TryParse(input, out donationType) || donationType != 1 && donationType != 2)
            {
                ErrorMessage("Invalid donation type!");
            }
            else
            {
                break;
            }
        }

        Donation donationRecord = new Donation();

        donationRecord.DonorName = donorName;

        if (donationType == 1)
        {
            decimal donationAmount;

            while (true)
            {
                Console.Write("\nEnter donation amount (PHP): ");
                string input = Console.ReadLine() ?? "";

                string errorMessage = "";

                if (!decimal.TryParse(input, out donationAmount))
                {
                    errorMessage = "Invalid donation amount!";
                }
                else if (donationAmount <= 0)
                {
                    errorMessage = "Donation amount must be greater than 0!";
                }
                else if (donationAmount > 1000000000m)
                {
                    errorMessage = "Donation amount must be less than 1 billion!";
                }
                else if (input.Contains('.') && input.Substring(input.IndexOf('.') + 1).Length > 2)
                {
                    errorMessage = "Maximum of 2 decimal places allowed!";
                }
                else
                {
                    break;
                }
                
                ErrorMessage(errorMessage);
            }

            donationRecord.DonationType = "Monetary";
            donationRecord.DonationAmount = decimal.Round(donationAmount, 2);
        }
        else
        {
            string itemName;

            while (true)
            {
                Console.Write("\nEnter item name: ");
                itemName = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(itemName))
                {
                    ErrorMessage("Item name cannot be empty!");
                }
                else if (!itemName.Any(char.IsLetter))
                {
                    ErrorMessage("Item name must include letters!");
                }
                else if (itemName.Length > 100)
                {
                    ErrorMessage("Item name must not exceed 100 characters!");
                }
                else
                {
                    break;
                }
            }

            int itemQuantity;

            while (true)
            {
                Console.Write("\nEnter item quantity: ");
                string input = Console.ReadLine() ?? "";

                string errorMessage = "";

                if (!int.TryParse(input, out itemQuantity))
                {
                    errorMessage = "Invalid item quantity!";
                }
                else if (itemQuantity <= 0)
                {
                    errorMessage = "Item quantity must be greater than 0!";
                }
                else if (itemQuantity > 1000000)
                {
                    errorMessage = "Item quantity must be less than 1 million!";
                }
                else
                {
                    break;
                }

                ErrorMessage(errorMessage);
            }

            donationRecord.DonationType = "Goods";
            donationRecord.ItemName = itemName;
            donationRecord.ItemQuantity = itemQuantity;
        }

        donationRecord.DateRecorded = DateTime.Now;

        donationRecords.Add(donationRecord);

        SuccessMessage("\nDonation recorded successfully!");
    }

    static void ViewDonations()
    {
        if (donationRecords.Count == 0)
        {
            Console.WriteLine("\nNo donation records found!");
        }
        else
        {
            Console.WriteLine("\n═════ DONATION RECORDS ═════");

            foreach (Donation record in donationRecords)
            {
                Console.WriteLine();
                Console.WriteLine("Donor: " + record.DonorName);

                if (record.DonationType == "Monetary")
                {
                    Console.WriteLine("Type: Monetary Donation");
                    Console.WriteLine("Amount: PHP " + record.DonationAmount?.ToString("N2") ?? "0.00");
                }
                else
                {
                    Console.WriteLine("Type: Donated Goods");
                    Console.WriteLine("Item: " + record.ItemName);
                    Console.WriteLine("Quantity: " + record.ItemQuantity?.ToString("N0") ?? "0");
                }

                Console.WriteLine("Date Recorded: " + record.DateRecorded.ToString("MMMM dd, yyyy hh:mm tt"));
            }
        }
    }

    static void SearchDonation()
    {
        Console.Write("\nEnter donor name to search (or press Enter to cancel): ");
        string searchName = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(searchName))
        {
            Console.WriteLine("Search cancelled.");
            return;
        }

        bool matchFound = false;

        foreach (Donation record in donationRecords)
        {
            if (string.Equals(
                record.DonorName,
                searchName,
                StringComparison.OrdinalIgnoreCase))
            {
                if (matchFound == false)
                {
                    Console.WriteLine("\n═════ MATCHING DONATIONS ═════");
                }

                Console.WriteLine();
                Console.WriteLine("Donor: " + record.DonorName);

                if (record.DonationType == "Monetary")
                {
                    Console.WriteLine("Amount: PHP " + (record.DonationAmount?.ToString("N2") ?? "0.00"));
                }
                else
                {
                    Console.WriteLine("Item: " + record.ItemName);
                    Console.WriteLine("Quantity: " + (record.ItemQuantity?.ToString("N0") ?? "0"));
                }

                Console.WriteLine("Date Recorded: " + record.DateRecorded.ToString("MMMM dd, yyyy hh:mm tt"));

                matchFound = true;
            }
        }

        if (matchFound == false)
        {
            Console.WriteLine("\nNo donation records found for \"" + searchName + "\"!");
        }
    }

    static void CalculateTotals()
    {
        totalMoney = 0;
        itemTotals.Clear();

        foreach (Donation record in donationRecords)
        {
            if (record.DonationType == "Monetary")
            {
                totalMoney += record.DonationAmount ?? 0;
            }
            else
            {
                if (itemTotals.ContainsKey(record.ItemName))
                {
                    itemTotals[record.ItemName] += record.ItemQuantity ?? 0;
                }
                else
                {
                    itemTotals.Add(
                        record.ItemName,
                        record.ItemQuantity ?? 0);
                }
            }
        }
    }

    static decimal CalculateRemainingAmount()
    {
        decimal remainingAmount = donationTarget - totalMoney;

        if (remainingAmount < 0)
        {
            remainingAmount = 0;
        }

        return remainingAmount;
    }

    static void ViewSummary()
    {
        int recordCount = donationRecords.Count;

        Console.WriteLine("\n═════ DONATION SUMMARY ═════");
        Console.WriteLine("Total Donation Records: " + recordCount);

        CalculateTotals();

        Console.WriteLine("\nTotal Monetary Donations: PHP " + totalMoney.ToString("N2"));

        Console.WriteLine("\nTotal Donated Goods:");
        if (itemTotals.Count == 0)
        {
            Console.WriteLine("No donated goods recorded!");
        }
        else
        {
            foreach (var item in itemTotals)
            {
                Console.WriteLine(" " + item.Key + ": " + item.Value);
            }
        }

        if (targetHasBeenSet == true)
        {
            decimal remainingAmount = CalculateRemainingAmount();
            decimal progressPercentage = (totalMoney / donationTarget) * 100;

            Console.WriteLine("\nDonation Target: PHP " + donationTarget.ToString("N2"));
            Console.WriteLine("Total Collected: PHP " + totalMoney.ToString("N2"));
            Console.WriteLine("Remaining Amount: PHP " + remainingAmount.ToString("N2"));
            Console.WriteLine("Progress: " + progressPercentage.ToString("F2") + "%");

            if (totalMoney >= donationTarget)
            {
                SuccessMessage("\nStatus: Target Reached!");
            }
            else
            {
                WarningMessage("\nStatus: Target in Progress!");
            }
        }
        else
        {
            Console.WriteLine("\nNo monetary target has been set!");
        }
    }

    static void SetDonationGoal()
    {
        decimal newTarget;

        while (true)
        {
            Console.Write("\nEnter donation target (PHP) (or press Enter to cancel): ");
            string input = Console.ReadLine() ?? "";
            
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Setting donation target cancelled.");
                return;
            }

            string errorMessage = "";

            if (!decimal.TryParse(input, out newTarget))
            {
                errorMessage = "Invalid donation target!";
            }
            else if (newTarget <= 0)
            {
                errorMessage = "Donation target must be greater than 0!";
            }
            else if (newTarget > 1000000000m)
            {
                errorMessage = "Donation target must be less than 1 billion!";
            }
            else if (input.Contains('.') && input.Substring(input.IndexOf('.') + 1).Length > 2)
            {
                errorMessage = "Maximum of 2 decimal places allowed!";
            }
            else
            {
                break;
            }
            
            ErrorMessage(errorMessage);
        }

        donationTarget = newTarget;
        targetHasBeenSet = true;

        CalculateTotals();

        decimal remainingAmount = CalculateRemainingAmount();
        decimal progressPercentage = (totalMoney / donationTarget) * 100;

        Console.WriteLine("\nDonation Target: PHP " + donationTarget.ToString("N2"));
        Console.WriteLine("Total Collected: PHP " + totalMoney.ToString("N2"));
        Console.WriteLine("Remaining Amount: PHP " + remainingAmount.ToString("N2"));
        Console.WriteLine("Progress: " + progressPercentage.ToString("F2") + "%");

        if (totalMoney >= donationTarget)
        {
            Console.WriteLine("Donation target reached!");
        }

        SuccessMessage("\nDonation target set!");
    }

    static void Main()
    {
        string? input;
        string menuChoice = "";

        do
        {
            Console.Clear();

            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║                CONSOLE-ATIONS                ║");
            Console.WriteLine("║         A Donation Management System         ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.WriteLine(" [1] Record a Donation");
            Console.WriteLine(" [2] View All Donations");
            Console.WriteLine(" [3] Search for a Donor");
            Console.WriteLine(" [4] View Donation Summary");
            Console.WriteLine(" [5] Manage Donation Goal");
            Console.WriteLine(" [6] Exit");

            Console.Write("\nEnter your choice: ");
            input = Console.ReadLine();
            if (input != null)
            {
                menuChoice = input.ToLower().Trim();
            }

            switch (menuChoice)
            {
                case "1":
                    RecordDonation();
                    InfoMessage("\nPress Enter to return to the main menu.");
                    input = Console.ReadLine();
                    break;
                case "2":
                    ViewDonations();
                    InfoMessage("\nPress Enter to return to the main menu.");
                    input = Console.ReadLine();
                    break;
                case "3":
                    SearchDonation();
                    InfoMessage("\nPress Enter to return to the main menu.");
                    input = Console.ReadLine();
                    break;
                case "4":
                    ViewSummary();
                    InfoMessage("\nPress Enter to return to the main menu.");
                    input = Console.ReadLine();
                    break;
                case "5":
                    SetDonationGoal();
                    InfoMessage("\nPress Enter to return to the main menu.");
                    input = Console.ReadLine();
                    break;
                case "6":
                    SuccessMessage("\nThank you for supporting CONSOLE-ATIONS!");
                    break;
                default:
                    ErrorMessage("Invalid choice! Press Enter to continue.");
                    input = Console.ReadLine();
                    break;
            }
        } while (menuChoice != "6");        
    }
}