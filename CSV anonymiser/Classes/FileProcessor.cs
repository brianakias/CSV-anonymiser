using CSV_anonymiser.Classes;
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

        private string InputFilesDirectory { get; set; } = "C:\\Users\\miltiadis.zoumekas\\Desktop\\";

        private string CustomersInputFilePath { get; set; }
        private int CustomersRowCountInputFile { get; set; } = 0;
        private string CustomersOutputFilePath { get; set; }
        private Dictionary<string, CustomerInfo> CustomersRecords { get; set; } = new Dictionary<string, CustomerInfo>();

        private string AddressesInputFilePath { get; set; }
        private int AddressesRowCountInputFile { get; set; } = 0;
        private string AddressesOutputFilePath { get; set; }
        private Dictionary<string, AddressInfo> AddressesRecords { get; set; } = new Dictionary<string, AddressInfo>();

        private string SubscriptionsInputFilePath { get; set; }
        private int SubscriptionsRowCountInputFile { get; set; } = 0;
        private string SubscriptionsOutputFilePath { get; set; }
        private Dictionary<string, SubscriptionInfo> SubscriptionsRecords { get; set; } = new Dictionary<string, SubscriptionInfo>();

        public FileProcessor(CsvConfiguration config)
        {
            Config = config;
        }

        public void CreateInputFilePaths(string[] args)
        {
            string customersFileName = args[0];
            string addressesFileName = args[1];
            string subscriptionsFileName = args[2];

            CustomersInputFilePath = Path.Combine(InputFilesDirectory, customersFileName);
            AddressesInputFilePath = Path.Combine(InputFilesDirectory, addressesFileName);
            SubscriptionsInputFilePath = Path.Combine(InputFilesDirectory, subscriptionsFileName);
        }

        public void ProvideOutputFileNames()
        {
            Console.WriteLine("-- Customers file created with name --");
            CommunicateWithUser.ProvideOutputFileName(Path.GetFileName(CustomersOutputFilePath));

            Console.WriteLine("-- Addresses file created with name --");
            CommunicateWithUser.ProvideOutputFileName(Path.GetFileName(AddressesOutputFilePath));

            Console.WriteLine("-- Subscriptions file created with name --");
            CommunicateWithUser.ProvideOutputFileName(Path.GetFileName(SubscriptionsOutputFilePath));
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
                        CommunicateWithUser.ShowReadingProgress(currentRow, CustomersRowCountInputFile, CustomersRecords, null, null);
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
            CustomersOutputFilePath = Path.Combine(InputFilesDirectory, $"{Path.GetFileNameWithoutExtension(CustomersInputFilePath)}-{DateTime.Now.ToFileTime()}.csv");
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
                        CommunicateWithUser.ShowWritingProgress(i, CustomersRecords, null, null);
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
                        CommunicateWithUser.ShowReadingProgress(currentRow, AddressesRowCountInputFile, null, AddressesRecords, null);
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
            AddressesOutputFilePath = Path.Combine(InputFilesDirectory, $"{Path.GetFileNameWithoutExtension(AddressesInputFilePath)}-{DateTime.Now.ToFileTime()}.csv");
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
                        CommunicateWithUser.ShowWritingProgress(i, null, AddressesRecords, null);
                        KeyValuePair<string, AddressInfo> kvp = AddressesRecords.ElementAt(i);
                        AddressInfo address = kvp.Value;
                        csvWriter.WriteRecord(address);
                        csvWriter.NextRecord();
                    }

                    CommunicateWithUser.ShowLineSeparator();
                }
            }
        }

        public void ProcessSubscriptionsFile()
        {
            CountSubscriptionsRowsInputFile();
            CreateSubscriptionsRecords();
            AnonymiseSensitiveInfo_Subscriptions();
            CreateOutputFilepath_Subscriptions();
            WriteSubscriptionsRecords();
        }

        private void CountSubscriptionsRowsInputFile()
        {
            using (var reader = new StreamReader(SubscriptionsInputFilePath))
            using (var csv = new CsvReader(reader, Config))
            {
                while (csv.Read())
                {
                    SubscriptionsRowCountInputFile++;
                }
            }
        }

        private void CreateSubscriptionsRecords()
        {
            using (var reader = new StreamReader(SubscriptionsInputFilePath))
            using (var csv = new CsvReader(reader, Config))
            {
                csv.Context.RegisterClassMap<SubscriptionInfoMap>();

                while (csv.Read())
                {
                    SubscriptionInfo subscription = csv.GetRecord<SubscriptionInfo>();
                    bool subscriptionKeyAlreadyExists = SubscriptionsRecords.ContainsKey(subscription.customer_id);

                    if (!subscriptionKeyAlreadyExists)
                    {
                        SubscriptionsRecords.Add(subscription.customer_id, subscription);
                        int currentRow = csv.Context.Parser.RawRow - 1;
                        CommunicateWithUser.ShowReadingProgress(currentRow, SubscriptionsRowCountInputFile, null, null, SubscriptionsRecords);
                    }
                }

                CommunicateWithUser.ShowLineSeparator();
            }
        }

        private void AnonymiseSensitiveInfo_Subscriptions()
        {
            foreach (KeyValuePair<string, SubscriptionInfo> pair in SubscriptionsRecords)
            {
                string currentCustomerId = pair.Value.customer_id;
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

        private void CreateOutputFilepath_Subscriptions()
        {
            SubscriptionsOutputFilePath = Path.Combine(InputFilesDirectory, $"{Path.GetFileNameWithoutExtension(SubscriptionsInputFilePath)}-{DateTime.Now.ToFileTime()}.csv");
        }

        private void WriteSubscriptionsRecords()
        {
            using (var writer = new StreamWriter(SubscriptionsOutputFilePath))
            {
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteHeader<SubscriptionInfo>();
                    csvWriter.NextRecord();

                    for (int i = 0; i < SubscriptionsRecords.Count; i++)
                    {
                        CommunicateWithUser.ShowWritingProgress(i, null, null, SubscriptionsRecords);
                        KeyValuePair<string, SubscriptionInfo> kvp = SubscriptionsRecords.ElementAt(i);
                        SubscriptionInfo subscription = kvp.Value;
                        csvWriter.WriteRecord(subscription);
                        csvWriter.NextRecord();
                    }

                    CommunicateWithUser.ShowLineSeparator();
                }
            }
        }

    }
}
