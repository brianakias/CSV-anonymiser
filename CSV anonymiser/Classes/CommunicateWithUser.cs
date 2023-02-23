using CSV_anonymiser.Classes;

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

        public static void ShowReadingProgress(int currentRow, int totalRows, Dictionary<string, CustomerInfo> customersRecords, Dictionary<string, AddressInfo> addressesRecords, Dictionary<string, SubscriptionInfo> subscriptionsRecords)
        {
            bool hasCustomers = customersRecords != null;
            bool hasAddresses = addressesRecords != null;
            bool hasSubscriptions = subscriptionsRecords != null;

            if (hasCustomers)
            {
                Console.WriteLine($"Reading customers record {currentRow} of {totalRows}");
            }

            else if (hasAddresses)
            {
                Console.WriteLine($"Reading addresses record {currentRow} of {totalRows}");
            }

            else if (hasSubscriptions)
            {
                Console.WriteLine($"Reading subscriptions record {currentRow} of {totalRows}");
            }
        }

        public static void ShowWritingProgress(int index, Dictionary<string, CustomerInfo> customersRecords, Dictionary<string, AddressInfo> addressesRecords, Dictionary<string, SubscriptionInfo> subscriptionsRecords)
        {
            bool hasCustomers = customersRecords != null;
            bool hasAddresses = addressesRecords != null;
            bool hasSubscriptions = subscriptionsRecords != null;

            if (hasCustomers)
            {
                int count = customersRecords.Count;
                Console.WriteLine($"Writing customers record {index + 1} of {count}");
            }

            else if (hasAddresses)
            {
                int count = addressesRecords.Count;
                Console.WriteLine($"Writing addresses record {index + 1} of {count}");
            }

            else if (hasSubscriptions)
            {
                int count = subscriptionsRecords.Count;
                Console.WriteLine($"Writing subscriptions record {index + 1} of {count}");
            }
        }
    }
}
