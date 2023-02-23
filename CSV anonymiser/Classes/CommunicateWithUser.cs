namespace CsvAnonymiser.Classes
{
    public static class CommunicateWithUser
    {
        public static void ShowLineSeparator()
        {
            Console.WriteLine(new string('*', Console.WindowWidth - 2));
        }

        public static void ProvideOutputFileName(string fileName)
        {
            Console.WriteLine(fileName);
            ShowLineSeparator();
        }

        public static void ShowReadingProgress(int currentRow, int totalRows, Dictionary<string, CustomerInfo> customersRecords, Dictionary<string, AddressInfo> addressesRecords)
        {
            bool hasCustomers = customersRecords != null;
            bool hasAddresses = addressesRecords != null;

            if (!hasCustomers && !hasAddresses)
            {
                Console.WriteLine("No records passed. Cannot show progress.");
            }

            else if (hasCustomers && hasAddresses)
            {
                Console.WriteLine("Both customers and addresses passed. Cannot understand for which of the two to show progress.");
            }

            else if (hasCustomers && !hasAddresses)
            {
                Console.WriteLine($"Reading customers record {currentRow} of {totalRows}");
            }

            else if (!hasCustomers && hasAddresses)
            {
                Console.WriteLine($"Reading addresses record {currentRow} of {totalRows}");
            }
        }

        public static void ShowWritingProgress(int index, Dictionary<string, CustomerInfo> customersRecords, Dictionary<string, AddressInfo> addressesRecords)
        {
            bool hasCustomers = customersRecords != null;
            bool hasAddresses = addressesRecords != null;

            if (!hasCustomers && !hasAddresses)
            {
                Console.WriteLine("No records passed. Cannot show progress.");
            }

            else if (hasCustomers && hasAddresses)
            {
                Console.WriteLine("Both customers and addresses passed. Cannot understand for which of the two to show progress.");
            }

            else if (hasCustomers && !hasAddresses)
            {
                int count = customersRecords.Count;
                Console.WriteLine($"Writing customers record {index + 1} of {count}");
            }

            else if (!hasCustomers && hasAddresses)
            {
                int count = addressesRecords.Count;
                Console.WriteLine($"Writing addresses record {index + 1} of {count}");
            }
        }
    }
}
