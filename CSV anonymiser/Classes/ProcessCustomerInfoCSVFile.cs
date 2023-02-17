using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using System.Text;

namespace CSV_anonymiser.Classes
{
    public class ProcessCustomerInfoCSVFile
    {
        public ProcessCustomerInfoCSVFile()
        {
            // put all the logic here and ideally just by instantiating this class, it runs the whole thing
            string inputFilePath = CommunicateWithUser.RequestFilePath("input");
            List<CustomerInfo> records = GetCSVRecords(inputFilePath);
            AnonymiseSensitiveInfo(records);
            WriteCSVRecords(records);
            // nneed to comme up with a way to return the records so that they cacn be used for working on the address ccsv file
        }
        private List<CustomerInfo> GetCSVRecords(string inputFilePath)
        {
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                Encoding = Encoding.UTF8,
                Delimiter = ",",
            };

            using (var fileStream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new StreamReader(fileStream, Encoding.UTF8))
            using (var csv = new CsvReader(reader, configuration))
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
                customerInfo.ReplaceSensitiveInfoWithFakeData();
            }
        }

        private void WriteCSVRecords(List<CustomerInfo> records)
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
