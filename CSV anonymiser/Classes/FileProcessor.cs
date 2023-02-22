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

        private string CustomersInputFilePath { get; set; }
        private string CustomersOutputFilePath { get; set; }
        private Dictionary<string, CustomerInfo> CustomersRecords { get; set; }

        private string AddressesInputFilePath { get; set; }
        private string AddressesOutputFilePath { get; set; }
        private Dictionary<string, AddressInfo> AddressesRecords { get; set; }

        public FileProcessor(CsvConfiguration config)
        {
            Config = config;
        }

        public void RequestInputFilePaths()
        {
            Console.WriteLine("-- Customers file path --");
            CustomersInputFilePath = CommunicateWithUser.RequestFilePath();

            Console.WriteLine("-- Addresses file path --");
            AddressesInputFilePath = CommunicateWithUser.RequestFilePath();
        }

        public void ProvideOutputFileNames()
        {
            Console.WriteLine("-- Customers file name --");
            CommunicateWithUser.ProvideOutputFileName(Path.GetFileName(CustomersOutputFilePath));

            Console.WriteLine("-- Addresses file name --");
            CommunicateWithUser.ProvideOutputFileName(Path.GetFileName(AddressesOutputFilePath));
        }

        public void ProcessCustomersFile()
        {
            CreateCustomersRecords();
            AnonymiseSensitiveInfo_Customers();
            CreateOutputFilepath_Customers();
            WriteCustomersRecords();
        }

        private void CreateCustomersRecords()
        {
            using (var reader = new StreamReader(CustomersInputFilePath))
            using (var csv = new CsvReader(reader, Config))
            {
                csv.Context.RegisterClassMap<CustomersInfoMap>();

                CustomersRecords = csv.GetRecords<CustomerInfo>()
                    .DistinctBy(customer => customer.entity_id)
                    .ToDictionary(customer => customer.entity_id, customer => customer);
            }
        }

        private void AnonymiseSensitiveInfo_Customers()
        {
            foreach (KeyValuePair<string, CustomerInfo> pair in CustomersRecords)
            {
                pair.Value.Anonymise();
            }
        }

        private void CreateOutputFilepath_Customers()
        {
            CustomersOutputFilePath = Path.Combine(Path.GetDirectoryName(CustomersInputFilePath), $"{Path.GetFileNameWithoutExtension(CustomersInputFilePath)}-{DateTime.Now.ToFileTime()}.csv");
        }

        private void WriteCustomersRecords()
        {
            using (var writer = new StreamWriter(CustomersOutputFilePath))
            {
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteHeader<CustomerInfo>();
                    csvWriter.NextRecord();

                    for (int i = 0; i < CustomersRecords.Count; i++)
                    {
                        CommunicateWithUser.ShowProgress(CSV_anonymiser.Enums.OperationType.Writing, i, CustomersRecords, null);
                        KeyValuePair<string, CustomerInfo> kvp = CustomersRecords.ElementAt(i);
                        CustomerInfo customer = kvp.Value;
                        csvWriter.WriteRecord(customer);
                        csvWriter.NextRecord();
                    }
                }
            }
        }

        public void ProcessAddressesFile()
        {
            CreateAddressesRecords();
            AnonymiseSensitiveInfo_Addresses();
            CreateOutputFilepath_Addresses();
            WriteAddressesRecords();
        }

        private void CreateAddressesRecords()
        {
            using (var reader = new StreamReader(AddressesInputFilePath))
            using (var csv = new CsvReader(reader, Config))
            {
                csv.Context.RegisterClassMap<AddressesInfoMap>();
                AddressesRecords = csv.GetRecords<AddressInfo>()
                    .DistinctBy(address => address.addressId)
                    .ToDictionary(address => address.addressId, address => address);
            }
        }

        private void AnonymiseSensitiveInfo_Addresses()
        {
            foreach (KeyValuePair<string, AddressInfo> pair in AddressesRecords)
            {
                string currentCustomerId = pair.Value.customerId;
                CustomerInfo currentCustomer;
                bool customerExists = CustomersRecords.TryGetValue(currentCustomerId, out currentCustomer);

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

        private void CreateOutputFilepath_Addresses()
        {
            AddressesOutputFilePath = Path.Combine(Path.GetDirectoryName(AddressesInputFilePath), $"{Path.GetFileNameWithoutExtension(AddressesInputFilePath)}-{DateTime.Now.ToFileTime()}.csv");
        }

        private void WriteAddressesRecords()
        {
            using (var writer = new StreamWriter(AddressesOutputFilePath))
            {
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteHeader<AddressInfo>();
                    csvWriter.NextRecord();

                    for (int i = 0; i < AddressesRecords.Count; i++)
                    {
                        CommunicateWithUser.ShowProgress(CSV_anonymiser.Enums.OperationType.Writing, i, null, AddressesRecords);
                        KeyValuePair<string, AddressInfo> kvp = AddressesRecords.ElementAt(i);
                        AddressInfo address = kvp.Value;
                        csvWriter.WriteRecord(address);
                        csvWriter.NextRecord();
                    }
                }
            }
        }
    }
}
