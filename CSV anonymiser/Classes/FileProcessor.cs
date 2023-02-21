using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Expressions;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace CsvAnonymiser.Classes
{
    public class FileProcessor
    {
        private CsvConfiguration Config { get; set; }

        public FileProcessor(CsvConfiguration config)
        {
            Config = config;
        }

        public List<string> RequestInputFilePaths()
        {
            Console.WriteLine("-- Customers file path --");
            string customersFilePath = CommunicateWithUser.RequestFilePath();
            Console.WriteLine("-- Addresses file path --");
            string addressesFilePath = CommunicateWithUser.RequestFilePath();
            List<string> filePaths = new List<string>() { customersFilePath, addressesFilePath };
            return filePaths;
        }

        public Dictionary<string, CustomerInfo> ProcessCustomersFile(string customersFilePath)
        {
            Dictionary<string, CustomerInfo> records = GetCustomersRecords(customersFilePath);
            AnonymiseSensitiveInfo_CustomersFile(records);
            //WriteRecords(records);
            WriteRecords(customersFilePath, records);
            return records;
        }

        private Dictionary<string, CustomerInfo> GetCustomersRecords(string customersFilePath)
        {
            using (var reader = new StreamReader(customersFilePath))
            using (var csv = new CsvReader(reader, Config))
            {
                csv.Context.RegisterClassMap<CustomersInfoMap>();
                Dictionary<string, CustomerInfo> records = csv.GetRecords<CustomerInfo>()
                    .DistinctBy(customer => customer.entity_id)
                    .ToDictionary(customer => customer.entity_id, customer => customer);
                return records;
            }
        }

        private void AnonymiseSensitiveInfo_CustomersFile(Dictionary<string, CustomerInfo> customerRecords)
        {
            foreach (KeyValuePair<string, CustomerInfo> pair in customerRecords)
            {
                pair.Value.Anonymise();
            }
        }

        private Dictionary<string, CustomerInfo> WriteRecords(string customersFilePath, Dictionary<string, CustomerInfo> customerRecords)
        {
            var csvPath = Path.Combine(Path.GetDirectoryName(customersFilePath), $"{Path.GetFileNameWithoutExtension(customersFilePath)}-{DateTime.Now.ToFileTime()}.csv");
            using (var writer = new StreamWriter(csvPath))
            {
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteHeader<CustomerInfo>();
                    csvWriter.NextRecord();

                    foreach (CustomerInfo customer in customerRecords.Values)
                    {
                        csvWriter.WriteRecord(customer);
                        csvWriter.NextRecord();
                    }
                }
            }

            return customerRecords;
        }


        public void ProcessAddressesFile(string addressesFilePath, Dictionary<string, CustomerInfo> customerRecords)
        {
            Dictionary<string, CustomerAddressInfo> addressRecords = GetAddressRecords(addressesFilePath);
            AnonymiseSensitiveInfo_AddressesFile(addressRecords, customerRecords);
            //WriteRecords(addressRecords);
            WriteRecords(addressesFilePath, addressRecords);
        }

        private Dictionary<string, CustomerAddressInfo> GetAddressRecords(string addressesFilePath)
        {
            using (var reader = new StreamReader(addressesFilePath))
            using (var csv = new CsvReader(reader, Config))
            {
                csv.Context.RegisterClassMap<CustomerAddressInfoMap>();
                var records = csv.GetRecords<CustomerAddressInfo>()
                    .DistinctBy(address => address.addressId)
                    .ToDictionary(address => address.addressId, address => address);
                return records;
            }
        }

        private void AnonymiseSensitiveInfo_AddressesFile(Dictionary<string, CustomerAddressInfo> customerAddressRecords, Dictionary<string, CustomerInfo> customerRecords)
        {

            foreach (KeyValuePair<string, CustomerAddressInfo> pair in customerAddressRecords)
            {
                string currentCustomerId = pair.Value.customerId;
                CustomerInfo currentCustomer;
                bool customerExists = customerRecords.TryGetValue(currentCustomerId, out currentCustomer);

                if (customerExists)
                {
                    pair.Value.Anonymise(currentCustomer);
                }

                else
                {
                    pair.Value.Anonymise();
                }
            }
        }

        private void WriteRecords(string addressesFilePath, Dictionary<string, CustomerAddressInfo> customerAddressRecords)
        {
            var csvPath = Path.Combine(Path.GetDirectoryName(addressesFilePath), $"{Path.GetFileNameWithoutExtension(addressesFilePath)}-{DateTime.Now.ToFileTime()}.csv");
            using (var writer = new StreamWriter(csvPath))
            {
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteHeader<CustomerAddressInfo>();
                    csvWriter.NextRecord();

                    foreach (CustomerAddressInfo customer in customerAddressRecords.Values)
                    {
                        csvWriter.WriteRecord(customer);
                        csvWriter.NextRecord();
                    }
                }
            }
        }
    }
}
