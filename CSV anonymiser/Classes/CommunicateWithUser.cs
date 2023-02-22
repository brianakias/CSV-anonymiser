using CSV_anonymiser.Enums;

namespace CsvAnonymiser.Classes
{
    public static class CommunicateWithUser
    {
        public static string RequestFilePath()
        {
            string filePath;

            do
            {
                Console.WriteLine("Provide the path of the file to read from: ");
                filePath = Console.ReadLine();

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Could not locate file with path `{filePath}`. Please retry.");
                }

            } while (!File.Exists(filePath));

            Console.WriteLine("\nThanks, file exists.");
            Console.WriteLine(new string('*', Console.WindowWidth - 2));

            return filePath;
        }

        public static void ProvideOutputFileName(string fileName)
        {
            Console.WriteLine(fileName);
            Console.WriteLine(new string('*', Console.WindowWidth - 2));
        }

        public static void ShowProgress(OperationType type, int index, Dictionary<string, CustomerInfo> customersRecords, Dictionary<string, AddressInfo> addressesRecords)
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
                Console.WriteLine($"{type.ToString()} progress: (current customer/total customers) ({index + 1}/{count})");
            }

            else if (!hasCustomers && hasAddresses)
            {
                int count = addressesRecords.Count;
                Console.WriteLine($"{type.ToString()} progress: (current address/total addresses) ({index + 1}/{count})");
            }
        }
    }
}
