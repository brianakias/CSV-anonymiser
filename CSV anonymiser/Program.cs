using CsvAnonymiser.Classes;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace CsvAnonymiser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                Encoding = Encoding.UTF8,
                Delimiter = ",",
            };
            FileProcessor processor = new FileProcessor(config);

            List<string> filePaths = processor.RequestInputFilePaths();
            string customersFilePath = filePaths[0];
            string addressesFilePath = filePaths[1];

            Dictionary<string, CustomerInfo> customerRecords = processor.ProcessCustomersFile(customersFilePath);
            processor.ProcessAddressesFile(addressesFilePath, customerRecords);
        }
    }
}
