using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace CSV_anonymiser.Classes
{
    public class ProcessCustomerInfoCSVFile
    {
        public ProcessCustomerInfoCSVFile()
        {
            string inputFilePath = CommunicateWithUser.RequestFilePath("input");
            List<CustomerInfo> records = GetRecords(inputFilePath);
            AnonymiseSensitiveInfo(records);
            WriteRecords(records);
            // nneed to comme up with a way to return the records so that they can be used for working on the address ccsv file
        }
        private List<CustomerInfo> GetRecords(string inputFilePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                Encoding = Encoding.UTF8,
                Delimiter = ",",
            };

            //using (var fileStream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //using (var reader = new StreamReader(fileStream, Encoding.UTF8))
            //using (var csv = new CsvReader(reader, config))
            using (var reader = new StreamReader(inputFilePath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<CustomerInfoMap>();
                var records = csv.GetRecords<CustomerInfo>().ToList();
                return records;
            }
        }

        private void AnonymiseSensitiveInfo(List<CustomerInfo> records)
        {
            foreach (CustomerInfo customerInfo in records)
            {
                customerInfo.Anonymise();
            }
        }

        private void WriteRecords(List<CustomerInfo> records)
        {
            var csvPath = Path.Combine(Environment.CurrentDirectory, $"customers_sample-{DateTime.Now.ToFileTime()}.csv");
            using (var writer = new StreamWriter(csvPath))
            {
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteRecords(records);
                }
            }
        }

    }
}
