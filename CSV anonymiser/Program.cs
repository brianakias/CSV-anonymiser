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
                BadDataFound = null,
            };
            FileProcessor processor = new FileProcessor(config);

            processor.CreateInputFilePaths(args);
            processor.ProcessCustomersFile();
            processor.ProcessAddressesFile();
            processor.ProcessSubscriptionsFile();
            processor.ProvideOutputFileNames();
        }
    }
}
