using System;
using System.Collections.Generic;

class Donation
{
    public string DonorName {get; set;}
    public string DonationType {get; set;}
    public decimal? DonationAmount {get; set;}
    public string ItemName {get; set;}
    public int? ItemQuantity {get; set;}
}

class Program
{
    static List<Donation> donationRecords = new List<Donation>();
    static Dictionary<string, int> itemTotals = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
    static decimal totalMoney = 0;
    static decimal donationTarget = 0;
    static bool targetHasBeenSet = false;

    static void RecordDonation()
    {
        string donorName = "";      

        do
        {
            Console.Write("Enter donor name: ");
            donorName = Console.ReadLine().Trim();

            if (string.IsNullOrWhiteSpace(donorName))
            {
                Console.WriteLine("\nDonor name cannot be empty!");
            }
        } while (string.IsNullOrWhiteSpace(donorName));

        Console.WriteLine("\nSelect donation type:");
        Console.WriteLine(" [1] Monetary Donation");
        Console.WriteLine(" [2] Donated Goods");
        Console.Write("Enter choice: ");
        int donationType;

        while (!int.TryParse(Console.ReadLine(), out donationType) || donationType != 1 && donationType != 2)
        {
            Console.Write("\nInvalid donation type! Please select 1 or 2: ");
        }

        Donation donationRecord = new Donation();

        donationRecord.DonorName = donorName;

        if (donationType == 1)
        {
            decimal donationAmount;

            Console.Write("\nEnter donation amount: ");

            while (!decimal.TryParse(Console.ReadLine(), out donationAmount) || donationAmount <= 0)
            {
                Console.Write("\nInvalid donation amount! Enter a valid amount greater than 0: ");
            }

            donationRecord.DonationType = "Monetary";
            donationRecord.DonationAmount = donationAmount;
        }
        else
        {
            string itemName;

            do
            {
                Console.Write("\nEnter item name: ");
                itemName = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(itemName))
                {
                    Console.WriteLine("\nItem name cannot be empty!");
                }
            } while (string.IsNullOrWhiteSpace(itemName));

            int itemQuantity;

            Console.Write("\nEnter item quantity: ");

            while (!int.TryParse(Console.ReadLine(), out itemQuantity) || itemQuantity <= 0)
            {
                Console.Write("\nInvalid item quantity! Enter a valid quantity greater than 0: ");
            }

            donationRecord.DonationType = "Goods";
            donationRecord.ItemName = itemName;
            donationRecord.ItemQuantity = itemQuantity;
        }

        donationRecords.Add(donationRecord);

        Console.WriteLine("\nDonation recorded successfully!");
    }

    static void ViewDonations()
    {
        if (donationRecords.Count == 0)
        {
            Console.WriteLine("No donation records found!");
        }
        else
        {
            Console.WriteLine("═════ DONATION RECORDS ═════\n");

            foreach (Donation record in donationRecords)
            {
                Console.WriteLine("Donor: " + record.DonorName);

                if (record.DonationType == "Monetary")
                {
                    Console.WriteLine("Type: Monetary Donation");
                    Console.WriteLine("Amount: " + record.DonationAmount);
                }
                else
                {
                    Console.WriteLine("Type: Donated Goods");
                    Console.WriteLine("Item: " + record.ItemName);
                    Console.WriteLine("Quantity: " + record.ItemQuantity);
                }

                Console.WriteLine();
            }
        }
    }

    static void SearchDonation()
    {
        Console.Write("Enter donor name to search: ");
        string searchName = Console.ReadLine();

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

                Console.WriteLine("Donor: " + record.DonorName);

                if (record.DonationType == "Monetary")
                {
                    Console.WriteLine("Amount: " + record.DonationAmount);
                }
                else
                {
                    Console.WriteLine("Item: " + record.ItemName);
                    Console.WriteLine("Quantity: " + record.ItemQuantity);
                }

                matchFound = true;
            }
        }

        if (matchFound == false)
        {
            Console.WriteLine($"\nNo matching donation records for {searchName} found!");
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

        Console.WriteLine("═════ DONATION SUMMARY ═════");
        Console.WriteLine("\nTotal Donation Records: " + recordCount);

        CalculateTotals();

        Console.WriteLine("\nTotal Monetary Donations: " + totalMoney);

        Console.WriteLine("\nTotal Donated Goods:");
        if (itemTotals.Count == 0)
        {
            Console.WriteLine("No donated goods recorded!");
        }
        else
        {
            foreach (var item in itemTotals)
            {
                Console.WriteLine(item.Key + ": " + item.Value);
            }
        }

        if (targetHasBeenSet == true)
        {
            decimal remainingAmount = CalculateRemainingAmount();
            decimal progressPercentage = (totalMoney / donationTarget) * 100;

            Console.WriteLine("\nDonation Target: " + donationTarget);
            Console.WriteLine("Total Collected: " + totalMoney);
            Console.WriteLine("Remaining Amount: " + remainingAmount);
            Console.WriteLine("Progress: " + progressPercentage.ToString("F2") + "%");

            if (totalMoney >= donationTarget)
            {
                Console.WriteLine("Status: Target Reached");
            }
            else
            {
                Console.WriteLine("Status: Target in Progress");
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

        Console.Write("Enter donation target: ");
        while (!decimal.TryParse(Console.ReadLine(), out newTarget) || newTarget <= 0)
        {
            Console.Write("\nInvalid target! Enter a valid target greater than 0: ");
        }

        donationTarget = newTarget;
        targetHasBeenSet = true;

        decimal remainingAmount = CalculateRemainingAmount();
        decimal progressPercentage = (totalMoney / donationTarget) * 100;

        Console.WriteLine("\nDonation Target: " + donationTarget);
        Console.WriteLine("Total Collected: " + totalMoney);
        Console.WriteLine("Remaining Amount: " + remainingAmount);
        Console.WriteLine("Progress: " + progressPercentage.ToString("F2") + "%");

        if (totalMoney >= donationTarget)
        {
            Console.WriteLine("Donation target reached!");
        }
    }

    static void Main()
    {
        string? readResult;
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
            readResult = Console.ReadLine();
            if (readResult != null)
            {
                menuChoice = readResult.ToLower().Trim();
            }

            switch (menuChoice)
            {
                case "1":
                    Console.WriteLine();
                    RecordDonation();
                    Console.WriteLine("\nPress Enter to continue.");
                    readResult = Console.ReadLine();
                    break;
                case "2":
                    Console.WriteLine();
                    ViewDonations();
                    Console.WriteLine("\nPress Enter to continue.");
                    readResult = Console.ReadLine();
                    break;
                case "3":
                    Console.WriteLine();
                    SearchDonation();
                    Console.WriteLine("\nPress Enter to continue.");
                    readResult = Console.ReadLine();
                    break;
                case "4":
                    Console.WriteLine();
                    ViewSummary();
                    Console.WriteLine("\nPress Enter to continue.");
                    readResult = Console.ReadLine();
                    break;
                case "5":
                    Console.WriteLine();
                    SetDonationGoal();
                    Console.WriteLine("\nPress Enter to continue.");
                    readResult = Console.ReadLine();
                    break;
                case "6":
                    Console.WriteLine("Thank you for supporting CONSOLE-ATIONS!");
                    break;
                default:
                    Console.WriteLine("Invalid choice! Press Enter to continue.");
                    readResult = Console.ReadLine();
                    break;
            }
        } while (menuChoice != "6");        
    }
}