using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Expressions;
using System;
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
        private int CustomersRowCountInputFile { get; set; } = 0;
        private string CustomersOutputFilePath { get; set; }
        private Dictionary<string, CustomerInfo> CustomersRecords { get; set; } = new Dictionary<string, CustomerInfo>();

        private string AddressesInputFilePath { get; set; }
        private int AddressesRowCountInputFile { get; set; } = 0;
        private string AddressesOutputFilePath { get; set; }
        private Dictionary<string, AddressInfo> AddressesRecords { get; set; } = new Dictionary<string, AddressInfo>();

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
            Console.WriteLine("-- Customers filecreated with name --");
            CommunicateWithUser.ProvideOutputFileName(Path.GetFileName(CustomersOutputFilePath));

            Console.WriteLine("-- Addresses file created with name --");
            CommunicateWithUser.ProvideOutputFileName(Path.GetFileName(AddressesOutputFilePath));
        }

        public void ProcessCustomersFile()
        {
            CountCustomersRowsInputFile();
            CreateCustomersRecords();
            AnonymiseSensitiveInfo_Customers();
            CreateOutputFilepath_Customers();
            WriteCustomersRecords();
        }

        private void CountCustomersRowsInputFile()
        {
            using (var reader = new StreamReader(CustomersInputFilePath))
            using (var csv = new CsvReader(reader, Config))
            {
                while (csv.Read())
                {
                    CustomersRowCountInputFile++;
                }
            }
        }

        private void CreateCustomersRecords()
        {
            using (var reader = new StreamReader(CustomersInputFilePath))
            using (var csv = new CsvReader(reader, Config))
            {
                csv.Context.RegisterClassMap<CustomersInfoMap>();

                while (csv.Read())
                {
                    CustomerInfo customer = csv.GetRecord<CustomerInfo>();

                    bool customerKeyAlreadyExists = CustomersRecords.ContainsKey(customer.entity_id);

                    if (!customerKeyAlreadyExists)
                    {
                        CustomersRecords.Add(customer.entity_id, customer);
                        int currentRow = csv.Context.Parser.RawRow - 1;
                        CommunicateWithUser.ShowReadingProgress(currentRow, CustomersRowCountInputFile, CustomersRecords, null);
                        //Console.WriteLine($"Reading customers record {currentRow} of {CustomersRowCountInputFile}");
                    }
                }

                CommunicateWithUser.ShowLineSeparator();
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
                        CommunicateWithUser.ShowWritingProgress(i, CustomersRecords, null);
                        KeyValuePair<string, CustomerInfo> kvp = CustomersRecords.ElementAt(i);
                        CustomerInfo customer = kvp.Value;
                        csvWriter.WriteRecord(customer);
                        csvWriter.NextRecord();
                    }

                    CommunicateWithUser.ShowLineSeparator();
                }
            }
        }

        public void ProcessAddressesFile()
        {
            CountAddressesRowsInputFile();
            CreateAddressesRecords();
            AnonymiseSensitiveInfo_Addresses();
            CreateOutputFilepath_Addresses();
            WriteAddressesRecords();
        }

        private void CountAddressesRowsInputFile()
        {
            using (var reader = new StreamReader(AddressesInputFilePath))
            using (var csv = new CsvReader(reader, Config))
            {
                while (csv.Read())
                {
                    AddressesRowCountInputFile++;
                }
            }
        }

        private void CreateAddressesRecords()
        {
            using (var reader = new StreamReader(AddressesInputFilePath))
            using (var csv = new CsvReader(reader, Config))
            {
                csv.Context.RegisterClassMap<AddressesInfoMap>();

                while (csv.Read())
                {
                    AddressInfo address = csv.GetRecord<AddressInfo>();
                    bool addressKeyAlreadyExists = AddressesRecords.ContainsKey(address.addressId);

                    if (!addressKeyAlreadyExists)
                    {
                        AddressesRecords.Add(address.addressId, address);
                        int currentRow = csv.Context.Parser.RawRow - 1;
                        //Console.WriteLine($"Reading addresses record {currentRow} of {AddressesRowCountInputFile}");
                        CommunicateWithUser.ShowReadingProgress(currentRow, AddressesRowCountInputFile, null, AddressesRecords);
                    }
                }

                CommunicateWithUser.ShowLineSeparator();
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
                        CommunicateWithUser.ShowWritingProgress(i, null, AddressesRecords);
                        KeyValuePair<string, AddressInfo> kvp = AddressesRecords.ElementAt(i);
                        AddressInfo address = kvp.Value;
                        csvWriter.WriteRecord(address);
                        csvWriter.NextRecord();
                    }

                    CommunicateWithUser.ShowLineSeparator();
                }
            }
        }
    }
}
